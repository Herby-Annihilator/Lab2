using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2.Models.Services
{
	public static class ServicesRegistrator
	{
		public static IServiceCollection AddServices(this IServiceCollection services) => services.AddSingleton<Exchanger>()
		// Register your services here
		
		;
	}
}
