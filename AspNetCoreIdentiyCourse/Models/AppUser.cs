using Microsoft.AspNetCore.Identity;

namespace WebUI.Models
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string Picture { get; set; }
        public int Gender { get; set; }
    }
}
