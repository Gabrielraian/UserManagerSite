using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace UserManagerSite.MVC.Data;

public class UserDTO
{
    public int? id { get; set; }
    public string name { get; set; }
    public DateTime birthdate { get; set; }
    public string email { get; set; }
    public int roleId { get; set; }
    public string roleName { get; set; }
    public RoleDTO role { get; set; }

    public UserDTO()
    {
        // Inicialize as propriedades aqui se necessário
        role = new RoleDTO();
    }
}
