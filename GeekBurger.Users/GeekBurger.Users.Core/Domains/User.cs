using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekBurger.Users.Core.Domains
{
    [Table("Users", Schema = "gbu")]
    public class User : DomainEntity
    {
        public User(string face)
        {
            FaceBase64 = face;
            AzureGuid = Guid.NewGuid().ToString();
            Face = new byte[5000];
            Restrictions = new List<UserRestriction>();
        }
        public string AzureGuid { get; set; }

        public string FaceBase64 { get; set; }

        [NotMapped]
        public byte[] Face { get; set; }

        [InverseProperty("User")]
        public List<UserRestriction> Restrictions { get; set; }
    }
}
