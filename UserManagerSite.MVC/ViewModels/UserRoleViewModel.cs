using System.Collections.Generic;
using UserManagerSite.MVC.Data;

namespace UserManagerSite.MVC.Models
{
    public class UserRoleViewModel
    {
        public UserDTO User { get; set; }
        public List<RoleDTO> Roles { get; set; }
    }
}
