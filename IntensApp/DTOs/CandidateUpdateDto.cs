namespace IntensApp.DTOs
{
    public class CandidateUpdateDto
    {
        public string? FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? ContactNumber { get; set; }
        public string? Email { get; set; }
        
        public List<string>? Skills { get; set; }
    }
}
