using AutoMapper;
using IntensApp.DTOs;
using IntensApp.Models;

namespace IntensApp.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<CandidateCreateDto, Candidate>()
                .ForMember(dest => dest.CandidateSkills, opt => opt.MapFrom(src =>
                    src.CandidateSkills.Select(cs => new CandidateSkill { Skill = new Skill { Name = cs.Name } })));

            CreateMap<Candidate, CandidateReadDto>()
                .ForMember(dest => dest.Skills, opt => opt.MapFrom(src =>
                    src.CandidateSkills.Select(cs => cs.Skill.Name)));

            CreateMap<CandidateUpdateDto, Candidate>()
                .ForMember(dest => dest.CandidateSkills, opt => opt.Ignore());

            CreateMap<SkillCreateDto, Skill>();
        }
    }
}
