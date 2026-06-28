using Cassandra;
using ISession = Cassandra.ISession;
using FitnessApp.Entities;
using FitnessApp.Enums;

namespace FitnessApp.DataProvider
{
    public class UserHealthDP
    {
        private readonly ISession _session;

        private readonly PreparedStatement _addHeight;
        private readonly PreparedStatement _addWeight;
        private readonly PreparedStatement _addChronic;
      
        private readonly PreparedStatement _setChronic;
        private readonly PreparedStatement _delete;
        private readonly PreparedStatement _selectInfo;
        private readonly PreparedStatement _addNotes;

        public UserHealthDP()
        {
            _session = SessionManager.GetSession();

            _addHeight = _session.Prepare(
                "UPDATE user_health SET height_cm = ? WHERE user_id = ?"   
            );
            _addWeight = _session.Prepare(
                "UPDATE user_health SET weight_kg = ? WHERE user_id = ?"  
            );
            _addChronic = _session.Prepare(
                "UPDATE user_health SET chronic_cond = chronic_cond + ? WHERE user_id = ?"    
            );
            _setChronic = _session.Prepare(
                "UPDATE user_health SET chronic_cond = ? WHERE user_id = ?"    
            );
            _delete = _session.Prepare(
                "DELETE FROM user_health " +
                "WHERE user_id = ?"
            );
            _selectInfo = _session.Prepare(
                "SELECT user_id, chronic_cond, height_cm, notes, weight_kg " +
                "FROM user_health WHERE user_id = ?"
            );
            _addNotes = _session.Prepare(
                "UPDATE user_health SET notes = ? WHERE user_id = ?"    
            );
        }

        public async Task DeleteAsync(Guid userId)
        {
            await _session.ExecuteAsync(_delete.Bind(userId));
        }
        public async Task SetHeightAsync(Guid userId, decimal height) =>
            await _session.ExecuteAsync(_addHeight.Bind(height, userId));
        public async Task SetWeightAsync(Guid userId, decimal weight) =>
            await _session.ExecuteAsync(_addWeight.Bind(weight, userId));
        public async Task SetChronicCondAsync(Guid userId, List<ChronicCondEnum> cc) =>
            await _session.ExecuteAsync(_setChronic.Bind(cc.Select(c=>c.ToString()).ToList(), userId));
        public async Task AddChronicCondAsync(Guid userId, ChronicCondEnum cc) =>
            await _session.ExecuteAsync(_addChronic.Bind(
                new List<string> { cc.ToString() }, userId));
        public async Task SetNotesAsync(Guid userId, string note) =>
            await _session.ExecuteAsync(_addNotes.Bind(note, userId));

        public async Task<UserHealth?> GetByUser(Guid userId)
        {
            RowSet rows = await _session.ExecuteAsync(_selectInfo.Bind(userId));
            Row row = rows.FirstOrDefault();
            if (row == null)
                return null;
            return new UserHealth
            {
                UserId = row.GetValue<Guid>("user_id"),
                Height = row.GetValue<decimal?>("height_cm"),
                Weight = row.GetValue<decimal?>("weight_kg"),
                Notes = row.GetValue<string?>("notes"),
                ChronicCond = row.GetValue<List<string>>("chronic_cond")
                ?.Select(s => Enum.Parse<ChronicCondEnum>(s))
                 .ToList()
            };
        }

        public async Task<decimal?> GetBmiAsync(Guid userId)
        {
            var user = await GetByUser(userId);

            if (user.Height == null)
                return null;
            if (user.Height <= 0 || user.Weight <= 0)
                return null;

            decimal? heightM = user.Height / 100;
            decimal? bmi = user.Weight / (heightM * heightM);
            return Math.Round((decimal)bmi, 1);
        }
    }
}
