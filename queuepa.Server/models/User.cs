namespace queuepa.Server.models
{
    public class User
    {
        public int Id { get; set; }
        public string TwitchId { get; set; }
        public DateTime LastSeen { get; set; }
        public string Role { get; set; }
        public ICollection<Queue> Queues { get; set; } = new List<Queue>();
        public ICollection<Song> Songs { get; set; } = new List<Song>();
        public int? StreamerId { get; set; } 
        public ICollection<User> ModeratorOf { get; set; } = new List<User>(); 
    }


}
