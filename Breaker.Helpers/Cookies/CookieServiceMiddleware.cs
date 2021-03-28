using Breaker.Helpers.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Breaker.Helpers.Cookies
{
	// maybe switch to IMiddleware: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/extensibility
	internal class CookieServiceMiddleware
	{
		private readonly RequestDelegate _next;

		public CookieServiceMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task Invoke(HttpContext context, ICookieService cookieService)
		{
			// write cookies to response right before it starts writing out from MVC/api responses...
			context.Response.OnStarting(() =>
			{
				// cookie service should not write out cookies on 500, possibly others as well
				if (!context.Response.StatusCode.IsInRange(500, 599))
				{
					cookieService.WriteToResponse(context);
				}
				return Task.CompletedTask;
			});

			await _next(context);
		}
	}

	public static class CookieServiceMiddlewareExtensions
	{
		public static IApplicationBuilder UseCookieService(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<CookieServiceMiddleware>();
		}
	}
}
