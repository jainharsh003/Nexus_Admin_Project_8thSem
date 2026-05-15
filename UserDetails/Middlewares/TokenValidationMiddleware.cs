using UserDetails.Clients.Interface;
using UserDetails.DTOs;

namespace UserDetails.Middlewares
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAuthClient _authClient;

        public TokenValidationMiddleware(RequestDelegate next, IAuthClient authClient)
        {
            _next = next;
            _authClient = authClient;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Skip Swagger endpoints
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                await _next(context);
                return;
            }

            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Token missing or invalid format");
                return;
            }

            // Extract token only
            var token = authHeader.Substring("Bearer ".Length).Trim();

            var authUser = await _authClient.ValidateTokenAsync(token);

            if (authUser == null || authUser.UserId == Guid.Empty)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid token");
                return;
            }

            // Store validated user
            context.Items["AuthUser"] = authUser;

            await _next(context);
        }
    }
}