namespace CitiesManager.Core.DTO
{
    public class JwtTokenModel
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
    }
}
