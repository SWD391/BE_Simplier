namespace Controllers
{
    using AutoMapper;
    using BusinessObjects.Models;

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AssignedDetail, AssignedDetailDTO>();
            CreateMap<AssignedDetailDTO, AssignedDetail>();
            CreateMap<FixTask, FixTaskDTO>()
             .ForMember(pts => pts.AssignedDetails, opt => opt.MapFrom(ps => ps.AssignedDetails));
            CreateMap<FixTaskDTO, FixTask>()
                .ForMember(pts => pts.AssignedDetails, opt => opt.MapFrom(ps => ps.AssignedDetails));
        }
    }
}
