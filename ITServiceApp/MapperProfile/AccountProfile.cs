using AutoMapper;
using ITServiceApp.Models.Identity;
using ITServiceApp.ViewModels;

namespace ITServiceApp.MapperProfile
{
    public class AccountProfile:Profile
    {
        public AccountProfile()
        {
            CreateMap<ApplicationUser , UserProfileViewModel>().ReverseMap();
        }
    }
}
