using System.ComponentModel.DataAnnotations;

namespace Filminurk.Models.Accounts
{
    public class ChangePasswordViewModes
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Sisesta oma praegune parool")]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Sisesta oma uus parool")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Kirjtuta parool uuesti")]
        [Compare("NewPassword", ErrorMessage = "Paroolid ei kattu, palun proovi uuesti")]
        public string ConfirmNewPassword { get; set; }
    }
}
