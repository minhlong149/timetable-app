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
    public partial class PageAdThemMon : ContentPage
    {
        MonHoc _mon;
        public PageAdThemMon()
        {
            InitializeComponent();

        }

        public PageAdThemMon(MonHoc monHoc)
        {
            InitializeComponent();
            Title = "Sửa môn học";
            _mon = monHoc;
            AddMaMon.Text = monHoc.MaMon;
            AddTenMon.Text= monHoc.TenMon;
            AddTC.Text = monHoc.SoTC.ToString();
            AddMaMon.Focus();

        }
        private async void Save_Clicked(object sender, EventArgs e)
        {
            if (_mon != null)
            { 
                
                _mon.MaMon = AddMaMon.Text;
                _mon.TenMon = AddTenMon.Text;
                _mon.SoTC = int.Parse(AddTC.Text.ToString());
               

                HttpClient httpClient = new HttpClient();
                string jsonup = JsonConvert.SerializeObject(_mon);
                StringContent stringContent = new StringContent(jsonup, Encoding.UTF8, "application/json");
                HttpResponseMessage kq;

                kq = await httpClient.PutAsync("http://www.lno-ie307.somee.com/api/MonHoc", stringContent);
                var kqthem = await kq.Content.ReadAsStringAsync();

                if (int.Parse(kqthem.ToString()) > 0)
                {
                    await DisplayAlert("Thông báo", "Sửa môn học thành công", "OK");
                }
                else
                    await DisplayAlert("Thông báo", "Sửa môn học không thành công", "Thử lại");
            }
            else
            {


                MonHoc _monHoc = new MonHoc();
                _monHoc.MaMon = AddMaMon.Text;
                _monHoc.TenMon = AddTenMon.Text;
                _monHoc.SoTC = int.Parse(AddTC.Text.ToString());


                HttpClient httpClient = new HttpClient();
                string json = JsonConvert.SerializeObject(_monHoc);
                StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage kq;

                kq = await httpClient.PostAsync("http://www.lno-ie307.somee.com/api/MonHoc", stringContent);
                var kqthem = await kq.Content.ReadAsStringAsync();
                if (int.Parse(kqthem.ToString()) > 0)
                {
                    await DisplayAlert("Thông báo", "Thêm môn học thành công", "OK");
                }
                else
                    await DisplayAlert("Thông báo", "Thêm môn học không thành công", "Thử lại");
            }
            await Navigation.PopAsync();
        }
    }
}