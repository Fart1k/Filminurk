using System.ComponentModel.DataAnnotations;

namespace Filminurk.Models.Accounts
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Sisesta parool uuesti: ")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Paroolid ei kattu, kontrolli et oled samamoodi sisestanud")]
        public string ConfirmPassword { get; set; }

        public string Token { get; set; }

    }
}
