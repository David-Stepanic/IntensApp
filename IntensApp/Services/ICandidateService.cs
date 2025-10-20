using IntensApp.DTOs;
using IntensApp.Models;

namespace IntensApp.Services
{
    public interface ICandidateService
    {
        public Task<bool> AddCandidateAsync(Candidate candidate);
        public Task<Candidate?> GetCandidateAsync(int id);
        public Task<List<Candidate>> GetAllCandidatesAsync();
        public Task<List<Candidate>> SearchCandidatesAsync(string? name, List<string>? skills);
        public Task<Candidate?> UpdateCandidateAsync(int id, CandidateUpdateDto dto);
        public Task<bool> DeleteCandidateAsync(int id);
        public Task<(bool CandidateExists, bool SkillDeleted)> DeleteCandidateSkillAsync(int id, string skillName);
 
    }
}
