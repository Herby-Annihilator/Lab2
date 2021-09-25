using Lab2.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Markup;

namespace Lab2.ViewModels
{
	[MarkupExtensionReturnType(typeof(EditingWindowViewModel))]
	public class EditingWindowViewModel : ViewModel
	{
		#region Properties

		private string _title = "Редактироване вершин";
		public string Title { get => _title; set => Set(ref _title, value); }

		private string _status = "Редактироване вершин";
		public string Status { get => _status; set => Set(ref _status, value); }

		private string _meaningLine = "Редактироване вершин";
		public string MeaningLine { get => _meaningLine; set => Set(ref _meaningLine, value); }

		#endregion
	}
}
