namespace GeekBurger.Users.Contract
{
    public class UserRetrieved
    {
        public bool AreRestrictionsSet { get; set; }

        public string UserId { get; set; }
    }
}
