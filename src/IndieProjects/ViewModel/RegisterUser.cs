using System.ComponentModel.DataAnnotations;

namespace IndieProjects.ViewModel
{
    public class RegisterUser
    {
        [Required]
        [Display(Name = "Имя пользователя")]
        public string NickName { get; set; }
        [Required]
        [Display(Name = "Почта")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}
