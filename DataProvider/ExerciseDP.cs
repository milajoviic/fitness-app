using Cassandra;
using FitnessApp.Entities;
using FitnessApp.Enums;
using ISession = Cassandra.ISession;

namespace FitnessApp.DataProvider
{
    public class ExerciseDP
    {
        private readonly ISession _session;

        private readonly PreparedStatement _insert;
        private readonly PreparedStatement _delete;
        private readonly PreparedStatement _update;
        private readonly PreparedStatement _selectByWorkout;

        public ExerciseDP()
        {
            _session = SessionManager.GetSession();

            _insert = _session.Prepare("INSERT INTO exercises_by_workout " +
                "(workout_id, exc_order, name, reps, rest_minutes, sets, weight_kg) " +
                "VALUES (?, ?, ?, ?, ?, ?, ?)");
            _delete = _session.Prepare(
                "DELETE FROM exercises_by_workout " +
                "WHERE workout_id = ? AND exc_order = ?"
                );
            _update = _session.Prepare(
                "UPDATE exercises_by_workout SET name = ?, reps = ?, rest_minutes = ?, sets = ?, weight_kg = ? " +
                "WHERE workout_id = ? AND exc_order = ?"
                );
            _selectByWorkout = _session.Prepare(
                "SELECT workout_id, exc_order, name, reps, rest_minutes, sets, weight_kg " +
                "FROM exercises_by_workout WHERE workout_id = ?"
                );
        }

        public async Task<List<ExercisesByWorkout>> GetByWorkout(Guid wId)
        {
            RowSet rows = await _session.ExecuteAsync(_selectByWorkout.Bind(wId));
            var exercises = new List<ExercisesByWorkout>();
            foreach(Row row in rows)
            {
                exercises.Add(new ExercisesByWorkout
                {
                    WorkoutId = row.GetValue<Guid>("workout_id"),
                    ExcOrder = row.GetValue<int>("exc_order"),
                    Name = row.GetValue<string>("name"),
                    Reps = row.GetValue<int>("reps"),
                    RestMinutes = row.GetValue<int>("rest_minutes"),
                    Sets = row.GetValue<int>("sets"),
                    WeightKg = row.GetValue<decimal>("weight_kg")
                });
            }
            return exercises;
        }

        public async Task InsertAsync(ExercisesByWorkout ew)
        {
            var numExc = await GetByWorkout(ew.WorkoutId);
            ew.ExcOrder = numExc.Count + 1;
            BoundStatement statement = _insert.Bind(
                ew.WorkoutId, ew.ExcOrder, ew.Name, ew.Reps, ew.RestMinutes, ew.Sets, ew.WeightKg);
            await _session.ExecuteAsync(statement);
        }

        public async Task UpdateAsync(ExercisesByWorkout ew)
        {
            BoundStatement statement = _update.Bind(
                ew.Name, ew.Reps, ew.RestMinutes, ew.Sets, ew.WeightKg,
                ew.WorkoutId, ew.ExcOrder);
            await _session.ExecuteAsync(statement);
        }

        public async Task DeleteAsync(Guid workoutId, int excOrder)
        {
            await _session.ExecuteAsync(_delete.Bind(workoutId, excOrder));
        }
    }
}
