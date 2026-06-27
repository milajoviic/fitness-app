using Cassandra;
using ISession = Cassandra.ISession;
using FitnessApp.Entities;
using FitnessApp.Enums;

namespace FitnessApp.DataProvider
{
    public class UserDP
    {
        private readonly ISession _session;

        private readonly PreparedStatement _insertByEmail;
        private readonly PreparedStatement _insertById;
        private readonly PreparedStatement _selectByEmail;
        private readonly PreparedStatement _selectById;
        private readonly PreparedStatement _updateByEmail;
        private readonly PreparedStatement _updateById;
        private readonly PreparedStatement _deleteByEmail;
        private readonly PreparedStatement _deleteById;

        public UserDP()
        {
            _session = SessionManager.GetSession();

            _insertByEmail = _session.Prepare(
                "INSERT INTO user_by_email " +
                "(email, user_id, password_hash, first_name, last_name, gender, birth_date) " +
                "VALUES (?, ?, ?, ?, ?, ?, ?)");

            _insertById = _session.Prepare(
                "INSERT INTO user_by_id " +
                "(user_id, email, password_hash, first_name, last_name, gender, birth_date) " +
                "VALUES (?, ?, ?, ?, ?, ?, ?)");

            _selectByEmail = _session.Prepare(
               "SELECT email, user_id, password_hash, first_name, last_name, gender, birth_date " +
               "FROM user_by_email WHERE email = ?");

            _selectById = _session.Prepare(
                "SELECT email, user_id, password_hash, first_name, last_name, gender, birth_date " +
                "FROM user_by_id WHERE user_id = ?");

            _updateByEmail = _session.Prepare(
               "UPDATE user_by_email SET first_name = ?, last_name = ?, gender = ?, birth_date = ? " +
               "WHERE email = ?");

            _updateById = _session.Prepare(
                "UPDATE user_by_id SET first_name = ?, last_name = ?, gender = ?, birth_date = ? " +
                "WHERE user_id = ?");

            _deleteByEmail = _session.Prepare("DELETE FROM user_by_email WHERE email = ?");
            _deleteById = _session.Prepare("DELETE FROM user_by_id WHERE user_id = ?");
        }

        public async Task<User?> GetByEmail(string email)
        {
            RowSet rows = await _session.ExecuteAsync(_selectByEmail.Bind(email));
            Row row = rows.FirstOrDefault();
            return row == null ? null : MapUser(row);
        }
        public async Task<User?> GetById(Guid id)
        {
            RowSet rows = await _session.ExecuteAsync(_selectById.Bind(id));
            Row row = rows.FirstOrDefault();
            return row == null ? null : MapUser(row);
        }

        public async Task AddAsync(User u)
        {
            LocalDate db = ToLocalDate(u.BirthDate);
            string gender = u.Gender.ToString();

            await _session.ExecuteAsync(_insertByEmail.Bind(
                u.Email, u.UserId, u.PasswordHash, u.FirstName, u.LastName, gender, db));
            await _session.ExecuteAsync(_insertById.Bind(
                u.UserId, u.Email, u.PasswordHash, u.FirstName, u.LastName, gender, db));
        }
        public async Task UpdateAsync(User u)
        {
            LocalDate db = ToLocalDate(u.BirthDate);
            string gender = u.Gender.ToString();

            await _session.ExecuteAsync(_updateByEmail.Bind(
                u.FirstName, u.LastName, gender, db, u.Email));
            await _session.ExecuteAsync(_updateById.Bind(
                u.FirstName, u.LastName, gender, db, u.UserId));
        }
        public async Task DeleteAsync(Guid userId, string email)
        {
            await _session.ExecuteAsync(_deleteByEmail.Bind(email));
            await _session.ExecuteAsync(_deleteById.Bind(userId));
        }

        private static User MapUser(Row row)
        {
            LocalDate local = row.GetValue<LocalDate>("birth_date");
            return new User
            {
                Email = row.GetValue<string>("email"),
                UserId = row.GetValue<Guid>("user_id"),
                PasswordHash = row.GetValue<string>("password_hash"),
                FirstName = row.GetValue<string>("first_name"),
                LastName = row.GetValue<string>("last_name"),
                Gender = Enum.Parse<GenderEnum>(row.GetValue<string>("gender")),
                BirthDate = new DateTime(local.Year, local.Month, local.Day)
            };

        }
        private static LocalDate ToLocalDate(DateTime dt) => new LocalDate(dt.Year, dt.Month, dt.Day);
    }
}
