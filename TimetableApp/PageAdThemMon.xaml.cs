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
        
        public PageAdThemMon()
        {
            InitializeComponent();
        }
       
        private async void Save_Clicked(object sender, EventArgs e)
        {
            MonHoc _monHoc = new MonHoc();
            _monHoc.MaMon = AddMaMon.Text;
            _monHoc.TenMon = AddTenMon.Text;
            _monHoc.SoTC = int.Parse(AddTC.Text.ToString());


            HttpClient httpClient = new HttpClient();
            string json = JsonConvert.SerializeObject(_monHoc);
            StringContent stringContent = new StringContent(json, Encoding.UTF8,"application/json");
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
    }
}