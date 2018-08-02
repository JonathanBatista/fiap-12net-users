using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekBurger.Users.Core.Domains
{
    [Table("Users", Schema = "gbu")]
    public class User : DomainEntity
    {
        public string AzureGuid { get; set; }

        public string FaceBase64 { get; set; }

        [NotMapped]
        public byte[] Face { get; set; }

        [InverseProperty("User")]
        public List<UserRestriction> Restrictions { get; set; }
    }
}
