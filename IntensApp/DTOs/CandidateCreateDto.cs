using IntensApp.Models;
using System.ComponentModel.DataAnnotations;

namespace IntensApp.DTOs
{
    public class CandidateCreateDto
    {
        [Required(ErrorMessage = "FullName is required.")]
        [MaxLength(50)]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "DateOfBirth is required.")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "ContactNumber is required.")]
        [MaxLength(15)]
        public string ContactNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        public List<CandidateSkillDto> CandidateSkills { get; set; } = new();
    }
}
