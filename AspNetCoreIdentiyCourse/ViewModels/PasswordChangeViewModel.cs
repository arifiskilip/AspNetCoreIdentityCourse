using System.ComponentModel.DataAnnotations;

namespace WebUI.ViewModels
{
    public class PasswordChangeViewModel
    {
        [Display(Name ="Eski Şifreniz")]
        [Required(ErrorMessage ="Eski şifreniz gereklidir.")]
        [DataType(DataType.Password)]
        public string PasswordOld { get; set; }

        [Display(Name = "Yeni Şifreniz")]
        [Required(ErrorMessage = "Yeni şifreniz gereklidir.")]
        [DataType(DataType.Password)]
        public string PasswordNew { get; set; }

        [Display(Name = "Tekrar Şifreniz")]
        [Required(ErrorMessage = "Tekrar şifreniz gereklidir.")]
        [DataType(DataType.Password)]
        [Compare("PasswordNew",ErrorMessage ="Yeni şifreler birbiri ile aynı olmak zorundadır.")]
        public string PasswordConfirm { get; set; }
    }
}
