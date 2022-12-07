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
    public partial class PageAdminMon : ContentPage
    {
        public PageAdminMon()
        {
            InitializeComponent();
            ListViewInit();
        }
        async void ListViewInit()
        {
            HttpClient httpClient = new HttpClient();
            var lstMon = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/MonHoc");
            var lstMonConverted = JsonConvert.DeserializeObject<List<MonHoc>>(lstMon);
            LstMonHoc.ItemsSource = lstMonConverted;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ListViewInit();
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PageAdThemMon());
        }

        private void LstMonHoc_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            MonHoc monHoc = (MonHoc)e.Item;
            Navigation.PushAsync(new PageChiTietLop(monHoc));
        }
    }
}