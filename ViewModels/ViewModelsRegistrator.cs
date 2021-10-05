using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2.ViewModels
{
	public static class ViewModelsRegistrator
	{
		public static IServiceCollection AddViewModels(this IServiceCollection services) => 
			services
			.AddSingleton<MainWindowViewModel>()
			.AddTransient<EditingWindowViewModel>()
			.AddTransient<DeleteWorksInCycleViewModel>()
		;
	}
}
