using System.ComponentModel.DataAnnotations;
using WebUI.Enums;

namespace WebUI.ViewModels
{
    public class UserViewModel
    {
        [Required(ErrorMessage = "Lütfen adınızı giriniz.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Lütfen soyadınızı giriniz.")]
        public string LastName { get; set; }
        public string Picture { get; set; }
        public Gender Gender { get; set; }
        [Required(ErrorMessage ="Kullanıcı Adı Zorunludur.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Şehir Alanı Zorunludur.")]
        public string City { get; set; }
        [Required(ErrorMessage = "Email Alanı Zorunludur.")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Telefon Numarası Zorunludur.")]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
