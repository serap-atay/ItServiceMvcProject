using System.ComponentModel.DataAnnotations;

namespace ITServiceApp.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Kullanıcı Adi")]
        [Required(ErrorMessage = "Kullnici adı alanı gereklidir.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Ad alanı gereklidir.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Şifreniz minimum 6 karakter olmalıdır.")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name ="Beni Hatırla")]
        public bool RememberMe { get; set; }
    }
}
