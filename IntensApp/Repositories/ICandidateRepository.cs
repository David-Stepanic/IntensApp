using IntensApp.Models;

namespace IntensApp.Repositories
{
    public interface ICandidateRepository
    {
        public Task AddCandidateAsync(Candidate candidate);
        public Task AddSkillAsync(Skill skill);
        public Task<bool> SkillExistsAsync(string skillName);
        public Task<Skill?> GetSkillByNameAsync(string SkillName);
        public Task<bool> CandidateExistsAsync(string email);
        public Task<Candidate?> GetCandidateAsync(int id);
        public Task<List<Candidate>> GetAllCandidatesAsync();
        public Task<List<Candidate>> SearchCandidatesAsync(string? name, List<string>? skills);
        public Task DeleteCandidateAsync(Candidate candidate);
        public Task DeleteCandidateSkillAsync(CandidateSkill candidateSkill);
        public Task SaveChangesAsync();
    }
}
