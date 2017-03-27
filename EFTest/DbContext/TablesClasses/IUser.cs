using System.Collections.Generic;

namespace EFTest.DbContext
{
    public interface IUser
    {
        ICollection<Action> Actions { get; set; }
        Comment Comment { get; set; }
        int Id { get; set; }
        string Login { get; set; }
        string Name { get; set; }
        UsersRoles UsersRole { get; set; }
    }
}