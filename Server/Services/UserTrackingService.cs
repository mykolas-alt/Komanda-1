using System.Collections.Concurrent;

namespace Projektas.Server.Services {
    public class UserTrackingService {
        private readonly ConcurrentDictionary<string, DateTime> _activeUsers = new();

        public void AddOrUpdateUser(string username) {
            _activeUsers[username] = DateTime.Now;

            var users = GetActiveUsers();

            var formattedUsers = users.Select(user => new {
                Username = user.Key,
                LastActive = user.Value.ToString("yyyy-MM-dd HH:mm:ss")
            });

            foreach( var user in formattedUsers ) {
                Console.WriteLine(user.Username + " Last Active: " + user.LastActive);
            }
        }

        public bool RemoveUser(string username) {
            Console.WriteLine("Removed: " + username);
            return _activeUsers.TryRemove(username, out _);
        }

        public IEnumerable<KeyValuePair<string, DateTime>> GetActiveUsers() {
            return _activeUsers;
        }
    }
}
