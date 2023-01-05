using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimetableApp.Class;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.TizenSpecific;
using Xamarin.Forms.Xaml;

namespace TimetableApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PageSchedule : ContentPage
	{
		public PageSchedule()
		{
			InitializeComponent();
			Calendar calendar = new Calendar();
			DateTime dt = DateTime.Today;
			calendar.Day = dt.Day;
			calendar.Month = dt.Month;
			calendar.Year = dt.Year;
			calendar.DayOfWeek = ((int)dt.DayOfWeek);
			Console.WriteLine(calendar.DayOfWeek);
			Week.BindingContext = calendar;
			if (calendar.DayOfWeek == 1)
				Mon_Clicked(Mon, EventArgs.Empty);
			else if (calendar.DayOfWeek == 2)
				Tue_Clicked(Tue, EventArgs.Empty);
			else if (calendar.DayOfWeek == 3)
				Wed_Clicked(Wed, EventArgs.Empty);
			else if (calendar.DayOfWeek == 4)
				Thu_Clicked(Thu, EventArgs.Empty);
			else if (calendar.DayOfWeek == 5)
				Fri_Clicked(Fri, EventArgs.Empty);
			else if (calendar.DayOfWeek == 6)
				Sat_Clicked(Sat, EventArgs.Empty);
			else if (calendar.DayOfWeek == 7)
				Sun_Clicked(Sun, EventArgs.Empty);
		}
		
		private async void Sun_Clicked(object sender, EventArgs e)
		{
			HttpClient httpClient = new HttpClient();
			var lstLop = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/LopHoc?MaSV=" + SinhVien.DangNhap.MaSV.ToString());
			var lstLopConverted = JsonConvert.DeserializeObject<List<LopHoc>>(lstLop);
			List<LopHoc> dsLop = new List<LopHoc>();
			foreach (LopHoc lop in lstLopConverted)
				if (lop.Thu == "CN")
					dsLop.Add(lop);
			LstLopHN.ItemsSource = dsLop;
		}


		private async void Mon_Clicked(object sender, EventArgs e)
		{
			HttpClient httpClient = new HttpClient();
			var lstLop = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/LopHoc?MaSV=" + SinhVien.DangNhap.MaSV.ToString());
			var lstLopConverted = JsonConvert.DeserializeObject<List<LopHoc>>(lstLop);
			List<LopHoc> dsLop = new List<LopHoc>();
			foreach (LopHoc lop in lstLopConverted)
				if (lop.Thu == "2")
					dsLop.Add(lop);
			LstLopHN.ItemsSource = dsLop;
		}

		private async void Tue_Clicked(object sender, EventArgs e)
		{
			HttpClient httpClient = new HttpClient();
			var lstLop = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/LopHoc?MaSV=" + SinhVien.DangNhap.MaSV.ToString());
			var lstLopConverted = JsonConvert.DeserializeObject<List<LopHoc>>(lstLop);
			List<LopHoc> dsLop = new List<LopHoc>();
			foreach (LopHoc lop in lstLopConverted)
				if (lop.Thu == "3")
					dsLop.Add(lop);
			LstLopHN.ItemsSource = dsLop;
		}

		private async void Wed_Clicked(object sender, EventArgs e)
		{
			HttpClient httpClient = new HttpClient();
			var lstLop = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/LopHoc?MaSV=" + SinhVien.DangNhap.MaSV.ToString());
			var lstLopConverted = JsonConvert.DeserializeObject<List<LopHoc>>(lstLop);
			List<LopHoc> dsLop = new List<LopHoc>();
			foreach (LopHoc lop in lstLopConverted)
				if (lop.Thu == "4")
					dsLop.Add(lop);
			LstLopHN.ItemsSource = dsLop;
		}


		private async void Thu_Clicked(object sender, EventArgs e)
		{

			HttpClient httpClient = new HttpClient();
			var lstLop = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/LopHoc?MaSV=" + SinhVien.DangNhap.MaSV.ToString());
			var lstLopConverted = JsonConvert.DeserializeObject<List<LopHoc>>(lstLop);
			List<LopHoc> dsLop = new List<LopHoc>();
			foreach (LopHoc lop in lstLopConverted)
				if (lop.Thu == "5")
					dsLop.Add(lop);
			LstLopHN.ItemsSource = dsLop;
		}

		private async void Fri_Clicked(object sender, EventArgs e)
		{
			
			HttpClient httpClient = new HttpClient();
			var lstLop = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/LopHoc?MaSV=" + SinhVien.DangNhap.MaSV.ToString());
			var lstLopConverted = JsonConvert.DeserializeObject<List<LopHoc>>(lstLop);
			List<LopHoc> dsLop = new List<LopHoc>();
			foreach (LopHoc lop in lstLopConverted)
				if (lop.Thu == "6")
					dsLop.Add(lop);
			LstLopHN.ItemsSource = dsLop;
		}

		private async void Sat_Clicked(object sender, EventArgs e)
		{
			HttpClient httpClient = new HttpClient();
			var lstLop = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/LopHoc?MaSV=" + SinhVien.DangNhap.MaSV.ToString());
			var lstLopConverted = JsonConvert.DeserializeObject<List<LopHoc>>(lstLop);
			List<LopHoc> dsLop = new List<LopHoc>();
			foreach (LopHoc lop in lstLopConverted)
				if (lop.Thu == "7")
					dsLop.Add(lop);
			LstLopHN.ItemsSource = dsLop;
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			
		}
	}
}

	