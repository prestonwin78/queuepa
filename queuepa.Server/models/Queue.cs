namespace queuepa.Server.models
{
    public class Queue
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StreamerId { get; set; }
        public User Streamer { get; set; }
        public ICollection<Song> Songs { get; set; } = new List<Song>();
        public int MaxSongs { get; set; }
    }
}

