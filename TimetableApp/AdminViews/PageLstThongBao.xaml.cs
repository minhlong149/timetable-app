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
    public partial class PageLstThongBao : ContentPage
    {
        public PageLstThongBao()
        {
            InitializeComponent();
            Title = "Các thông báo đã gửi";
            SelectAllNotification();
        }
        async void SelectAllNotification()
        {

            HttpClient httpClient = new HttpClient();
            var lstTB = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/Notification");
            var lstTBConverted = JsonConvert.DeserializeObject<List<ThongBao>>(lstTB);
            LstThongBao.ItemsSource = lstTBConverted;
        }
    }
}