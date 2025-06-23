namespace Realestate.DTOs.Auth
{
    public class ResfreshTokenRequestDto
    {
        public string UserId { get; set; }
        public required string RefreshToken { get; set; }
    }
}
