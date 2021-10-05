using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2.ViewModels
{
	public class ViewModelLocator
	{
		public MainWindowViewModel MainWindowModel => 
			App.Services.GetRequiredService<MainWindowViewModel>();
		
		public EditingWindowViewModel EditingWindowModel => 
			App.Services.GetRequiredService<EditingWindowViewModel>();

		public DeleteWorksInCycleViewModel DeleteWorksInCycleViewModel =>
			App.Services.GetRequiredService<DeleteWorksInCycleViewModel>();
	}
}
