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
	public partial class PageThongBao : ContentPage
	{
		public PageThongBao ()
		{
			InitializeComponent ();
            Title = "Thông báo";
            SelectAllNotification();
        }
        async void SelectAllNotification()
        {
            HttpClient httpClient = new HttpClient();
            var lstTB = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/Notification?MaSV=" + SinhVien.DangNhap.MaSV.ToString());
            var lstTBConverted = JsonConvert.DeserializeObject<List<ThongBao>>(lstTB);
            LstThongBao.ItemsSource = lstTBConverted;
        }
    }
}