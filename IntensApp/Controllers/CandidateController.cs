using AutoMapper;
using IntensApp.DTOs;
using IntensApp.Models;
using IntensApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntensApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidateController : Controller
    {
        private readonly ICandidateService _candidateService;
        private readonly IMapper _mapper;

        public CandidateController(ICandidateService candidateService, IMapper mapper)
        {
            _candidateService = candidateService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> AddCandidate(CandidateCreateDto dto)
        {
            if (dto.DateOfBirth > DateTime.UtcNow)
                return BadRequest("Invalid date of birth.");

            var candidate = _mapper.Map<Candidate>(dto);

            var added = await _candidateService.AddCandidateAsync(candidate);

            if (!added)
                return BadRequest("Candidate already exists!");

            return Ok("Successfully created candidate");
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Candidate>> GetCandidateAsync(int id)
        {
            var candidate = await _candidateService.GetCandidateAsync(id);

            if (candidate == null)
                return NotFound("Candidate not found.");

            var dto = _mapper.Map<CandidateReadDto>(candidate);

            return Ok(dto);
        }

        [HttpGet]
        public async Task<ActionResult<List<Candidate>>> GetAllCandidatesAsync()
        {
            var candidates = await _candidateService.GetAllCandidatesAsync();
            if (candidates.Count == 0 || !candidates.Any())
                return NotFound("No candidates found.");

            var dtos = _mapper.Map<List<CandidateReadDto>>(candidates);
            return Ok(dtos);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<CandidateReadDto>>> SearchCandidate(
            [FromQuery] string? name,
            [FromQuery] List<string>? skills)
        {
            var candidates = await _candidateService.SearchCandidatesAsync(name, skills);

            if (candidates == null || !candidates.Any())
                return NotFound("No candidates match your search criteria.");
            var dto = _mapper.Map<List<CandidateReadDto>>(candidates);
            return Ok(dto);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult<Candidate>> UpdateCandidateAsync(int id, CandidateUpdateDto dto)
        {
            if (dto.DateOfBirth > DateTime.UtcNow)
                return BadRequest("Invalid date of birth.");

            var updatedCandidate = await _candidateService.UpdateCandidateAsync(id, dto);
            if (updatedCandidate == null)
                return NotFound("Failed to update candidate!");

            var readDto = _mapper.Map<CandidateReadDto>(updatedCandidate);
            return Ok(readDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCandidate(int id)
        {
            var deleted = await _candidateService.DeleteCandidateAsync(id);
            if (!deleted)
                return NotFound("Candidate not found.");

            return Ok("Candidate deleted successfully.");
        }

        [HttpDelete("skill/{id}")]
        public async Task<IActionResult> DeleteCandidateSkill(int id, string skillName)
        {
            if (string.IsNullOrWhiteSpace(skillName))
                return BadRequest("Skill name is required.");

            var (candidateExists, skillDeleted) = await _candidateService.DeleteCandidateSkillAsync(id, skillName);

            if (!candidateExists)
                return NotFound("Candidate not found.");

            if (!skillDeleted)
                return BadRequest("Candidate does not have this skill.");

            return Ok("Candidate skill deleted successfully.");
        }

    }
}
