using IntensApp.Models;
using IntensApp.Repositories;

namespace IntensApp.Services
{
    public class SkillService : ISkillService
    {
        private readonly ISkillRepository _skillRepository;

        public SkillService(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }
        public async Task<bool> AddSkillAsync(Skill skill)
        {
            return await _skillRepository.AddSkillAsync(skill);
        }
    }
}
