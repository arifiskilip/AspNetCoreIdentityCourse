using System.ComponentModel.DataAnnotations;

namespace WebUI.ViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Lütfen email adresinizi giriniz.")]
        [Display(Name = "Email adresi")]
        [EmailAddress(ErrorMessage = "Lütfen geçerli bir email adresi giriniz.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Lütfen şifre alanını doldurun.")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
