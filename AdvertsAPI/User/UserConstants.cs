namespace AdvertsAPI.User
{
    public class UserConstants
    {
        public static List<UserModel> Users = new List<UserModel>()
        {
            // Full read/write access
            new UserModel()
            {
                UserName = "stefan_admin",
                EmailAddress = "stefan_admin@email.se",
                Password = "passwordAdmin",
                GivenName = "Stefan",
                SurName = "Holmberg",
                Role = "Admin",
            },
            // Can only Read
            new UserModel()
            {
                UserName = "stefan_user",
                EmailAddress = "stefan_user@email.se",
                Password = "passwordUser",
                GivenName = "Stefan",
                SurName = "Holmberg",
                Role = "User",
            }
        };
    }
}
