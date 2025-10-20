using IntensApp.DTOs;
using IntensApp.Models;
using IntensApp.Repositories;

namespace IntensApp.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly ICandidateRepository _candidateRepository;

        public CandidateService(ICandidateRepository candidateRepository)
        {
            _candidateRepository = candidateRepository;
        }
        public void NormalizeCandidate(Candidate candidate)
        {
            candidate.FullName = candidate.FullName.Trim();
            candidate.Email = candidate.Email.Trim().ToLower();
            candidate.ContactNumber = candidate.ContactNumber.Trim();

            candidate.CandidateSkills = candidate.CandidateSkills
                .Where(cs => !string.IsNullOrWhiteSpace(cs.Skill.Name))
                .ToList();

            foreach (var cs in candidate.CandidateSkills)
            {
                cs.Skill.Name = cs.Skill.Name.Trim();
            }

            candidate.CandidateSkills = candidate.CandidateSkills
                .GroupBy(cs => cs.Skill.Name.Trim().ToLower())
                .Select(g => g.First())
                .ToList();
        }

        public async Task<bool> AddCandidateAsync(Candidate candidate)
        {
            NormalizeCandidate(candidate);

            if (await _candidateRepository.CandidateExistsAsync(candidate.Email))
                return false;

            foreach (var candidateSkill in candidate.CandidateSkills)
            {
                var skill = await _candidateRepository.GetSkillByNameAsync(candidateSkill.Skill.Name);
                if (skill == null)
                {
                    if (!await _candidateRepository.SkillExistsAsync(candidateSkill.Skill.Name))
                    {
                        var newSkill = new Skill { Name = candidateSkill.Skill.Name };
                        await _candidateRepository.AddSkillAsync(newSkill);
                        skill = newSkill;
                    }
                }
                candidateSkill.SkillId = skill!.Id;
                candidateSkill.Skill = skill;
            }

            await _candidateRepository.AddCandidateAsync(candidate);
            await _candidateRepository.SaveChangesAsync();
            return true;
        }
        public async Task<Candidate?> GetCandidateAsync(int id)
        {
            return await _candidateRepository.GetCandidateAsync(id);    
        }
        public async Task<List<Candidate>> GetAllCandidatesAsync()
        {
            return await _candidateRepository.GetAllCandidatesAsync();
        }
        public async Task<bool> DeleteCandidateAsync(int id)
        {
            var candidate = await GetCandidateAsync(id);
            if (candidate == null)
                return false;

            await _candidateRepository.DeleteCandidateAsync(candidate);
            await _candidateRepository.SaveChangesAsync();
            return true;
        }

        public async Task<Candidate?> UpdateCandidateAsync(int id, CandidateUpdateDto dto)
        {
            var candidate = await _candidateRepository.GetCandidateAsync(id);
            if (candidate == null) return null;

            candidate.FullName = dto.FullName ?? candidate.FullName;
            candidate.Email = dto.Email ?? candidate.Email;
            candidate.ContactNumber = dto.ContactNumber ?? candidate.ContactNumber;
            candidate.DateOfBirth = dto.DateOfBirth ?? candidate.DateOfBirth;

            

            if (dto.Skills != null)
            {
                dto.Skills = dto.Skills
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Select(s => s.Trim())
                    .ToList();

                if (dto.Skills.Count == 0)
                {
                    candidate.CandidateSkills.Clear();
                }
                else
                {
                    candidate.CandidateSkills = candidate.CandidateSkills
                        .Where(cs => dto.Skills.Contains(cs.Skill.Name, StringComparer.OrdinalIgnoreCase))
                        .ToList();

                    foreach (var skillName in dto.Skills)
                    {
                        if (!candidate.CandidateSkills
                                .Any(cs => cs.Skill.Name.Equals(skillName, StringComparison.OrdinalIgnoreCase)))
                        {
                            var skill = await _candidateRepository.GetSkillByNameAsync(skillName);
                            if (skill == null)
                            {
                                skill = new Skill { Name = skillName };
                                await _candidateRepository.AddSkillAsync(skill);
                            }

                            candidate.CandidateSkills.Add(new CandidateSkill
                            {
                                CandidateId = candidate.Id,
                                SkillId = skill.Id,
                                Skill = skill
                            });
                        }
                    }

                    candidate.CandidateSkills = candidate.CandidateSkills
                        .OrderBy(cs => dto.Skills.FindIndex(s =>
                                 s.Equals(cs.Skill.Name, StringComparison.OrdinalIgnoreCase)))
                        .ToList();
                }
            }

            await _candidateRepository.SaveChangesAsync();
            return candidate;
        }

        public async Task<(bool CandidateExists, bool SkillDeleted)> DeleteCandidateSkillAsync(int id, string skillName)
        {
            var candidate = await GetCandidateAsync(id);

            if (candidate == null)
                return (false, false);

            var candidateSkill = candidate.CandidateSkills
                .FirstOrDefault(cs => cs.Skill.Name.ToLower() == skillName.ToLower());

            if (candidateSkill == null)
                return (true, false);

            await _candidateRepository.DeleteCandidateSkillAsync(candidateSkill);
            await _candidateRepository.SaveChangesAsync();
            return (true, true);
        }

        public async Task<List<Candidate>> SearchCandidatesAsync(string? name, List<string>? skills)
        {
            List<string>? loweredSkills = skills?.Select(s => s.ToLower()).ToList();

            return await _candidateRepository.SearchCandidatesAsync(name, loweredSkills);
        }
    }
}
