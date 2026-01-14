using Microsoft.Extensions.Primitives;
using Rudyson.Autify.Application.Contracts;

namespace Rudyson.Autify.Server.Helpers;

public sealed class TenantProvider : ITenantProvider
{
    private const string TenantIdHeaderName = "X-TenantId";
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TenantProvider(IHttpContextAccessor httpContextAccessor)
        => _httpContextAccessor = httpContextAccessor;

    public string TenantId
    {
        get
        {
            var context = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HTTP context is not available");

            if (!context.Request.Headers.TryGetValue(TenantIdHeaderName, out var tenantId)
                || StringValues.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(TenantId), "Missing tenant ID");
            }

            return tenantId.ToString();
        }
    }
}
