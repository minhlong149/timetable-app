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
	public partial class PageUnregistered : ContentPage
	{
		List<LopHoc> lops;
		public PageUnregistered()
		{
			InitializeComponent();
			Title = "Danh sách các lớp chưa đăng ký";
			ListUnregistered();
		}
		public async void ListUnregistered()
		{
			HttpClient httpClient = new HttpClient();
			var lstAll = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/LopHoc");
			var lstAllConverted = JsonConvert.DeserializeObject<List<LopHoc>>(lstAll);

			List<LopHoc> lopdk = new List<LopHoc>();
			var lstLopdk = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/LopHoc?MaSV=" + SinhVien.DangNhap.MaSV.ToString());
			var lstlopConverted = JsonConvert.DeserializeObject<List<LopHoc>>(lstLopdk);
			foreach (LopHoc all in lstAllConverted)
			{
				var t = 0;
				foreach (LopHoc lop in lstlopConverted)
				{
					if (all.MaLop == lop.MaLop)
					{
						t++;
						break;
					}
				}
				if (t==0)
				{
					lopdk.Add(all);
				}	

			}
			LstLop.ItemsSource = lopdk;
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

			kq = await httpClient.PostAsync("http://www.lno-ie307.somee.com/api/SinhVien?MaSV=" + SinhVien.DangNhap.MaSV.ToString() + "&MaLop=" + lopHoc.MaLop.ToString(), stringContent);
			string kqdk = await kq.Content.ReadAsStringAsync();
			if (int.Parse(kqdk.ToString()) > 0)
			{
				await DisplayAlert("Thông báo", "Bạn đã đăng ký lớp " + lopHoc.MaLop + " thành công!", "OK");
			}
			else
				await DisplayAlert("Thông báo", "Đã có lỗi xảy ra!\tVui lòng thử lại", "OK");

		}

		private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
		{
			var texto = searchBar.Text;
			LstLop.ItemsSource = lops.Where((x => x.TenMon.ToLower().Contains(texto) || x.MaLop.ToUpper().Contains(texto)));
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			ListUnregistered();
		}
	}
}