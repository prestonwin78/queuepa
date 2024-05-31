using System.Drawing;

namespace queuepa.Server.models
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public DateTime AddedAt { get; set; }
        public bool Highlighted { get; set; }
        public int QueueId { get; set; }
        public Queue Queue { get; set; }
        public User AddedBy { get; set; }
        public int Position { get; set; }
        public Color Color { get; set; }
    }

}