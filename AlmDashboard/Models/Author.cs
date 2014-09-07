namespace AlmDashboard.Models
{
    public class Author
    {
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string UniqueName { get; set; }
    }

    public class AuthorWithUrl : Author
    {
        public string Url { get; set; }
    }
}