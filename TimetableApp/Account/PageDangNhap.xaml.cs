using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;

namespace TimetableApp.Account
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageDangNhap : ContentPage
    {
        HttpClient client;
        public PageDangNhap()
        {
            InitializeComponent();
            client = new HttpClient();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            // txtUsername.Text = "";
            txtPassword.Text = "";
        }

        private async void btnLogin_Clicked(object sender, EventArgs e)
        {
            string TenDangNhap = txtUsername.Text;
            string MatKhau = txtPassword.Text;
            string uri = $"http://lno-ie307.somee.com/api/TaiKhoan?TenDangNhap={TenDangNhap}&MatKhau={MatKhau}";
            try
            {
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    List<SinhVien> DSSVDangNhap = JsonConvert.DeserializeObject<List<SinhVien>>(content);
                    if (DSSVDangNhap.Count() == 1)
                    {
                        SinhVien.DangNhap = DSSVDangNhap[0];
                        await DisplayAlert("Đăng nhập thành công", "Xin chào, " + SinhVien.DangNhap.TenSV + "!", "OK");

                        string shellRoute = SinhVien.DangNhap.QuyenAdmin ? "//adminDefault" : "//studentDefault";
                        await Shell.Current.GoToAsync(shellRoute);
                    }
                    else
                    {
                        await DisplayAlert("Đăng nhập thất bại", "Tên đăng nhập hoặc mật khẩu không đúng", "Thử lại");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"\tERROR {0}", ex.Message);
            }
        }
    }
}