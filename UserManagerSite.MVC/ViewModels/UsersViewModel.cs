using UserManagerSite.MVC.Data;

namespace UserManagerSite.MVC.ViewModels
{
    public class UsersViewModel
    {
        public IEnumerable<UserDTO> Users { get; set; }
        public IEnumerable<RoleDTO> Roles { get; set; }
    }

}
