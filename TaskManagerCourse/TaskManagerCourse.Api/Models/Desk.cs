namespace TaskManagerCourse.Api.Models
{
    public class Desk:CommonObject
    {
        public bool IsPrivat { get; set; }

        public string? Columns { get; set; }

        public User? Admin { get; set; }
    }
}
