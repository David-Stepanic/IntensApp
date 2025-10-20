using IntensApp.Models;

namespace IntensApp.Repositories
{
    public interface ISkillRepository
    {
        public Task<bool> AddSkillAsync(Skill skill);
    }
}
