using System.Collections.Generic;

namespace GeekBurger.Users.Contract
{
    public class UserFoodRestriction
    {
        public List<string> Restrictions { get; set; }

        public string Other { get; set; }

        public string UserId { get; set; }

        public int RequesterId { get; set; }
    }
}
