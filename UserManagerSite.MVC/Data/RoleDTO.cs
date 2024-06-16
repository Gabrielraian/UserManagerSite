using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace UserManagerSite.MVC.Data;

public class RoleDTO
{
    public int id { get; set; }
    public string role { get; set; }
}

