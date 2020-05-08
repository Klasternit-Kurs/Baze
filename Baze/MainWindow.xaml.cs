using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Baze
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private string pretraga;
		public string Pretraga 
		{
			get => pretraga; 
			set
			{
				pretraga = value;

				Baza b = new Baza();
				if (string.IsNullOrWhiteSpace(pretraga))
					dg.ItemsSource = b.Osobas.ToList();
				else
					dg.ItemsSource = b.Osobas.Where(o => o.Ime.Contains(pretraga.Trim()) || o.Prezime.Contains(pretraga.Trim()))
					                         .ToList();
			}
		}

		public MainWindow()
		{
			InitializeComponent();
			DataContext = new Osoba();
			txtPretraga.DataContext = this;
			Baza b = new Baza();
			dg.ItemsSource = b.Osobas.ToList();
		}

		private void UBazu(object sender, RoutedEventArgs e)
		{
			Baza b = new Baza();
			b.Osobas.Add(DataContext as Osoba);
			b.SaveChanges();
			dg.ItemsSource = b.Osobas.ToList();
			DataContext = new Osoba();
		}
	}

	public class Osoba
	{
		public int ID { get; set; }
		public string Ime { get; set; }
		public string Prezime { get; set; }

		public Osoba(string i, string p)
		{
			Ime = i;
			Prezime = p;
		}

		public Osoba() { }
	}

	public class Baza:DbContext
	{
		public Baza() :base(@"Data Source=DESKTOP-75VO5EN\TESTSERVER;Initial Catalog = EFBaza;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
		{}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Osoba>().HasKey(o => o.ID);
		}

		public DbSet<Osoba> Osobas { get; set; }
	}
}
