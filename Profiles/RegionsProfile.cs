using AutoMapper;

namespace NZWalks.Profiles
{
    public class RegionsProfile : Profile
    {
        public RegionsProfile()
        {
            CreateMap<Models.Dormain.Region, Models.DTO.Region>().ReverseMap();
        }
    }
}
