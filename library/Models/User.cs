using library.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace library.Models
{
    public class User : IdentityUser
    {
       

       

        public string FullName { get; set; }
    }
    
}
