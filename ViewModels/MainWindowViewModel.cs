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
			BrowseCommand = new LambdaCommand(OnBrowseCommandExecuted, CanBrowseCommandExecute);
			ClearFinalTableCommand = new LambdaCommand(OnClearFinalTableCommandExecuted, CanClearFinalTableCommandExecute);
			ClearListBoxCommand = new LambdaCommand(OnClearListBoxCommandExecuted, CanClearListBoxCommandExecute);
			FullPathsInTheGraph.Add("4, 5, 9, 8, 10");
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

		public ObservableCollection<string> FullPathsInTheGraph { get; set; } = new ObservableCollection<string>();

		public ObservableCollection<string> Log { get; set; } = new ObservableCollection<string>();

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
				Log.Add(Status);
			}
			catch(Exception e)
			{
				Status = e.Message;
				Log.Add(Status);
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
				Log.Add(Status);
			}
			catch(Exception e)
			{
				Status = e.Message;
				Log.Add(Status);
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
				Log.Add(Status);
			}
			catch (Exception e)
			{
				Status = e.Message;
				Log.Add(Status);
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
				if (File.Exists(Path))
				{
					SourceTable.Clear();
					LoadSourceTable(Path);
				}
				else
				{
					Status = $"Файл {Path} не существует";
					Log.Add(Status);
				}
			}
			catch (Exception e)
			{
				Status = e.Message;
				Log.Add(Status);
			}
		}
		private bool CanReloadSourceTableCommandExecute(object p) => !string.IsNullOrWhiteSpace(Path);
		#endregion

		#region BrowseCommand
		public ICommand BrowseCommand { get; }
		private void OnBrowseCommandExecuted(object p)
		{
			try
			{
				Status = "Открытие файла";
				Log.Add(Status);
				OpenFileDialog dialog = new OpenFileDialog();
				dialog.InitialDirectory = Environment.CurrentDirectory;
				dialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
				if (dialog.ShowDialog() == true)
				{
					Path = dialog.FileName;
				}
				Status = "Открытие файла завершено";
				Log.Add(Status);
			}
			catch (Exception e)
			{
				Status = e.Message;
				Log.Add(Status);
			}
		}
		private bool CanBrowseCommandExecute(object p) => true;
		#endregion

		#region ClearFinalTableCommand
		public ICommand ClearFinalTableCommand { get; }
		private void OnClearFinalTableCommandExecuted(object p)
		{
			try
			{
				FinalTable.Clear();
				Status = "Итоговая таблица очищена";
				Log.Add(Status);
			}
			catch (Exception e)
			{
				Status = e.Message;
				Log.Add(Status);
			}
		}
		private bool CanClearFinalTableCommandExecute(object p) => FinalTable.Count > 0;
		#endregion

		#region ClearListBoxCommand
		public ICommand ClearListBoxCommand{ get; }
		private void OnClearListBoxCommandExecuted(object p)
		{
			try
			{
				FullPathsInTheGraph.Clear();
				Status = "Список путей очищен";
				Log.Add(Status);
			}
			catch (Exception e)
			{
				Status = e.Message;
				Log.Add(Status);
			}
		}
		private bool CanClearListBoxCommandExecute(object p) => FullPathsInTheGraph.Count > 0;
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
