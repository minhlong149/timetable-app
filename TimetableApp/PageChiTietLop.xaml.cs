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
        MonHoc mon = new MonHoc();
        public PageChiTietLop(MonHoc monHoc)
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

        private async void Add_Invoked(object sender, EventArgs e)
        {
            if (SinhVien.DangNhap.QuyenAdmin)
            {
                await DisplayAlert("Thông báo", "Đây là chức năng của sinh viên!", "OK");
            }
            else
            {
                SwipeItem swipeItem = (SwipeItem)sender;
                LopHoc lopHoc = swipeItem.CommandParameter as LopHoc;
                HttpClient httpClient = new HttpClient();

                var lstLop = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/SinhVien?MaSV=" + SinhVien.DangNhap.MaSV.ToString());
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
                    var kqdk = await kq.Content.ReadAsStringAsync();
                    if (int.Parse(kqdk.ToString()) > 0)
                    {
                        await DisplayAlert("Thông báo", "Bạn đã đăng ký lớp " + lopHoc.MaLop + " thành công!", "OK");
                    }
                    else
                        await DisplayAlert("Thông báo", "Đã có lỗi xảy ra!\tVui lòng thử lại", "OK");
                }
            }
        }


   //ADMIN
        private async void Del_Invoked(object sender, EventArgs e)
        {
            if (SinhVien.DangNhap.QuyenAdmin)
            {
                SwipeItem swipeItem = (SwipeItem)sender;
                LopHoc lopHoc = swipeItem.CommandParameter as LopHoc;
                HttpClient httpClient = new HttpClient();

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
            else
               await DisplayAlert("Thông báo", "Bạn không được cấp quyền xóa!\t Vui lòng liên hệ admin", "OK");
            await Navigation.PopAsync();
                
        }

        private void AddLop_Clicked(object sender, EventArgs e)
        {
            if(SinhVien.DangNhap.QuyenAdmin)
                Navigation.PushAsync(new PageAdThemLop(mon));
            else
            {
                DisplayAlert("Thông báo", "Bạn không được cấp quyền để thêm lớp", "OK");
            }    

        }
        

        private void Up_Invoked(object sender, EventArgs e)
        {
            if (SinhVien.DangNhap.QuyenAdmin)
            {
                SwipeItem swipeItem = (SwipeItem)sender;
                LopHoc lop = swipeItem.CommandParameter as LopHoc;
                Navigation.PushAsync(new PageAdThemLop(lop));
            }
            else
            {
                DisplayAlert("Thông báo", "Bạn không được cấp quyền để sửa lớp", "OK");
            }
            
        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            SelectStudentClass(mon.MaMon);
        }
    }
}