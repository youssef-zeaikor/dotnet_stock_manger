namespace Inventory_M.Models.users_management;

public class user
{
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Lname { get; set; } 
    public string Email { get; set; }
    public string PassWord { get; set; }
    public bool status { get; set; }
    
    public virtual ICollection<UserToRole> UsersToRoles { get; set; }



}
