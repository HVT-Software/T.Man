namespace T.Domain.Models;

public class OAuthProvider {
    public required string ClientId     { get; set; }
    public required string ClientSecret { get; set; }
}


public class JwtProvider {
    public required string Issuer   { get; set; }
    public required string Audience { get; set; }

    public required string Key { get; set; }
}


public class Providers {
    public required OAuthProvider Google  { get; set; }
    public required OAuthProvider Github  { get; set; }
    public required OAuthProvider Discord { get; set; }
    public required JwtProvider   Jwt     { get; set; }
}
