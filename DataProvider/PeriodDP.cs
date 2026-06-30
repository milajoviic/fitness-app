using Cassandra;
using ISession = Cassandra.ISession;
using FitnessApp.Entities;
using FitnessApp.Enums;

namespace FitnessApp.DataProvider
{
    public class PeriodDP
    {
        private readonly ISession _session;
        private readonly PreparedStatement _insert;
        private readonly PreparedStatement _delete;
        private readonly PreparedStatement _select;
        private readonly PreparedStatement _setNotes;

        public PeriodDP()
        {
            _session = SessionManager.GetSession();

            _insert = _session.Prepare(
                "INSERT INTO period_by_user " +
                "(user_id, start_date, end_date, notes) " +
                "VALUES (?, ?, ?, ?)"
            );
            _delete = _session.Prepare(
                "DELETE FROM period_by_user WHERE user_id = ? AND start_date = ?"    
            );
            _select = _session.Prepare(
                "SELECT user_id, start_date, end_date, notes FROM period_by_user " +
                "WHERE user_id = ?"
            );
            _setNotes = _session.Prepare(
                "UPDATE period_by_user SET notes = ? WHERE user_id = ? AND start_date = ?"    
            );
        }

        public async Task AddPeriodAsync(PeriodByUser p)
        {
            LocalDate start = ToLocalDate(p.StartDate);
            LocalDate? end = p.EndDate == null 
                ? ToLocalDate(p.StartDate.AddDays(7)) 
                : ToLocalDate(p.EndDate.Value);
            await _session.ExecuteAsync(_insert.Bind(p.UserId, start, end, p.Notes));
        }

        public async Task<List<PeriodByUser>> GetByUser(Guid userId)
        {
            RowSet rows = await _session.ExecuteAsync(_select.Bind(userId));
            var list = new List<PeriodByUser>();
            foreach(Row r in rows)
            {
                LocalDate startDate = r.GetValue<LocalDate>("start_date");
                LocalDate? endDate = r.GetValue<LocalDate?>("end_date");

                list.Add(new PeriodByUser
                {
                    UserId = r.GetValue<Guid>("user_id"),
                    StartDate = new DateTime(startDate.Year, startDate.Month, startDate.Day),
                    EndDate = new DateTime(endDate.Year, endDate.Month, endDate.Day),
                    Notes = r.GetValue<string?>("notes")
                });
            }
            return list;
        }

        public async Task SetNotesAsync(Guid userId, string notes) =>
            await _session.ExecuteAsync(_setNotes.Bind(notes, userId));

        public async Task DeleteAsync(Guid userId, DateTime sd) =>
            await _session.ExecuteAsync(_delete.Bind(userId, ToLocalDate(sd)));

        private static LocalDate ToLocalDate(DateTime date) =>
            new LocalDate(date.Year, date.Month, date.Day);

        public CycleEnum GetCurrentPhase(DateTime lastStartDate)
        {
            int day = (DateTime.UtcNow.Date - lastStartDate.Date).Days % 28 + 1;
            if (day <= 5) return CycleEnum.Menstrual;
            if (day <= 13) return CycleEnum.Follicular;
            if (day == 14) return CycleEnum.Ovulation;
            return CycleEnum.Luteal;
        }
    }
}
