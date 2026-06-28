using Cassandra;
using ISession = Cassandra.ISession;

namespace FitnessApp.DataProvider
{
    public class RefreshTokenDP
    {
        private readonly ISession _session;
        private readonly PreparedStatement _insert;
        private readonly PreparedStatement _select;
        private readonly PreparedStatement _delete;

        public RefreshTokenDP()
        {
            _session = SessionManager.GetSession();
            _insert = _session.Prepare(
                "INSERT INTO refresh_tokens (refresh_token, user_id, expires_at) " +
                "VALUES (?, ?, ?)"
            );
            _select = _session.Prepare(
                "SELECT user_id, expires_at FROM refresh_tokens WHERE refresh_token = ?"    
            );
            _delete = _session.Prepare(
                "DELETE FROM refresh_tokens WHERE refresh_token = ?"    
            );
        }
        public async Task SaveAsync(string token, Guid userId, DateTimeOffset expiresAt)
        {
            await _session.ExecuteAsync(_insert.Bind(token, userId, expiresAt));
        }
        public async Task<(Guid userId, DateTimeOffset expiresAt)?> GetAsync(string token)
        {
            RowSet rows = await _session.ExecuteAsync(_select.Bind(token));
            Row row = rows.FirstOrDefault();
            if (row == null) return null;
            return (row.GetValue<Guid>("user_id"), row.GetValue<DateTimeOffset>("expires_at"));
        }
        public async Task DeleteAsync(string token) =>
            await _session.ExecuteAsync(_delete.Bind(token));
    }
}
