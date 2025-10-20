using AutoMapper;
using IntensApp.DTOs;
using IntensApp.Models;
using IntensApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace IntensApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SkillController : Controller
    {
        private readonly ISkillService _service;
        private readonly IMapper _mapper;
        public SkillController(ISkillService service, IMapper mapper) 
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> AddSkillAsync(SkillCreateDto dto)
        {
            var skill = _mapper.Map<Skill>(dto);
            var added = await _service.AddSkillAsync(skill);
            if(!added)
                return BadRequest("This skill already exists!");
            return Ok("Successfully created Skill!");
        }

    }
}
