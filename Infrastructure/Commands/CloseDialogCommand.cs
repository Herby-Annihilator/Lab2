using Lab2.Infrastructure.Commands.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Lab2.Infrastructure.Commands
{
	public class CloseDialogCommand : Command
	{
		public bool? DialogResult { get; set; }
		protected override void Execute(object parameter)
		{
			if (!CanExecute(parameter)) return;

			var window = (Window)parameter;
			window.DialogResult = DialogResult;
			window.Close();
		}

		protected override bool CanExecute(object parameter)
		{
			return parameter is Window;
		}
	}
}
