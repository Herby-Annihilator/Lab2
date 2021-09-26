using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2.Models.Services
{
	public class ServiceLocator
	{
		public Exchanger Exchanger => App.Services.GetRequiredService<Exchanger>();
	}
}
