using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFTest.DbContext
{
    public class User : IUser
    {
        public User()
        {
            Actions = new HashSet<Action>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        [StringLength(16)]
        [Index(IsUnique = true)]
        public string Login { get; set; }
        public UsersRoles UsersRole { get; set; }
        public virtual Comment Comment { get; set; }
        public virtual ICollection<Action> Actions { get; set; }

    }
}