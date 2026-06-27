using Cassandra;
using ISession = Cassandra.ISession;
using FitnessApp.Entities;
using FitnessApp.Enums;

namespace FitnessApp.DataProvider
{
    public class UserWorkoutDP
    {
        private readonly ISession _session;

        private readonly PreparedStatement _insert;
        private readonly PreparedStatement _delete;
        private readonly PreparedStatement _update;
        private readonly PreparedStatement _selectByUser;

        public UserWorkoutDP()
        {
            _session = SessionManager.GetSession();

            _insert = _session.Prepare(
                "INSERT INTO workouts_by_user " +
                "(user_id, workout_date, workout_id, is_rest_day, notes, workout_type) " +
                "VALUES (?, ?, ?, ?, ?, ?)"
                );

            _delete = _session.Prepare(
                "DELETE FROM workouts_by_user " +
                "WHERE user_id = ? AND workout_date = ? AND workout_id = ?"
                );

            _selectByUser = _session.Prepare(
                "SELECT user_id, workout_date, workout_id, is_rest_day, notes, workout_type " +
                "FROM workouts_by_user WHERE user_id = ?"
                );

            _update = _session.Prepare(
                "UPDATE workouts_by_user SET workout_type = ?, is_rest_day = ?, notes = ? " +
                "WHERE user_id = ? AND workout_date = ? AND workout_id = ?"
                );
        }
        public async Task<List<UserWorkouts>> GetWorkoutByUser(Guid userId)
        {
            RowSet rows = await _session.ExecuteAsync(_selectByUser.Bind(userId));
            var workouts = new List<UserWorkouts>();
            foreach(Row r in rows)
            {
                workouts.Add(new UserWorkouts
                {
                    UserId = r.GetValue<Guid>("user_id"),
                    WorkoutDate = r.GetValue<DateTimeOffset>("workout_date"),
                    WorkoutId = r.GetValue<Guid>("workout_id"),
                    IsRestDay = r.GetValue<bool>("is_rest_day"),
                    Notes = r.GetValue<string>("notes"),
                    TypeOfWorkout = Enum.Parse<WorkoutType>(r.GetValue<string>("workout_type"))
                });
            }
            return workouts;
        }

        public async Task InsertAsync(UserWorkouts w)
        {
            w.WorkoutId = TimeUuid.NewId();
            BoundStatement statement = _insert.Bind(
                w.UserId, w.WorkoutDate, w.WorkoutId, w.IsRestDay, w.Notes, w.TypeOfWorkout.ToString());
            await _session.ExecuteAsync(statement);
        }

        public async Task UpdateAsync(UserWorkouts w)
        {
            BoundStatement statement = _update.Bind(
                w.TypeOfWorkout.ToString(), w.IsRestDay, w.Notes,
                w.UserId, w.WorkoutDate, w.WorkoutId);
            await _session.ExecuteAsync(statement);
        }

        public async Task DeleteAsync(Guid userId, DateTimeOffset workoutDate, Guid workoutId)
        {
            await _session.ExecuteAsync(_delete.Bind(userId, workoutDate, workoutId));
        }
    }
}
