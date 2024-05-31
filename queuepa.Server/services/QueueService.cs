using Microsoft.EntityFrameworkCore;
using queuepa.Server.models;
using System.Drawing;

namespace queuepa.Server.services
{
    public class QueueService
    {
        private readonly QueuepaContext _context;

        public QueueService(QueuepaContext context)
        {
            _context = context;
        }

        public async Task AddSongToQueue(int queueId, int userId, string songTitle, string songUrl)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new InvalidOperationException("User not found.");
            }

            var queue = await _context.Queues.Include(q => q.Songs).FirstOrDefaultAsync(q => q.Id == queueId);
            if (queue == null)
            {
                throw new InvalidOperationException("Queue not found.");
            }

            // Check if user can add a song
            var songsInQueue = queue.Songs.Count;
            if (songsInQueue >= queue.MaxSongs)
            {
                throw new InvalidOperationException("Queue is full.");
            }

            var userLastSong = queue.Songs
                .Where(s => s.AddedBy == user)
                .OrderByDescending(s => s.AddedAt)
                .FirstOrDefault();

            if (userLastSong != null && (DateTime.Now - userLastSong.AddedAt).TotalMinutes < Math.Max(1, songsInQueue / 10))
            {
                throw new InvalidOperationException("You can only add one song every few minutes.");
            }

            var song = new Song
            {
                Title = songTitle,
                Url = songUrl,
                AddedAt = DateTime.Now,
                AddedBy = user,
                QueueId = queueId
            };

            queue.Songs.Add(song);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveSongFromQueue(int songId)
        {
            var song = await _context.Songs.FindAsync(songId);
            if (song != null)
            {
                _context.Songs.Remove(song);
                await _context.SaveChangesAsync();
            }
        }

        public async Task PromoteSong(int songId, int positions)
        {
            var song = await _context.Songs.FindAsync(songId);
            if (song != null)
            {
                song.Position += positions;
                await _context.SaveChangesAsync();
            }
        }

        public async Task HighlightSong(int songId, Color color)
        {
            var song = await _context.Songs.FindAsync(songId);
            if (song != null)
            {
                song.Color = color;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> CheckUserStillHere(int songId)
        {
            var song = await _context.Songs.FindAsync(songId);
            if (song != null)
            {
                if ((DateTime.Now - song.AddedAt).TotalMinutes > 30 && !song.Highlighted)
                {
                    // Trigger "Are you still here" message logic here
                    // If user does not respond in 5 minutes, return false
                    return false;
                }
                return true;
            }
            return false;
        }

        public async Task<bool> RefreshUserTimeout(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                user.LastSeen = DateTime.Now;
                return true;
            }
            return false;
        }

        public async Task CreateQueue(int streamerId, string queueName)
        {
            var streamer = await _context.Users.FindAsync(streamerId);
            if (streamer != null)
            {
                var queue = new Queue
                {
                    Name = queueName,
                    StreamerId = streamerId
                };
                streamer.Queues.Add(queue);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteQueue(int queueId)
        {
            var queue = await _context.Queues.FindAsync(queueId);
            if (queue != null)
            {
                _context.Queues.Remove(queue);
                await _context.SaveChangesAsync();
            }
        }
    }

}
