namespace GeekBurger.Users.Contract.Dtos.RequestDto
{
    public class UserFoodRestrictionRequestDto
    {
        public string Restrictions { get; set; }

        public string Other { get; set; }

        public int UserId { get; set; }

        public int RequesterId { get; set; }
    }
}
