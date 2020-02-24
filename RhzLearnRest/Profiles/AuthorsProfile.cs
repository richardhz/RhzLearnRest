using AutoMapper;
using RhzLearnRest.Domains.Models;
using RhzLearnRest.Domains.Models.Dtos;
using RhzLearnRest.Extensions;

namespace RhzLearnRest.Profiles
{
    public class AuthorsProfile : Profile
    {
        public AuthorsProfile()
        {
            CreateMap<Author, AuthorDto>()
                .ForMember(
                    dest => dest.Name,
                    op => op.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(
                    dest => dest.Age,
                    op => op.MapFrom(src => src.DateOfBirth.CurrentAge()));
        }
    }
}
