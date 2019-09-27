namespace todoapi.AppOptions
{
    public sealed class JWTOptions
    {
        public string Issuer {get; set;}

        public string Audience {get; set;}

        public int ExpiresInMins {get; set;}
    }
}