using IntensApp.Models;
using IntensApp.Services;
using IntensApp.DTOs;
using IntensApp.Repositories;
using Moq;

namespace IntensApp.Tests
{
    public class CandidateServiceTests
    {
        [Fact]
        public async Task AddCandidateAsync_ShouldAddCandidate_WhenEmailDoesNotExist()
        {
            var mockRepo = new Mock<ICandidateRepository>();
            mockRepo.Setup(r => r.CandidateExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
            mockRepo.Setup(r => r.GetSkillByNameAsync(It.IsAny<string>())).ReturnsAsync((Skill?)null);
            var service = new CandidateService(mockRepo.Object);

            var candidate = new Candidate
            {
                FullName = "Jovan Jovanovic",
                Email = "jovan@gmail.com",
                ContactNumber = "061222333",
                CandidateSkills = new List<CandidateSkill>
        {
            new CandidateSkill { Skill = new Skill { Name = "  C#  " } },
            new CandidateSkill { Skill = new Skill { Name = "" } } 
        }
            };

            var result = await service.AddCandidateAsync(candidate);

            Assert.True(result);
            Assert.Single(candidate.CandidateSkills); 
            Assert.Equal("C#", candidate.CandidateSkills.First().Skill.Name);
            mockRepo.Verify(r => r.AddCandidateAsync(candidate), Times.Once);
        }


        [Fact]
        public async Task UpdateCandidateAsync_ShouldUpdateSkills_CaseInsensitive()
        {
            var mockRepo = new Mock<ICandidateRepository>();
            var candidate = new Candidate
            {
                Id = 1,
                FullName = "Jovan Jovanovic",
                CandidateSkills = new List<CandidateSkill>
                {
                    new CandidateSkill { Skill = new Skill { Name = "Java" } }
                }
            };
            mockRepo.Setup(r => r.GetCandidateAsync(1)).ReturnsAsync(candidate);
            mockRepo.Setup(r => r.GetSkillByNameAsync(It.IsAny<string>())).ReturnsAsync((Skill?)null);
            var service = new CandidateService(mockRepo.Object);

            var dto = new CandidateUpdateDto
            {
                Skills = new List<string> { "JAVA", "C#" } 
            };

            var updated = await service.UpdateCandidateAsync(1, dto);

            Assert.NotNull(updated);
            Assert.Contains(updated!.CandidateSkills, cs => cs.Skill.Name == "Java");
            Assert.Contains(updated.CandidateSkills, cs => cs.Skill.Name == "C#");
            Assert.Equal(2, updated.CandidateSkills.Count);
        }

        [Fact]
        public async Task DeleteCandidateSkillAsync_ShouldRemoveSkill_CaseInsensitive()
        {
            var mockRepo = new Mock<ICandidateRepository>();
            var skill = new Skill { Name = "Python", Id = 1 };
            var candidate = new Candidate
            {
                Id = 1,
                CandidateSkills = new List<CandidateSkill>
        {
            new CandidateSkill { Skill = skill }
        }
            };
            mockRepo.Setup(r => r.GetCandidateAsync(1)).ReturnsAsync(candidate);

            mockRepo.Setup(r => r.DeleteCandidateSkillAsync(It.IsAny<CandidateSkill>()))
                    .Callback<CandidateSkill>(cs => candidate.CandidateSkills.Remove(cs))
                    .Returns(Task.CompletedTask);

            var service = new CandidateService(mockRepo.Object);

            var result = await service.DeleteCandidateSkillAsync(1, "python");

            Assert.True(result.CandidateExists);
            Assert.True(result.SkillDeleted);
            Assert.Empty(candidate.CandidateSkills);
            mockRepo.Verify(r => r.DeleteCandidateSkillAsync(It.IsAny<CandidateSkill>()), Times.Once);
        }


        [Fact]
        public async Task SearchCandidatesAsync_ShouldLowerSkillsBeforeSearch()
        {
            var mockRepo = new Mock<ICandidateRepository>();
            mockRepo.Setup(r => r.SearchCandidatesAsync("Jovan", It.Is<List<string>>(l => l.SequenceEqual(new List<string> { "c#", "java" }))))
                    .ReturnsAsync(new List<Candidate> { new Candidate { FullName = "Jovan Jovanovic" } });
            var service = new CandidateService(mockRepo.Object);

            var result = await service.SearchCandidatesAsync("Jovan", new List<string> { "C#", "Java" });

            Assert.Single(result);
            Assert.Equal("Jovan Jovanovic", result[0].FullName);
        }
    }
}
