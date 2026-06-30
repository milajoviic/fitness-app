using Cassandra;
using ISession = Cassandra.ISession;
using FitnessApp.Entities;
using FitnessApp.Enums;

namespace FitnessApp.DataProvider
{
    public class BodyMetricsDP
    {
        private readonly ISession _session;

        private readonly PreparedStatement _insert;
        private readonly PreparedStatement _delete;
        private readonly PreparedStatement _select;

        public BodyMetricsDP()
        {
            _session = SessionManager.GetSession();

            _insert = _session.Prepare(
                "INSERT INTO body_metrics_by_user " +
                "(user_id, body_part, metric_id, recorded_at, value) " +
                "VALUES (?, ?, ?, ?, ?)"
            );
            _delete = _session.Prepare(
                "DELETE FROM body_metrics_by_user " +
                "WHERE user_id = ? AND body_part = ? AND metric_id = ?"
            );
            _select = _session.Prepare(
                "SELECT user_id, body_part, metric_id, recorded_at, value " +
                "FROM body_metrics_by_user WHERE user_id = ? AND body_part = ?"
            );
        }

        public async Task<List<UserBodyMetrics>> GetBodyMetrics(Guid userId, string bodyPart)
        {
            RowSet rows = await _session.ExecuteAsync(_select.Bind(userId, bodyPart));
            var metrics = new List<UserBodyMetrics>();

            foreach(Row r in rows)
            {
                metrics.Add(new UserBodyMetrics
                {
                    UserId = r.GetValue<Guid>("user_id"),
                    BodyPart = r.GetValue<string>("body_part"),
                    MetricId = r.GetValue<Guid>("metric_id"),
                    RecordedAt = r.GetValue<DateTimeOffset>("recorded_at"),
                    Value = r.GetValue<decimal>("value")
                });
            }
            return metrics;
        }
        public async Task InsertAsync(UserBodyMetrics bm)
        {
            bm.MetricId = TimeUuid.NewId();
            bm.RecordedAt = DateTimeOffset.UtcNow;
            BoundStatement statement = _insert.Bind(
                bm.UserId, bm.BodyPart, bm.MetricId, bm.RecordedAt, bm.Value);
            await _session.ExecuteAsync(statement);
        }
        public async Task DeleteAsync(Guid userId, string bodyPart, Guid metricId)
        {
            await _session.ExecuteAsync(_delete.Bind(userId, bodyPart, metricId));
        }
    }
}
