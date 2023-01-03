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
            SelectStudentClass(monHoc.MaMon);
            mon = monHoc;
        }
		async void SelectStudentClass(string mamon)
		{

			HttpClient httpClient = new HttpClient();
			var lstLop = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/LopHoc?MaMon=" + mamon.ToString());
			var lstLopConverted = JsonConvert.DeserializeObject<List<LopHoc>>(lstLop);
			//So sánh danh sách tất cả các môn với danh sách các môn => chỉ chọn hiện thị những môn chưa chưa đăng ký
			LstLop.ItemsSource = lstLopConverted;
		}

		private void AddLop_Clicked(object sender, EventArgs e)
		{
			if (SinhVien.DangNhap.QuyenAdmin)
				Navigation.PushAsync(new PageAdThemLop(mon));
			else
			{
				DisplayAlert("Thông báo", "Bạn không được cấp quyền để thêm lớp", "OK");
			}
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			SelectStudentClass(mon.MaMon);
		}
	}
}