using Lab2.Infrastructure.Commands;
using Lab2.Models.Data;
using Lab2.Models.Services;
using Lab2.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using System.Windows.Markup;

namespace Lab2.ViewModels
{
	[MarkupExtensionReturnType(typeof(DeleteWorksInCycleViewModel))]
	public class DeleteWorksInCycleViewModel : ViewModel
	{
		private Exchanger _exchanger;
		public DeleteWorksInCycleViewModel()
		{
			_exchanger = App.Services.GetRequiredService<Exchanger>();
			DeleteSelectedWorkCommand = new LambdaCommand(OnDeleteSelectedWorkCommandExecuted,
				CanDeleteSelectedWorkCommandExecute);
			AcceptChangesCommand = new LambdaCommand(OnAcceptChangesCommandExecuted,
				CanAcceptChangesCommandExecute);
			CancelCommand = new LambdaCommand(OnCancelCommandExecuted,
				CanCancelCommandExecute);
			PrepareList();
		}

		public ObservableCollection<Work> WorksInCycles { get; private set; } =
			new ObservableCollection<Work>();
		private Work _selectedWork;
		public Work SelectedWork { get => _selectedWork; set => Set(ref _selectedWork, value); }

		private List<Work> toRemove = new List<Work>();

		#region DeleteSelectedWorkCommand
		public ICommand DeleteSelectedWorkCommand { get; }
		private void OnDeleteSelectedWorkCommandExecuted(object p)
		{
			toRemove.Add(SelectedWork);
			WorksInCycles.Remove(SelectedWork);
			SelectedWork = null;
		}
		private bool CanDeleteSelectedWorkCommandExecute(object p) => SelectedWork != null;
		#endregion

		#region AcceptChangesCommand
		public ICommand AcceptChangesCommand { get; }
		private void OnAcceptChangesCommandExecuted(object p)
		{
			foreach (var item in toRemove)
			{
				_exchanger.CurrentTable.Remove(item);
				_exchanger.Log.Add($"Робота {item} удалена");
			}
			_exchanger.Log.Add("Изменения применены");
			App.ActivedWindow.DialogResult = true;
			App.ActivedWindow.Close();
		}
		private bool CanAcceptChangesCommandExecute(object p) => toRemove.Count > 0;
		#endregion

		#region CancelCommand
		public ICommand CancelCommand { get; }
		private void OnCancelCommandExecuted(object p)
		{
			App.ActivedWindow.DialogResult = false;
			_exchanger.Log.Add("Работы не были удалены");
			App.ActivedWindow.Close();
		}
		private bool CanCancelCommandExecute(object p) => true;
		#endregion

		private void PrepareList()
		{
			foreach (var item in _exchanger.WorksInCycles)
			{
				WorksInCycles.Add(item);
			}
		}
	}
}
