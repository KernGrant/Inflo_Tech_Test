using System.ComponentModel.DataAnnotations;

namespace UserManagement.Web.Models.Users
{
    public class UserEditViewModel : UserCreateViewModel
    {
        [Required]
        public new int Id { get; set; }
    }
}
