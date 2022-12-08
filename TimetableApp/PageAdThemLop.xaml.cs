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
    public partial class PageAdThemLop : ContentPage
    {
        MonHoc mon = new MonHoc();
        public PageAdThemLop(MonHoc monHoc)
        {
            InitializeComponent();
            Title = "Thêm "+ monHoc.TenMon;
            mon = monHoc;
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
                LopHoc lopHoc = new LopHoc();
                lopHoc.MaMon = mon.MaMon; 
                lopHoc.GiaoVien = AddGV.Text;
                lopHoc.Thu = AddThu.Text;
                lopHoc.Tiet = AddTiet.Text;
                lopHoc.PhongHoc = AddPhong.Text; /*Tạo sẵn list các phòng và để picker*/


                HttpClient httpClient = new HttpClient();
                string json = JsonConvert.SerializeObject(lopHoc);
                StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage kq;

                kq = await httpClient.PostAsync("http://www.lno-ie307.somee.com/api/LopHoc", stringContent);
                var kqthem = await kq.Content.ReadAsStringAsync();
                if (int.Parse(kqthem.ToString()) > 0)
                {
                    await DisplayAlert("Thông báo", "Thêm lớp thành công", "OK");
                }
                else
                    await DisplayAlert("Thông báo", "Thêm lớp không thành công", "Thử lại");

        }
    }
}