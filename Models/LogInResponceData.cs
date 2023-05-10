namespace JWTAuthentication.API.Models
{
    public class LogInResponceData
    {
        public string token { get; set; }
        public DateTime expiration { get; set; }
    }
}
