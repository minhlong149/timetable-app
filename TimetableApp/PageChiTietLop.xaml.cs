using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TimetableApp.Class;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;

namespace TimetableApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageChiTietLop : ContentPage
    {
        public PageChiTietLop(MonHoc monHoc)
        {
            InitializeComponent();
            SelectStudentClass(monHoc.MaMon);
            Title = monHoc.TenMon;
            

        }
        async void SelectStudentClass(string mamon)
        {

            HttpClient httpClient = new HttpClient();
            var lstLop = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/LopHoc?MaMon=" + mamon.ToString());
            var lstLopConverted = JsonConvert.DeserializeObject<List<LopHoc>>(lstLop);

            //So sánh danh sách tất cả các môn với danh sách các môn => chỉ chọn hiện thị những môn chưa chưa đăng ký
           
            LstLop.ItemsSource = lstLopConverted;
        }

        private async void Add_Invoked(object sender, EventArgs e)
        {
            SwipeItem swipeItem = (SwipeItem)sender;
            LopHoc lopHoc = swipeItem.CommandParameter as LopHoc;
            HttpClient httpClient = new HttpClient();

            var lstLop = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/SinhVien?MaSV=" + SinhVien.DangNhap.MaSV.ToString());
            var lstLopConverted = JsonConvert.DeserializeObject<List<LopHoc>>(lstLop);

            string jsondk = JsonConvert.SerializeObject(lopHoc);
            StringContent stringContent = new StringContent(jsondk,Encoding.UTF8,"application/json");
            HttpResponseMessage kq;
            var daki = 0;
            foreach (LopHoc lop in lstLopConverted)
            {
                if (lopHoc.MaLop == lop.MaLop)
                {
                    daki= daki+1;
                } 
            }
            /*Kiểm tra đã đăng ký hay chưa*/
            if (daki > 0)
                await DisplayAlert("Thông báo", "Bạn đã đăng ký lớp " + lopHoc.MaLop, "OK");

            else if (daki == 0)
            {
                kq = await httpClient.PostAsync("http://www.lno-ie307.somee.com/api/SinhVien?MaSV=" + SinhVien.DangNhap.MaSV.ToString() + "&MaLop=" + lopHoc.MaLop.ToString(),stringContent);
                var kqdk = await kq.Content.ReadAsStringAsync();
                if (int.Parse(kqdk.ToString()) >0)
                {
                    await DisplayAlert("Thông báo", "Bạn đã đăng ký lớp " + lopHoc.MaLop + " thành công!", "OK");

                }
                else
                    await DisplayAlert("Thông báo", "Đã có lỗi xảy ra!\tVui lòng thử lại", "OK");
            }
     
        }
      
    }
}