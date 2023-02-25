using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebUI.ViewModels
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "Kullanıcı adını lütfen giriniz.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Lütfen email alanına uygun bir adres girin.")]
        public string Email { get; set; }
        [Phone(ErrorMessage = "Telefon numaranızı girin.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Lütfen şifrenizi giriniz.")]
        public string Password { get; set; }
    }
}
