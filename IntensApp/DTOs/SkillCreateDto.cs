using System.ComponentModel.DataAnnotations;

namespace IntensApp.DTOs
{
    public class SkillCreateDto
    {
        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
    }
}
