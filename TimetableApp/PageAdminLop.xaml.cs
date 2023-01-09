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

namespace TimetableApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageAdminLop : ContentPage
    {	
		MonHoc mon = new MonHoc();
        public PageAdminLop(MonHoc monHoc)
        {
            InitializeComponent();
            Title = monHoc.TenMon;
            ListClassInIt(mon.MaMon);
            mon = monHoc;
        }
		async void ListClassInIt(string mamon)
		{
			HttpClient httpClient = new HttpClient();
			var lstLop = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/LopHoc?MaMon=" + mamon);
			var lstLopConverted = JsonConvert.DeserializeObject<List<LopHoc>>(lstLop);

			LstLop.ItemsSource = lstLopConverted;
		}

		private void AddLop_Clicked(object sender, EventArgs e)
		{
				Navigation.PushAsync(new PageAdThemLop(mon));
		}
		

		private async void Del_Clicked(object sender, EventArgs e)
		{
			
			Button bt = (Button)sender;
			LopHoc lopHoc = (LopHoc)bt.BindingContext;
			HttpClient httpClient = new HttpClient();
			bool ans = await DisplayAlert("Cảnh báo", "Bạn có chắc chắn muốn xóa lớp " + lopHoc.MaLop + " ?", "Có", "Không");
			if (ans)
			{
				HttpResponseMessage kq;
				kq = await httpClient.DeleteAsync("http://www.lno-ie307.somee.com/api/LopHoc?maLop=" + lopHoc.MaLop.ToString());
				var kqdk = await kq.Content.ReadAsStringAsync();
				if (int.Parse(kqdk.ToString()) > 0)
				{
					await DisplayAlert("Thông báo", "Đã xóa lớp " + lopHoc.MaLop + " thành công!", "OK");

				}
				else
					await DisplayAlert("Thông báo", "Đã có lỗi xảy ra!\tVui lòng thử lại", "OK");
			}
			ListClassInIt(mon.MaMon);
		}

		private  void Update_Clicked(object sender, EventArgs e)
		{

			Button bt = (Button)sender;
			LopHoc lop= (LopHoc)bt.BindingContext;
			Navigation.PushAsync(new PageAdThemLop(lop));
		
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			ListClassInIt(mon.MaMon);
		}

		private void searchAd_TextChanged(object sender, TextChangedEventArgs e)
		{

		}
	}
	
}