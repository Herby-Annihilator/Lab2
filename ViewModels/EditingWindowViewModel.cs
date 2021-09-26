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
	[MarkupExtensionReturnType(typeof(EditingWindowViewModel))]
	public class EditingWindowViewModel : ViewModel
	{
		private Exchanger _exchanger;

		public EditingMode EditingMode { get; set; } = EditingMode.StartVertexMode;

		public EditingWindowViewModel()
		{
			_exchanger = App.Services.GetRequiredService<Exchanger>();
			
			ChoiceOfActionCommand = new LambdaCommand(OnChoiceOfActionCommandExecuted,
				CanChoiceOfActionCommandExecute);
			AddFakeVertexCommand = new LambdaCommand(OnAddFakeVertexCommandExecuted,
				CanAddFakeVertexCommandExecute);
			DeleteVerticesCommand = new LambdaCommand(OnDeleteVerticesCommandExecuted,
				CanDeleteVerticesCommandExecute);
		}

		#region Properties

		private string _title = "Редактироване вершин";
		public string Title { get => _title; set => Set(ref _title, value); }

		private string _status = "Редактироване вершин";
		public string Status { get => _status; set => Set(ref _status, value); }

		private string _meaningLine = "Редактироване вершин";
		public string MeaningLine { get => _meaningLine; set => Set(ref _meaningLine, value); }

		private string _fakeVertexIndex;
		public string FakeVertexIndex { get => _fakeVertexIndex; 
			set => Set(ref _fakeVertexIndex, value); }

		private string _indexesToConnectWithFakeVertex;
		public string IndexesToConnectWithFakeVertex { get => _indexesToConnectWithFakeVertex;
			set => Set(ref _indexesToConnectWithFakeVertex, value); }

		private bool _addingFakeVertexIsNecessary = true;
		public bool AddingFakeVertexIsNecessary { get => _addingFakeVertexIsNecessary;
			set => Set(ref _addingFakeVertexIsNecessary, value);
		}

		private bool _deletingVerticesIsNecessary = false;
		public bool DeletingVerticesIsNecessary
		{
			get => _deletingVerticesIsNecessary;
			set => Set(ref _deletingVerticesIsNecessary, value);
		}

		private bool _deletingEdgesIsNecessary = false;
		public bool DeletingEdgesIsNecessary
		{
			get => _deletingEdgesIsNecessary;
			set => Set(ref _deletingEdgesIsNecessary, value);
		}

		public ObservableCollection<int> VerticesCanBeDeleted { get; set; } = 
			new ObservableCollection<int>();

		private int _selectedVertexToDelete;
		public int SelectedVertexToDelete
		{
			get => _selectedVertexToDelete;
			set
			{
				Set(ref _selectedVertexToDelete, value);
				AdjacencyEdgesWillBeDeleted.Clear();
				
				List<Work> works;
				if (!_exchanger.AdjacencyList.Edges
					.TryGetValue(_selectedVertexToDelete, out works))
					return;
				foreach (Work work in works)
				{
					AdjacencyEdgesWillBeDeleted.Add(work);
				}
			}
		}

		public ObservableCollection<Work> AdjacencyEdgesWillBeDeleted { get; set; } =
			new ObservableCollection<Work>();

		#endregion

		#region Commands

		#region ChoiceOfActionCommand
		public ICommand ChoiceOfActionCommand { get; }
		private void OnChoiceOfActionCommandExecuted(object p)
		{
			AddingFakeVertexIsNecessary = false;
			DeletingEdgesIsNecessary = false;
			DeletingVerticesIsNecessary = false;
			if ((string)p == "add fake")
				AddingFakeVertexIsNecessary = true;
			else if ((string)p == "delete edges")
				DeletingEdgesIsNecessary = true;
			else
				DeletingVerticesIsNecessary = true;
		}
		private bool CanChoiceOfActionCommandExecute(object p) => true;
		#endregion

		#region AddFakeVertexCommand
		public ICommand AddFakeVertexCommand { get; }
		private void OnAddFakeVertexCommandExecuted(object p)
		{
			try
			{
				int fakeIndex = Convert.ToInt32(FakeVertexIndex);
				foreach (int vertex in _exchanger.Vertices)
				{
					if (vertex == fakeIndex)
						throw new Exception($"Вершина {vertex} уже существует");
				}
				string[] buffer = IndexesToConnectWithFakeVertex.Split(' ',
					StringSplitOptions.RemoveEmptyEntries);
				int[] indexesToConnect = new int[buffer.Length];
				for (int i = 0; i < buffer.Length; i++)
				{
					indexesToConnect[i] = Convert.ToInt32(buffer[i]);
				}
				ValidateIndexesToConnect(indexesToConnect, fakeIndex);
				Connect(fakeIndex, indexesToConnect);
				Status = "Вершина создана. Для того, чтобы изменения вступили в силу, нажмите 'ОК'";
			}
			catch(Exception e)
			{
				Status = e.Message;
			}			
		}
		private bool CanAddFakeVertexCommandExecute(object p) => true;
		#endregion

		#region DeleteVerticesCommand
		public ICommand DeleteVerticesCommand { get; }
		private void OnDeleteVerticesCommandExecuted(object p)
		{
			
		}
		private bool CanDeleteVerticesCommandExecute(object p) => true; 
		#endregion

		#endregion

		private void ValidateIndexesToConnect(int[] indexes, int fakeVertexIndex)
		{
			Array.Sort(indexes);
			if (indexes[0] == fakeVertexIndex)
				throw new Exception($"Попытка соединить врешину {fakeVertexIndex} и" +
					$" {indexes[0]} недопустима");
			if (!_exchanger.Vertices.Contains(indexes[0]))
				throw new Exception($"Вершина {indexes[0]} не существует " +
					$"в исходной таблице");
			for (int i = 0; i < indexes.Length - 1; i++)
			{
				if (indexes[i] == indexes[i + 1])
					throw new Exception($"Повторение вершин в списке для соединения: " +
						$"{indexes[i]} и {indexes[i + 1]}");
				else if (indexes[i + 1] == fakeVertexIndex)
					throw new Exception($"Попытка соединить врешину {fakeVertexIndex} и" +
						$" {indexes[i + 1]} недопустима");
				else if (!_exchanger.Vertices.Contains(indexes[i + 1]))
					throw new Exception($"Вершина {indexes[i + 1]} не существует " +
						$"в исходной таблице");
			}
		}

		private void Connect(int fakeVertexIndex, int[] indexes)
		{
			if (EditingMode == EditingMode.StartVertexMode)
			{

			}
			else
			{

			}
		}

		private void PrepareVerticesDeleting()
		{
			VerticesCanBeDeleted.Clear();
			AdjacencyEdgesWillBeDeleted.Clear();
			foreach (var vertex in _exchanger.AdjacencyList.Vertices)
			{
				VerticesCanBeDeleted.Add(vertex);
			}
			SelectedVertexToDelete = VerticesCanBeDeleted.Count > 0 ? VerticesCanBeDeleted[0] :
				0;
		}
	}


	public enum EditingMode
	{
		StartVertexMode,
		EndVertexMode
	}
}
