using queuepa.Server.models;

namespace queuepa.Server
{
    public class QueuepaDbInitializer
    {
        public static void SeedDatabase(QueuepaContext context)
        {
            context.Database.EnsureCreated();

            var users = new User[]
            {
                new User { Id = 1, TwitchId = "streamer1", Role = "Streamer" },
                new User { Id = 2, TwitchId = "moderator1", Role = "Moderator", StreamerId = 1 },
                new User { Id = 3, TwitchId = "viewer1", Role = "Viewer" }
            };

            foreach (var u in users)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();

            var queues = new Queue[]
            {
                new Queue { Id = 1, Name = "Main Queue", StreamerId = 1 }
            };

            foreach (var q in queues)
            {
                context.Queues.Add(q);
            }
            context.SaveChanges();

            var songs = new Song[]
            {
                new Song { Id = 1, Title = "Song 1", Url = "https://www.youtube.com/watch?v=dQw4w9WgXcQ", AddedAt = DateTime.Now, QueueId = 1, AddedBy = users[0] }
            };

            foreach (var s in songs)
            {
                context.Songs.Add(s);
            }
            context.SaveChanges();
        }
    }
}
