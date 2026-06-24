using Cassandra;
using System;
using System.IO;

namespace FitnessApp
{
    public static class SessionManager
    {
        private static Cassandra.ISession _session;
        private static readonly object padlock = new object();

        public static Cassandra.ISession GetSession()
        {
            if(_session == null)
            {
                lock (padlock)
                {
                    if (_session == null)
                        _session = CreateSession();
                }
            }
            return _session;
        }

        private static Cassandra.ISession CreateSession()
        {
            Cluster _cluster = Cluster.Builder().
                AddContactPoint("127.0.0.1").
                WithPort(9042).
                Build();
            return _cluster.Connect("fitness_app");
        }
    }
}
