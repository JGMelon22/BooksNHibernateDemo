namespace NHibernateDemo.Infrastructure.Configuration;

public class BasicAuthOptions
{
    public const string BasicAuth = "BasicAuth";

    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}