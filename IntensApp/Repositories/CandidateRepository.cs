using IntensApp.Data;
using IntensApp.Models;
using Microsoft.EntityFrameworkCore;

namespace IntensApp.Repositories
{
    public class CandidateRepository : ICandidateRepository
    {
        private readonly AppDbContext _context;

        public CandidateRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Skill?> GetSkillByNameAsync(string name)
        {
            return await _context.Skills
                .FirstOrDefaultAsync(s => s.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> CandidateExistsAsync(string email)
        {
            return await _context.Candidates
                .AnyAsync(c => c.Email.ToLower().Trim() == email.ToLower().Trim());
        }

        public async Task AddSkillAsync(Skill skill)
        {
            await _context.Skills.AddAsync(skill);
        }

        public async Task AddCandidateAsync(Candidate candidate)
        {
            await _context.Candidates.AddAsync(candidate);
        }
        public async Task<Candidate?> GetCandidateAsync(int id)
        {
            return await _context.Candidates
                .Include(c => c.CandidateSkills)
                .ThenInclude(cs => cs.Skill)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<List<Candidate>> GetAllCandidatesAsync()
        {
            return await _context.Candidates
                 .Include(c => c.CandidateSkills)
                 .ThenInclude(cs => cs.Skill)
                 .ToListAsync();
        }
        public Task DeleteCandidateAsync(Candidate candidate)
        {
            _context.Candidates.Remove(candidate);
            return Task.CompletedTask;
        }
        public Task DeleteCandidateSkillAsync(CandidateSkill candidateSkill)
        {
            _context.CandidateSkills.Remove(candidateSkill);
            return Task.CompletedTask;
        }

        public async Task<List<Candidate>> SearchCandidatesAsync(string? name, List<string>? skills)
        {
            var query = _context.Candidates
                .Include(c => c.CandidateSkills)
                .ThenInclude(cs => cs.Skill)
                .AsQueryable();

            if(!string.IsNullOrWhiteSpace(name))
                query = query.Where(c => c.FullName.ToLower().Contains(name.ToLower()));
            // allows us to add where condition because of queryable

            if(skills != null && skills.Count > 0)
            {
                query = query.Where(c =>
                    c.CandidateSkills.Any(cs =>
                        skills.Any(skill =>
                            cs.Skill.Name.ToLower().Contains(skill))));
            }

            return await query.ToListAsync();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
