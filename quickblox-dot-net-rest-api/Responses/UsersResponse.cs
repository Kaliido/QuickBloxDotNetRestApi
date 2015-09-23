namespace Kaliido.QuickBlox.Responses
{
    public class UsersResponse
    {
        public int current_page { get; set; }
        public int per_page { get; set; }
        public int total_entries { get; set; }
        public LocationUserItems[] items { get; set; }
    }
}