using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EFTest.DbContext
{
    public class Box
    {

        public Box()
        {
            BoxesToProcess = new HashSet<Box>();
        }

        [Key]
        public int Id { get; set; }
        public int Number { get; set; }
        public string Address { get; set; }
        public string IpAddress { get; set; }
        public virtual Comment Comment { get; set; }
        public int CashLeft { get; set; }
        
        public virtual ICollection<Box> BoxesToProcess { get; set; }

    }
}
