using Lab2.Infrastructure.Commands;
using Lab2.Models.Data;
using Lab2.Models.Services;
using Microsoft.Extensions.DependencyInjection;
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
		private Exchanger _exchanger;
		public DeleteUselessWorkWindow()
		{
			InitializeComponent();
			_exchanger = App.Services.GetRequiredService<Exchanger>();
			FirstWorkToDelete = $"Удалить {_exchanger.FirstWorkToRemove}";
			SecondWorkToDelete = $"Удалить {_exchanger.SecondWorkToRemove}";
			DeleteFirstWorkIsNecessary = true;
			DeleteSecondWorkIsNecessary = false;
			DeleteUselessWorkCommand = new LambdaCommand(OnDeleteUselessWorkCommandExecuted,
				CanDeleteUselessWorkCommandExecute);
		}


		public ICommand DeleteUselessWorkCommand
		{
			get { return (ICommand)GetValue(DeleteUselessWorkCommandProperty); }
			set { SetValue(DeleteUselessWorkCommandProperty, value); }
		}

		// Using a DependencyProperty as the backing store for DeleteUselessWorkCommand.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty DeleteUselessWorkCommandProperty =
			DependencyProperty.Register(
				"DeleteUselessWorkCommand",
				typeof(ICommand), 
				typeof(DeleteUselessWorkWindow),
				new PropertyMetadata(default(ICommand)));

		private void OnDeleteUselessWorkCommandExecuted(object p)
		{
			this.DialogResult = true;
			if (DeleteFirstWorkIsNecessary)
			{
				_exchanger.SelectedWorkToRemove = _exchanger.FirstWorkToRemove;
			}
			else
			{
				_exchanger.SelectedWorkToRemove = _exchanger.SecondWorkToRemove;
			}
			this.Close();
		}
		private bool CanDeleteUselessWorkCommandExecute(object p) => DeleteFirstWorkIsNecessary 
			|| DeleteSecondWorkIsNecessary;

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
