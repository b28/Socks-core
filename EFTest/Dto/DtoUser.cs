using EFTest.DbContext;

namespace EFTest.Dto
{
    public interface IDtoUser
    {
        string Name { get; set; }
        string Login { get; set; }
        UsersRoles Role { get; set; }

    }
    
    public class DtoUser
    {
        
    }
}