using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekBurger.Users.Core.Domains
{
    public abstract class DomainEntity
    {
        [Key]
        [Column(Order = 0)]
        public int Id { get; set; }
    }
}
