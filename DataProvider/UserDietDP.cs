using Cassandra;
using FitnessApp.Entities;
using FitnessApp.Enums;
using ISession = Cassandra.ISession;

namespace FitnessApp.DataProvider
{
    public class UserDietDP
    {
        private readonly ISession _session;

        private readonly PreparedStatement _setBreakfast;
        private readonly PreparedStatement _setDinner;
        private readonly PreparedStatement _setLunch;
        private readonly PreparedStatement _setCalories;

        private readonly PreparedStatement _addSnack;
        private readonly PreparedStatement _removeSnack;
        private readonly PreparedStatement _addSupplement;
        private readonly PreparedStatement _removeSupplement;


        private readonly PreparedStatement _deleteDay;
        private readonly PreparedStatement _select;
        private readonly PreparedStatement _selectAllDays;

        public UserDietDP()
        {
            _session = SessionManager.GetSession();

            _setBreakfast = _session.Prepare(
                "UPDATE diet_by_user SET breakfast = ? WHERE user_id = ? AND log_day = ?"
            );
            _setDinner = _session.Prepare(
                "UPDATE diet_by_user SET dinner = ? WHERE user_id = ? AND log_day = ?"
            );
            _setLunch = _session.Prepare(
                "UPDATE diet_by_user SET lunch = ? WHERE user_id = ? AND log_day = ?"
            );
            _setCalories = _session.Prepare(
                "UPDATE diet_by_user SET calories = ? WHERE user_id = ? AND log_day = ?"
            );
            _addSnack = _session.Prepare(
                "UPDATE diet_by_user SET snacks = snacks + ? WHERE user_id = ? AND log_day = ?"
            );
            _removeSnack = _session.Prepare(
                "UPDATE diet_by_user SET snacks = snacks - ? WHERE user_id = ? AND log_day = ?"
            );
            _addSupplement = _session.Prepare(
                "UPDATE diet_by_user SET supplements = supplements + ? WHERE user_id = ? AND log_day = ?"
            );
            _removeSupplement = _session.Prepare(
                "UPDATE diet_by_user SET supplements = supplements - ? WHERE user_id = ? AND log_day = ?"
            );
            _deleteDay = _session.Prepare(
                "DELETE FROM diet_by_user " +
                "WHERE user_id = ? AND log_day = ?"
            );
            _select = _session.Prepare(
                "SELECT user_id, log_day, breakfast, calories, dinner, lunch, snacks, supplements " +
                "FROM diet_by_user WHERE user_id = ? AND log_day = ?"
            );
            _selectAllDays = _session.Prepare(
                "SELECT user_id, log_day, breakfast, calories, dinner, lunch, snacks, supplements " +
                "FROM diet_by_user WHERE user_id = ?"
            );
        }
        public async Task<UserDiet?> GetByDay(Guid userId, DateTime logDay)
        {
            RowSet rows = await _session.ExecuteAsync(_select.Bind(userId, ToLocalDate(logDay)));
            Row row = rows.FirstOrDefault();
            return row == null ? null : MapDiet(row);
        }
        public async Task<List<UserDiet>> GetAllDays(Guid userId)
        {
            RowSet rows = await _session.ExecuteAsync(_selectAllDays.Bind(userId));
            var results = new List<UserDiet>();
            foreach (Row r in rows)
                results.Add(MapDiet(r));
            return results;
        }
        public async Task DeleteDayAsync(Guid userId, DateTime logDay)
        {
            await _session.ExecuteAsync(_deleteDay.Bind(userId, ToLocalDate(logDay)));
        }

        public async Task SetBreakfastAsync(Guid userId, DateTime day, string breakfast) =>
            await _session.ExecuteAsync(_setBreakfast.Bind(breakfast, userId, ToLocalDate(day)));
        public async Task SetDinnerAsync(Guid userId, DateTime day, string dinner) =>
            await _session.ExecuteAsync(_setDinner.Bind(dinner, userId, ToLocalDate(day)));
        public async Task SetLunchAsync(Guid userId, DateTime day, string lunch) =>
            await _session.ExecuteAsync(_setLunch.Bind(lunch, userId, ToLocalDate(day)));
        public async Task SetCaloriesAsync(Guid userId, DateTime day, int calories) =>
            await _session.ExecuteAsync(_setCalories.Bind(calories, userId, ToLocalDate(day)));


        public async Task AddSnackAsync(Guid userId, DateTime day, string snack) =>
            await _session.ExecuteAsync(_addSnack.Bind(
                new List<string> { snack }, userId, ToLocalDate(day)));

        public async Task RemoveSnackAsync(Guid userId, DateTime day, string snack) =>
            await _session.ExecuteAsync(_removeSnack.Bind(
                new List<string> { snack }, userId, ToLocalDate(day)));

        public async Task AddSupplementAsync(Guid userId, DateTime day, SupplementsEnum supplement) =>
           await _session.ExecuteAsync(_addSupplement.Bind(
               new List<string> { supplement.ToString() }, userId, ToLocalDate(day)));

        public async Task RemoveSupplementAsync(Guid userId, DateTime day, SupplementsEnum supplement) =>
            await _session.ExecuteAsync(_removeSupplement.Bind(
                new List<string> { supplement.ToString() }, userId, ToLocalDate(day)));


        private static UserDiet MapDiet(Row r)
        {
            LocalDate ld = r.GetValue<LocalDate>("log_day");
            return new UserDiet
            {
                UserId = r.GetValue<Guid>("user_id"),
                LogDay = new DateTime(ld.Year, ld.Month, ld.Day),
                Breakfast = r.GetValue<string>("breakfast"),
                Lunch = r.GetValue<string>("lunch"),
                Dinner = r.GetValue<string>("dinner"),
                Snacks = r.GetValue<List<string>>("snacks"),
                Supplements = r.GetValue<List<string>>("supplements")          
                ?.Select(s => Enum.Parse<SupplementsEnum>(s))   
                 .ToList(),
                Calories = r.GetValue<int?>("calories")
            };
        }
        private static LocalDate ToLocalDate(DateTime dt) =>
            new LocalDate(dt.Year, dt.Month, dt.Day);
    }
}
