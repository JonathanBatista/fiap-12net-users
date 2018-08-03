namespace GeekBurger.Users.Contract
{
    public class UserFoodRestriction
    {
        public string Restrictions { get; set; }

        public string Other { get; set; }

        public int UserId { get; set; }

        public int RequesterId { get; set; }
    }
}
