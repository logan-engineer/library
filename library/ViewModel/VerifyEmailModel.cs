using System.ComponentModel.DataAnnotations;

namespace library.ViewModel
{
    public class VerifyEmailModel
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress]
        public string Email { get; set; }
    }
}
