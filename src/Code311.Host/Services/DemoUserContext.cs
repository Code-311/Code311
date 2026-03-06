namespace Code311.Host.Services;

public interface IDemoUserContext
{
    string TenantId { get; }
    string UserId { get; }
}

public sealed class DemoUserContext : IDemoUserContext
{
    public string TenantId => "demo-tenant";
    public string UserId => "demo-user";
}
