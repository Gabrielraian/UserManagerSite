using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace UserManagerSite.Application.Data;

public class UserDTO
{
    public int? id { get; set; }
    
    [StringLength(100, MinimumLength = 3)]
    [Required]
    public string? name { get; set; }
    
    [Display(Name = "Data de Nascimento")]
    [DataType(DataType.Date)]
    public DateTime birthdate { get; set; }
    
    public string? email { get; set; }
    
    public int roleId { get; set; }  // ID da Role
    
    public string roleName { get; set; }  // Nome da Role
}

