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

namespace TimetableApp.AdminViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageTaoThongBao : ContentPage
    {
        public PageTaoThongBao()
        {
            InitializeComponent();
            GetMaSinhVien();
            AddTieuDe.Focus();
            AddNoiDung.Focus();
        }
        public async void GetMaSinhVien()
        {
            HttpClient httpClient = new HttpClient();
            var lstMaLop = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/SinhVien");
            var lstMaLopConverted = JsonConvert.DeserializeObject<List<Deadline>>(lstMaLop);
            picker.ItemsSource = lstMaLopConverted;
        }


        private async void Save_Clicked(object sender, EventArgs e)
        {
            ThongBao _ThongBao = new ThongBao();
            _ThongBao.MaSV = picker.ItemDisplayBinding.StringFormat.ToString();
            _ThongBao.TieuDe = AddTieuDe.Text;
            _ThongBao.NoiDung = AddNoiDung.Text;
            _ThongBao.ThoiGian = DateTime.Now;

            HttpClient httpClient = new HttpClient();
            string json = JsonConvert.SerializeObject(_ThongBao);
            StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage kq;

            kq = await httpClient.PostAsync("http://www.lno-ie307.somee.com/api/Notification", stringContent);
            var kqthem = await kq.Content.ReadAsStringAsync();
            if (int.Parse(kqthem.ToString()) > 0)
            {
                await DisplayAlert("Thông báo", "Thêm thông báo thành công", "OK");
            }
            else
                await DisplayAlert("Thông báo", "Thêm thông báo không thành công", "Thử lại");

            await Navigation.PushAsync(new PageTaoThongBao());
        }
    }
}