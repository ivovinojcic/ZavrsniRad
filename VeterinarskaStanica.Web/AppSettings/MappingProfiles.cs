using AutoMapper;
using VeterinarskaStanica.Model.Core;
using VeterinarskaStanica.Model.Model.User;

namespace VeterinarskaStanica.Web.AppSettings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<RegisterForm, User>();
        }
    }
}
