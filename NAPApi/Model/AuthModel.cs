namespace NAPApi.Model
{
    public class AuthModel
    {
        public string Message { set; get; }
        public string Username { set; get; }
        public string Email { set; get; }
        public bool IsAuthentication { set; get; }
        public string Roles { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
