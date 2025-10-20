namespace IntensApp.DTOs
{
    public class CandidateReadDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string ContactNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<string> Skills { get; set; } = new();
    }
}
