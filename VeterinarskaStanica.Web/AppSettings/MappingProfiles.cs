using AutoMapper;
using VeterinarskaStanica.Model.Core;
using VeterinarskaStanica.Model.Model.User;
using VeterinarskaStanica.Model.Model.Pet;

namespace VeterinarskaStanica.Web.AppSettings
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<RegisterForm, User>();
            CreateMap<User, RegisterForm>();
            CreateMap<PetModel, Pet>();
            CreateMap<Pet, PetModel>();
        }
    }
}
