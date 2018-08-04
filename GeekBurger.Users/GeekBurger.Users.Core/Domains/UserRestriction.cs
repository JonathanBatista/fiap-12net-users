using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekBurger.Users.Core.Domains
{
    [Table("UserRestrictions", Schema = "gbu")]
    public class UserRestriction : DomainEntity
    {
        public UserRestriction()
        {
            User = new User();
        }
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [StringLength(100)]
        public string Ingredient { get; set; }
    }
}