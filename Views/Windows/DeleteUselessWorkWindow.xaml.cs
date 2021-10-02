using Lab2.Models.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Lab2.Views.Windows
{
	/// <summary>
	/// Логика взаимодействия для DeleteUselessWorkWindow.xaml
	/// </summary>
	public partial class DeleteUselessWorkWindow : Window
	{
		public DeleteUselessWorkWindow(Work firstWork, Work secondWork)
		{
			InitializeComponent();
			FirstWorkToDelete = $"Удалить {firstWork}";
			SecondWorkToDelete = $"Удалить {secondWork}";
			DeleteFirstWorkIsNecessary = true;
			DeleteSecondWorkIsNecessary = false;
		}



		public string FirstWorkToDelete
		{
			get { return (string)GetValue(FirstWorkToDeleteProperty); }
			set { SetValue(FirstWorkToDeleteProperty, value); }
		}

		// Using a DependencyProperty as the backing store for FirstWorkToDelete.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty FirstWorkToDeleteProperty =
			DependencyProperty.Register(
				"FirstWorkToDelete", 
				typeof(string), 
				typeof(DeleteUselessWorkWindow), 
				new PropertyMetadata(default(string)));



		public string SecondWorkToDelete
		{
			get { return (string)GetValue(SecondWorkToDeleteProperty); }
			set { SetValue(SecondWorkToDeleteProperty, value); }
		}

		// Using a DependencyProperty as the backing store for SecondWorkToDelete.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty SecondWorkToDeleteProperty =
			DependencyProperty.Register(
				"SecondWorkToDelete",
				typeof(string),
				typeof(DeleteUselessWorkWindow),
				new PropertyMetadata(default(string)));



		public bool DeleteFirstWorkIsNecessary
		{
			get { return (bool)GetValue(DeleteFirstWorkIsNecessaryProperty); }
			set { SetValue(DeleteFirstWorkIsNecessaryProperty, value); }
		}

		// Using a DependencyProperty as the backing store for DeleteFirstWorkIsNecessary.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty DeleteFirstWorkIsNecessaryProperty =
			DependencyProperty.Register(
				"DeleteFirstWorkIsNecessary",
				typeof(bool),
				typeof(DeleteUselessWorkWindow),
				new PropertyMetadata(default(bool)));



		public bool DeleteSecondWorkIsNecessary
		{
			get { return (bool)GetValue(DeleteSecondWorkIsNecessaryProperty); }
			set { SetValue(DeleteSecondWorkIsNecessaryProperty, value); }
		}

		// Using a DependencyProperty as the backing store for DeleteSecondWorkIsNecessary.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty DeleteSecondWorkIsNecessaryProperty =
			DependencyProperty.Register(
				"DeleteSecondWorkIsNecessary", 
				typeof(bool),
				typeof(DeleteUselessWorkWindow),
				new PropertyMetadata(default(bool)));

		private void RadioButton_Checked(object sender, RoutedEventArgs e)
		{
			DeleteFirstWorkIsNecessary = true;
			DeleteSecondWorkIsNecessary = false;
		}

		private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
		{
			DeleteFirstWorkIsNecessary = false;
			DeleteSecondWorkIsNecessary = true;
		}
	}
}
