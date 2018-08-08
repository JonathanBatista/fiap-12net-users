using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekBurger.Users.Core.Domains
{
    [Table("Users", Schema = "gbu")]
    public class User : DomainEntity
    {
        public User()
        {

        }
        public User(string face)
        {
            FaceBase64 = face;
            UserId = Guid.NewGuid();
            Restrictions = new List<UserRestriction>();
        }
        public Guid UserId { get; set; }

        public Guid PersistedId { get; set; }

        public string FaceBase64 { get; set; }

        public string GuidReference { get; set; }

        public bool InProcessing { get; set; }

        [InverseProperty("User")]
        public List<UserRestriction> Restrictions { get; set; }
    }
}
