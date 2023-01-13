using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimetableApp.Class;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimetableApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PageAllClass : ContentPage
	{
		List<LopHoc> lops;
		public PageAllClass()
		{
			InitializeComponent();
			Title = "Danh sách tất cả các lớp đang mở";
			ListAllClass();
		}

		public async void ListAllClass()
		{
			HttpClient httpClient = new HttpClient();
			var lstAllClass = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/LopHoc");
			var lstAllClassConverted = JsonConvert.DeserializeObject<List<LopHoc>>(lstAllClass);
			lops = lstAllClassConverted;
			LstLop.ItemsSource = lstAllClassConverted;
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
		}

		private async void AddClass_Clicked(object sender, EventArgs e)
		{
			Button bt = (Button)sender;
			LopHoc lopHoc = (LopHoc)bt.BindingContext;
			HttpClient httpClient = new HttpClient();

			var lstLop = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/LopHoc?MaSV=" + SinhVien.DangNhap.MaSV.ToString());
			var lstLopConverted = JsonConvert.DeserializeObject<List<LopHoc>>(lstLop);

			string jsondk = JsonConvert.SerializeObject(lopHoc);
			StringContent stringContent = new StringContent(jsondk, Encoding.UTF8, "application/json");
			HttpResponseMessage kq;
			var daki = 0;
			foreach (LopHoc lop in lstLopConverted)
			{
				if (lopHoc.MaLop == lop.MaLop)
				{
					daki = daki + 1;
				}
			}
			/*Kiểm tra đã đăng ký hay chưa*/
			if (daki > 0)
				await DisplayAlert("Thông báo", "Bạn đã đăng ký lớp " + lopHoc.MaLop, "OK");
			else if (daki == 0)
			{
				kq = await httpClient.PostAsync("http://www.lno-ie307.somee.com/api/SinhVien?MaSV=" + SinhVien.DangNhap.MaSV.ToString() + "&MaLop=" + lopHoc.MaLop.ToString(), stringContent);
				string kqdk = await kq.Content.ReadAsStringAsync();
				if (int.Parse(kqdk.ToString()) > 0)
				{
					await DisplayAlert("Thông báo", "Bạn đã đăng ký lớp " + lopHoc.MaLop + " thành công!", "OK");
				}
				else
					await DisplayAlert("Thông báo", "Đã có lỗi xảy ra!\tVui lòng thử lại", "OK");
			}
		}

		private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
		{
			var texto = searchBar.Text;
			LstLop.ItemsSource = lops.Where((x => x.TenMon.ToLower().Contains(texto) ||  x.MaLop.ToUpper().Contains(texto)));

		}
	}
}