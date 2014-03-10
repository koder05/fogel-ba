using System.ComponentModel.DataAnnotations;

namespace RF.WinApp.ViewModel
{
    public class LogonModel 
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }

        public bool IsOk { get; set; }

        public bool IsComplete { get; set; }
    }
}
