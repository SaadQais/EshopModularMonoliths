namespace EShopWeb.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IEndpointRouteBuilder MapLoginAndLogout(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/Login", (string? returnUrl) =>
            {
                var properties = new AuthenticationProperties
                {
                    RedirectUri = returnUrl ?? "/"
                };

                return TypedResults.Challenge(properties, [OpenIdConnectDefaults.AuthenticationScheme]);
            });

            endpoints.MapPost("/Logout", ([FromForm] string? returnUrl) =>
            {
                var properties = new AuthenticationProperties
                {
                    RedirectUri = returnUrl ?? "/"
                };

                return TypedResults.SignOut(properties,
                    [CookieAuthenticationDefaults.AuthenticationScheme, OpenIdConnectDefaults.AuthenticationScheme]);
            });

            return endpoints;
        }
    }
}
