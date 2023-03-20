using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Profiles
{
    public class WalksProfile : Profile
    {
        public WalksProfile()
        {
            CreateMap<Walk, WalkDTO>().ReverseMap();
            CreateMap<Walk, CreateWalkDTO>().ReverseMap();
            CreateMap<Walk, UpdateWalkDTO>().ReverseMap();

            CreateMap<WalkDifficulty, WalkDifficultyDTO>().ReverseMap();
        }
    }
}
