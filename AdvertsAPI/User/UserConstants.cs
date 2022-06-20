namespace AdvertsAPI.User
{
    public class UserConstants
    {
        public static List<UserModel> Users = new List<UserModel>()
        {
            // Full read/write access
            new UserModel()
            {
                UserName = "richard_admin",
                EmailAddress = "richard_admin@email.se",
                Password = "passwordAdmin",
                GivenName = "Richard",
                SurName = "chalk",
                Role = "Admin",
            },
            // Can only Read
            new UserModel()
            {
                UserName = "richard_user",
                EmailAddress = "richard_user@email.se",
                Password = "passwordUser",
                GivenName = "Richard",
                SurName = "Chalk",
                Role = "User",
            }
        };
    }
}
