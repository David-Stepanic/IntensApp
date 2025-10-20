using IntensApp.Data;
using IntensApp.Models;
using Microsoft.EntityFrameworkCore;

namespace IntensApp.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        private readonly AppDbContext _context;

        public SkillRepository(AppDbContext context)
        {
            _context = context; 
        }
        public async Task<bool> AddSkillAsync(Skill skill)
        {
            var existingSkill = await _context.Skills
                .FirstOrDefaultAsync(s => s.Name.ToLower() == skill.Name.ToLower());

            if (existingSkill == null)
            {
                await _context.Skills.AddAsync(skill);
                await _context.SaveChangesAsync();

                return true;
            }
            return false;
        }
    }
}
