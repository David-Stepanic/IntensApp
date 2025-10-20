using IntensApp.DTOs;
using IntensApp.Models;

namespace IntensApp.Services
{
    public interface ISkillService
    {
        public Task<bool> AddSkillAsync(Skill skill);
    }
}
