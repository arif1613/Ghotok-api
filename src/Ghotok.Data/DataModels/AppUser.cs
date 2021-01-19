using System.ComponentModel.DataAnnotations;

namespace Ghotok.Data.DataModels
{
    public class AppUser : BaseDbModel
    {
        public string UserRole { get; set; }
        [Required]
        public string Password { get; set; }
        public bool IsVarified { get; set; }
        public bool IsLoggedin { get; set; }
        public int LoggedInDevices { get; set; }
        public virtual User User { get; set; }

    }
}
