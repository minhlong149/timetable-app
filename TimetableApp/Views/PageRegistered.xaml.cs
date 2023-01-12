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
	public partial class PageRegistered : ContentPage
	{
		List<LopHoc> lops;
		public PageRegistered()
		{
			InitializeComponent();
			Title = "Danh sách các lớp đã đăng ký";
			ListRegistered();
		}

		public async void ListRegistered()
		{
			HttpClient httpClient = new HttpClient();

			List<MonHoc> mondk = new List<MonHoc>();
			var lstLopdk = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/LopHoc?MaSV=" + SinhVien.DangNhap.MaSV.ToString());
			var lstlopConverted = JsonConvert.DeserializeObject<List<LopHoc>>(lstLopdk);
		
			LstLop.ItemsSource = lstlopConverted;
		}
		private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
		{
			var texto = searchBar.Text;
			LstLop.ItemsSource = lops.Where((x => x.TenMon.ToLower().Contains(texto) || x.MaLop.ToUpper().Contains(texto)));
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

		protected override void OnAppearing()
		{
			base.OnAppearing();
			ListRegistered();
		}
	}
}