namespace Inventory_M.Models.users_management;

public class role
{
    public int RoleId { get; set; }
    public string rolename { get; set; }
    public virtual ICollection<UserToRole> UsersToRoles { get; set; }

    
}
