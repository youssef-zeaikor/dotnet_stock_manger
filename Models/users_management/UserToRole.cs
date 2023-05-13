namespace Inventory_M.Models.users_management;

public class UserToRole
{
    public int UserToRoleId { get; set; }
    public int UserId { get; set; }
    public int RoleId { get; set; }

    public virtual role Role { get; set; }
    public virtual user User { get; set; }
}
