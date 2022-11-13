using AutoMapper;

namespace NZWalks.Profiles
{
    public class WalksProfile : Profile
    {
        public WalksProfile()
        {
            CreateMap<Models.Dormain.Walk, Models.DTO.Walk>().ReverseMap();
            CreateMap<Models.Dormain.WalkDifficulty, Models.DTO.WalkDifficulty>().ReverseMap();
        }
    }
}
