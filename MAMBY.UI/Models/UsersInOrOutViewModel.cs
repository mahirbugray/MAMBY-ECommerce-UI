namespace MAMBY.UI.Models
{
    public class UsersInOrOutViewModel
    {
        public RoleViewModel Role { get; set; }
        public List<UserViewModel> UsersInRole { get; set; }
        public List<UserViewModel> UsersOutRole { get; set; }
        public EditRoleViewModel EditRole { get; set; }
    }
}
