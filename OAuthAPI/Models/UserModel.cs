namespace OAuthAPI.Models
{
    public class UserModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public string[] regions { get; set; }
    }
}