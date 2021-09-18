using Lab2.Infrastructure.Commands;
using Lab2.Models.Data;
using Lab2.ViewModels.Base;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using System.Windows.Markup;

namespace Lab2.ViewModels
{
	[MarkupExtensionReturnType(typeof(MainWindowViewModel))]
	public class MainWindowViewModel : ViewModel
	{
		public MainWindowViewModel()
		{
			AddWorkCommand = new LambdaCommand(OnAddWorkCommandExecuted, CanAddWorkCommandExecute);
			RemoveSelectedWorkCommand = new LambdaCommand(OnRemoveSelectedWorkCommandExecuted, CanRemoveSelectedWorkCommandExecute);
			ClearSourceTableCommand = new LambdaCommand(OnClearSourceTableCommandExecuted, CanClearSourceTableCommandExecute);
			ReloadSourceTableCommand = new LambdaCommand(OnReloadSourceTableCommandExecuted, CanReloadSourceTableCommandExecute);
		}

		#region Properties
		private string _title = "Title";
		public string Title { get => _title; set => Set(ref _title, value); }

		private string _status = "Status";
		public string Status { get => _status; set => Set(ref _status, value); }

		private string _path = "";
		public string Path { get => _path; set => Set(ref _path, value); }

		public ObservableCollection<Work> SourceTable { get; set; } = new ObservableCollection<Work>();
		public Work SelectedWork { get; set; }

		public ObservableCollection<Work> FinalTable { get; set; } = new ObservableCollection<Work>();

		private ObservableCollection<Work> _workingTable;
		#endregion

		#region Commands

		#region AddWorkCommand
		public ICommand AddWorkCommand { get; }
		private void OnAddWorkCommandExecuted(object p)
		{
			try
			{
				SourceTable.Add(new Work(0, 0, 0));
				Status = "Добавлена новая работа с параметрами: 0, 0, 0";
			}
			catch(Exception e)
			{
				Status = e.Message;
			}			
		}
		private bool CanAddWorkCommandExecute(object p) => true;
		#endregion

		#region RemoveSelectedWorkCommand
		public ICommand RemoveSelectedWorkCommand { get; }
		private void OnRemoveSelectedWorkCommandExecuted(object p)
		{
			try
			{
				SourceTable.Remove(SelectedWork);
				SelectedWork = null;
				Status = "Выбранная работа удалена";
			}
			catch(Exception e)
			{
				Status = e.Message;
			}			
		}
		private bool CanRemoveSelectedWorkCommandExecute(object p) => SelectedWork != null;
		#endregion

		#region ClearSourceTableCommand
		public ICommand ClearSourceTableCommand { get; }
		private void OnClearSourceTableCommandExecuted(object p)
		{
			try
			{
				SourceTable.Clear();
				Status = "Начальная таблица очищена";
			}
			catch (Exception e)
			{
				Status = e.Message;
			}
		}
		private bool CanClearSourceTableCommandExecute(object p) => SourceTable.Count > 0;
		#endregion

		#region ReloadSourceTableCommand
		public ICommand ReloadSourceTableCommand { get; }
		private void OnReloadSourceTableCommandExecuted(object p)
		{
			try
			{
				string fileName = Path.Substring(Path.LastIndexOf("\\") + 1);
				if (File.Exists(fileName))
				{
					SourceTable.Clear();
					LoadSourceTable(fileName);
				}
				else
				{
					Status = $"Файл {Path} не существует";
				}
			}
			catch (Exception e)
			{
				Status = e.Message;
			}
		}
		private bool CanReloadSourceTableCommandExecute(object p) => !string.IsNullOrWhiteSpace(Path);
		#endregion

		#endregion

		private void LoadSourceTable(string fileName)  //  формат файла: число число число
		{
			StreamReader reader = new StreamReader(fileName);
			string buffer;
			string[] numbers;
			int firstEventID, secondEventID, length;
			while ((buffer = reader.ReadLine()) != null)
			{
				if (!string.IsNullOrWhiteSpace(buffer))
				{
					numbers = buffer.Split(" ", StringSplitOptions.RemoveEmptyEntries);
					firstEventID = Convert.ToInt32(numbers[0]);
					secondEventID = Convert.ToInt32(numbers[1]);
					length = Convert.ToInt32(numbers[2]);
					SourceTable.Add(new Work(firstEventID, secondEventID, length));
				}
			}
			reader.Close();
		}
	}
}
