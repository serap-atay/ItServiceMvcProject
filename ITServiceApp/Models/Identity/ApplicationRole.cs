using Microsoft.AspNetCore.Identity;

namespace ITServiceApp.Models.Identity
{
    public class ApplicationRole:IdentityRole
    {
        public ApplicationRole()
        {

        }
        public ApplicationRole(string name,string description)
        {
            this.Name = name;
            this.Description = description;
        }
        public string Description { get; set; }
    }
}
