using Lab2.Infrastructure.Commands;
using Lab2.Models.Data;
using Lab2.Models.Services;
using Lab2.ViewModels.Base;
using Lab2.Views.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace Lab2.ViewModels
{
	[MarkupExtensionReturnType(typeof(MainWindowViewModel))]
	public class MainWindowViewModel : ViewModel
	{
		private Exchanger _exchanger = App.Services.GetRequiredService<Exchanger>();
		public MainWindowViewModel()
		{
			AddWorkCommand = new LambdaCommand(OnAddWorkCommandExecuted, CanAddWorkCommandExecute);
			RemoveSelectedWorkCommand = new LambdaCommand(OnRemoveSelectedWorkCommandExecuted, CanRemoveSelectedWorkCommandExecute);
			ClearSourceTableCommand = new LambdaCommand(OnClearSourceTableCommandExecuted, CanClearSourceTableCommandExecute);
			ReloadSourceTableCommand = new LambdaCommand(OnReloadSourceTableCommandExecuted, CanReloadSourceTableCommandExecute);
			BrowseCommand = new LambdaCommand(OnBrowseCommandExecuted, CanBrowseCommandExecute);
			ClearFinalTableCommand = new LambdaCommand(OnClearFinalTableCommandExecuted, CanClearFinalTableCommandExecute);
			ClearListBoxCommand = new LambdaCommand(OnClearListBoxCommandExecuted, CanClearListBoxCommandExecute);
			StreamlineCommand = new LambdaCommand(OnStreamlineCommandExecuted, CanStreamlineCommandExecute);
			FullPathsInTheGraph.Add("4, 5, 9, 8, 10");
			ShowWindowCommand = new LambdaCommand(OnShowWindowCommandExecuted,
				CanShowWindowCommandExecute);
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

		private List<Work> _workingTable;
		#endregion

		#region Commands

		#region ShowWindowCommand
		public ICommand ShowWindowCommand { get; }
		private void OnShowWindowCommandExecuted(object p)
		{
			try
			{
				Window window = new EditingStartVertexWindow();
				((EditingWindowViewModel)(window.DataContext)).EditingMode = 
					EditingMode.EndVertexMode;
				((EditingWindowViewModel)(window.DataContext)).MeaningLine = 
					"Найдено несколько конечных вершин";
				_exchanger.CurrentTable = new List<Work>();
				window.ShowDialog();
				Log.Add("Окно открыто");
			}
			catch (Exception e)
			{
				Status = e.Message;
				Log.Add(Status);
			}
		}
		private bool CanShowWindowCommandExecute(object p) => true; 
		#endregion

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

		#region StreamlineCommand
		public ICommand StreamlineCommand { get; }
		private void OnStreamlineCommandExecuted(object p)
		{
			try
			{
				int startVertexIndex;
				int endVertexIndex;
				bool isOk = false;
				_workingTable = new List<Work>();
				CopySourceTableToWorkingTable(SourceTable, _workingTable);
				while (!isOk)
				{
					isOk = false;
					_workingTable.Sort(SortAscending);
					RemoveLoops(_workingTable);
					RemoveRepeatedWorksFromTable(_workingTable);
					try
					{
						startVertexIndex = FindStartVertex(_workingTable);
						endVertexIndex = FindEndVertex(_workingTable);
						isOk = true;
						FindCycles(_workingTable);
						_workingTable = Streamline(_workingTable, startVertexIndex);
						FindAllPaths(_workingTable, 0, startVertexIndex, endVertexIndex, new List<int>());
						CopySourceTableToWorkingTable(_workingTable, FinalTable);
					}
					catch (SeveralVerticesFoundException e)
					{
						_exchanger.CurrentTable = _workingTable;
						_exchanger.Vertices = e.Vertcies;
						Window window = new EditingStartVertexWindow();
						((EditingWindowViewModel)window.DataContext).MeaningLine = e.Message;
						((EditingWindowViewModel)window.DataContext).EditingMode = e.EditingMode;
						Log.Add("Открыто окно для редактирования");
						window.ShowDialog();
						_workingTable = _exchanger.CurrentTable;
						Log.Add("Изменения применены");
					}
					catch (NoVerticesFoundException e)
					{
						_exchanger.CurrentTable = _workingTable;
						_exchanger.Vertices = e.Vertcies;
						Window window = new EditingStartVertexWindow();
						((EditingWindowViewModel)window.DataContext).MeaningLine = e.Message;
						((EditingWindowViewModel)window.DataContext).EditingMode = e.EditingMode;
						Log.Add("Открыто окно для редактирования");
						window.ShowDialog();
						_workingTable = _exchanger.CurrentTable;
						Log.Add("Изменения применены");
					}
				}
			}
			catch(CyclesFoundException e)
			{
				MessageBox.Show(e.ToString(), "Найден цикл", MessageBoxButton.OK, MessageBoxImage.Warning);
				Log.Add(e.ToString());
			}
			catch(Exception e)
			{
				Status = e.Message;
				Log.Add(Status);
			}
		}
		private bool CanStreamlineCommandExecute(object p) => SourceTable.Count > 0;
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

		private void CopySourceTableToWorkingTable(ICollection<Work> source, ICollection<Work> working)
		{
			foreach (Work work in source)
			{
				working.Add(work.Clone());
			}
		}

		private int SortAscending(Work first, Work second)
		{
			if (first.FirstEventID > second.FirstEventID) return 1;
			if (first.FirstEventID < second.FirstEventID) return -1;
			if (first.FirstEventID == second.FirstEventID)
			{
				if (first.SecondEventID > second.SecondEventID) return 1;
				if (first.SecondEventID < second.SecondEventID) return -1;
			}
			return 0;
		}

		private int SortDescending(Work first, Work second)
		{
			if (first.FirstEventID > second.FirstEventID) return -1;
			if (first.FirstEventID < second.FirstEventID) return 1;
			return 0;
		}

		private void RemoveRepeatedWorksFromTable(List<Work> works)
		{
			List<Work> toRemove = new List<Work>();
			Work prevWork = null;
			bool thereIsNoRepeatedWorks = true;
			do
			{
				if (!thereIsNoRepeatedWorks)
				{
					works.Remove(_exchanger.SelectedWorkToRemove);
					Log.Add($"Работа {_exchanger.SelectedWorkToRemove} удалена");
				}
					
				thereIsNoRepeatedWorks = true;
				foreach (Work work in works)
				{
					if (prevWork == null)
					{
						prevWork = work;
						continue;
					}
					if (prevWork.FirstEventID == work.FirstEventID)
					{
						if (prevWork.SecondEventID == work.SecondEventID)
						{
							if (prevWork.Length == work.Length)  // полное совпадение
							{
								toRemove.Add(work);
							}
							else // частичное совпадение - нужно вмешательство пользователя
							{
								_exchanger.FirstWorkToRemove = prevWork;
								_exchanger.SecondWorkToRemove = work;
								_exchanger.SelectedWorkToRemove = null;
								DeleteUselessWorkWindow dialog = new DeleteUselessWorkWindow();
								if (dialog.ShowDialog() == true)
								{
									thereIsNoRepeatedWorks = false;
									toRemove.Clear();
									break;
								}
								else
								{
									_exchanger.FirstWorkToRemove = null;
									_exchanger.SecondWorkToRemove = null;
									_exchanger.SelectedWorkToRemove = null;
									throw new Exception("Отказ удалять частично совпадающие работы. Обработка завершена");
								}
							}
						}
					}
					prevWork = work;
				}
				foreach (var work in toRemove)
				{
					works.Remove(work);
					Log.Add($"Работа {work} удалена из-за полного повтора");
				}
			} while (!thereIsNoRepeatedWorks);
		}

		private int FindStartVertex(List<Work> works)
		{
			List<int> vertecies = GetVerteciesList(works);
			List<int> startVertecies = new List<int>();
			foreach (int vertex in vertecies)
			{
				if (VerterxHasNoInsideEdges(works, vertex))
				{
					startVertecies.Add(vertex);
				}
			}
			if (startVertecies.Count > 1)
			{
				throw new SeveralVerticesFoundException("Найдено несколько " +
					"начальных вершин")
				{
					Vertcies = startVertecies,
					EditingMode = EditingMode.StartVertexMode
				};
			}
			else if (startVertecies.Count == 0)
			{
				throw new NoVerticesFoundException("Не найдено начальных вершин")
				{
					EditingMode = EditingMode.StartVertexMode
				};
			}
			return startVertecies[0];
		}

		private List<int> GetVerteciesList(List<Work> works)
		{
			List<int> vertecies = new List<int>();
			foreach (Work work in works)
			{
				if (!vertecies.Contains(work.FirstEventID))
					vertecies.Add(work.FirstEventID);
				if (!vertecies.Contains(work.SecondEventID))
					vertecies.Add(work.SecondEventID);
			}
			return vertecies;
		}

		private void RemoveLoops(List<Work> works)
		{
			List<Work> toRemove = new List<Work>();
			foreach (Work work in works)
			{
				if (work.FirstEventID == work.SecondEventID)
				{
					toRemove.Add(work);
				}
			}
			foreach (Work work in toRemove)
			{
				works.Remove(work);
				Log.Add($"Петля {work.FirstEventID} → {work.SecondEventID} удалена");
			}
		}

		private void FindCycles(List<Work> works)
		{
			List<int> vertecies = new List<int>();
			foreach (Work work in works)
			{
				if (!vertecies.Contains(work.FirstEventID))
					vertecies.Add(work.FirstEventID);
				if (!vertecies.Contains(work.SecondEventID))
					vertecies.Add(work.SecondEventID);
			}
			List<Work> edges = new List<Work>();
			CopySourceTableToWorkingTable(works, edges);
			int currentVertex;
			int currentVertexIndex = 0;
			while (currentVertexIndex < vertecies.Count)
			{
				currentVertex = vertecies[currentVertexIndex];
				currentVertexIndex++;
				if (VertexHasNoOutsideEdges(edges, currentVertex)
					|| VerterxHasNoInsideEdges(edges, currentVertex))
				{
					RemoveEdgeWithSpecifiedVertex(edges, currentVertex);
					vertecies.Remove(currentVertex);
					currentVertexIndex = 0;
				}
			}
			if (edges.Count > 0)
				throw new CyclesFoundException(edges, vertecies);
		}

		private bool VertexHasNoOutsideEdges(List<Work> edges, int vertexID)
		{
			foreach (Work work in edges)
			{
				if (work.FirstEventID == vertexID)
					return false;
			}
			return true;
		}

		private bool VerterxHasNoInsideEdges(List<Work> edges, int vertexID)
		{
			foreach (Work edge in edges)
			{
				if (edge.SecondEventID == vertexID)
					return false;
			}
			return true;
		}

		private void RemoveEdgeWithSpecifiedVertex(List<Work> works, int vertexID)
		{
			List<Work> toRemove = new List<Work>();
			foreach (Work work in works)
			{
				if (work.FirstEventID == vertexID || work.SecondEventID == vertexID)
					toRemove.Add(work);
			}
			foreach (Work work in toRemove)
			{
				works.Remove(work);
			}
		}

		private List<Work> Streamline(List<Work> source, int startVertex)
		{
			List<int> processedVerticies = new List<int>();
			List<Work> works = new List<Work>();			
			CopySourceTableToWorkingTable(source, works);
			//
			// найти начальную вершину в таблице и переместить все работы в начало
			//
			MoveStartWorksToTheBegining(source, startVertex);
			List<Work> result = new List<Work>();
			Queue<int> toProcess = new Queue<int>(source.Count / 2);
			toProcess.Enqueue(works[0].FirstEventID);
			int currentVertex;
			List<Work> toRemove = new List<Work>();
			while (toProcess.Count > 0)
			{
				currentVertex = toProcess.Dequeue();
				foreach (Work work in works)
				{
					if (work.FirstEventID == currentVertex)
					{
						if (!processedVerticies.Contains(work.SecondEventID))
						{
							toProcess.Enqueue(work.SecondEventID);
						}
						result.Add(work);
						toRemove.Add(work);
					}
				}
				foreach (var item in toRemove)
				{
					works.Remove(item);
				}
				toRemove.Clear();
			}
			return result;
		}

		private void MoveStartWorksToTheBegining(List<Work> source, int startVertex)
		{
			List<Work> buffer = new List<Work>();
			foreach (Work work in source)
			{
				if (work.FirstEventID == startVertex)
				{
					buffer.Add(work);
				}
			}
			foreach (Work work in buffer)
			{
				source.Remove(work);
			}
			source.InsertRange(0, buffer);
		}

		private int FindEndVertex(List<Work> source)
		{
			List<int> vertices = GetVerteciesList(source);
			List<int> endVertices = new List<int>();
			foreach (int vertex in vertices)
			{
				if (VertexHasNoOutsideEdges(source, vertex))
					endVertices.Add(vertex);
			}
			if (endVertices.Count > 1)
				throw new SeveralVerticesFoundException("Найдено несколько конечных вершин")
				{
					Vertcies = endVertices,
					EditingMode = EditingMode.EndVertexMode
				};
			else if (endVertices.Count == 0)
				throw new NoVerticesFoundException("Конечных вершин не найдено")
				{
					EditingMode = EditingMode.EndVertexMode
				};
			return endVertices[0];
		}

		private void FindAllPaths(List<Work> table, int currentWorkIndex, int currentVertex, int endVertex, List<int> currentPath)
		{
			currentPath.Add(currentVertex);
			if (currentVertex == endVertex)
			{
				string path = "";
				foreach (int vertex in currentPath)
				{
					path += vertex.ToString() + " ";
				}
				FullPathsInTheGraph.Add(path);
				currentPath.Remove(currentVertex);
				return;
			}
			int endIndex = currentWorkIndex;
			for (int i = currentWorkIndex + 1; i < table.Count; i++)
			{
				if (table[i].FirstEventID != table[endIndex].FirstEventID)
				{
					endIndex = i;
					break;
				}
			}
			int nextWorkIndex;
			for (int i = currentWorkIndex; i < endIndex; i++)
			{
				for (nextWorkIndex = i; nextWorkIndex < table.Count; nextWorkIndex++)
				{
					if (table[nextWorkIndex].FirstEventID == table[i].SecondEventID)
						break;
				}
				FindAllPaths(table, nextWorkIndex, table[i].SecondEventID, endVertex, currentPath);
			}
			currentPath.Remove(currentVertex);
			return;
		}
	}
}
