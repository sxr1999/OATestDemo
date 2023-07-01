namespace WebAPi.Data;

public class JwtDto
{
    public string SecurityKey { get; set; }
    public string Audience { get; set; }
    public string Issuer { get; set; }
}