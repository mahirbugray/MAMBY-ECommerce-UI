namespace MAMBY.UI.Models
{
    public class EditRoleViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string[]? UserIdsToAdd { get; set; }
        public string[]? UserIdsToDelete { get; set; }
    }
}
