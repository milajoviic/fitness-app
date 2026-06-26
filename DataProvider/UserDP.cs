using Cassandra;
using ISession = Cassandra.ISession;
using FitnessApp.Entities;
using FitnessApp.Enums;

namespace FitnessApp.DataProvider
{
    public class UserDP
    {
        private readonly ISession _session;

        private readonly PreparedStatement _insert;
        private readonly PreparedStatement _selectByEmail;
    }
}
