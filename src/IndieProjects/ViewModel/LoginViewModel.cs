using System;
using System.ComponentModel.DataAnnotations;

namespace IndieProjects.ViewModel
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Display(Name = "Запомнить")]
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
    }
}
