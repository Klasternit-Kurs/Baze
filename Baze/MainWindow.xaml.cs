using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
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

			Osoba o = new Osoba("Pera", "Peric");
			o.Brojevi.Add(new Vrednost(5));
			o.Brojevi.Add(new Vrednost(15));
			o.Brojevi.Add(new Vrednost(23));
			Adresa a = new Adresa("Grad", "PO", "Ulica", "Neki broj");
			o.Adrese.Add(a);
			o.Adrese.Add(new Adresa("Grad2", "PO3", "Ulica4", "Neki broj5"));
			b.Osobas.Add(o);

			Osoba o2 = new Osoba("Pera2", "Peric2");
			o2.Adrese.Add(a);
			b.Osobas.Add(o2);

			b.SaveChanges();
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

	public class Vrednost
	{
		public int ID { get; set; }
		public int Broj { get; set; }
		public Vrednost(int i) => Broj = i;
		public Vrednost() { }
	}
	public class Osoba
	{
		public int ID { get; set; }
		public string Ime { get; set; }
		public string Prezime { get; set; }
		public List<Adresa> Adrese { get; set; } = new List<Adresa>();
		public List<Vrednost> Brojevi { get; set; } = new List<Vrednost>();

		public Osoba(string i, string p)
		{
			Ime = i;
			Prezime = p;
		}

		public Osoba() { }
	}

	public class Adresa
	{
		public int ID { get; set; }
		public string Grad { get; set; }
		public string Postanski { get; set; }
		public string Ulica { get; set; }
		public string Broj { get; set; }
		public List<Osoba> Osobe { get; set; } = new List<Osoba>();

		public Adresa (string g, string p, string u, string b)
		{
			Grad = g;
			Postanski = p;
			Ulica = u;
			Broj = b;
		}

		public Adresa() { }
	}

	public class Baza:DbContext
	{
		public Baza() :base(@"Data Source=DESKTOP-75VO5EN\TESTSERVER;Initial Catalog = EFBaza;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")
		{}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Osoba>().HasKey(o => o.ID)
										.HasMany(o => o.Adrese)
										.WithMany(a => a.Osobe);
			
			modelBuilder.Entity<Adresa>().HasKey(a => a.ID);
			modelBuilder.Entity<Vrednost>().HasKey(v => v.ID);
		}

		public DbSet<Osoba> Osobas { get; set; }
		public DbSet<Adresa> Adresas { get; set; }
	}
}
