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
    public partial class PageMonHoc : ContentPage
    {
        public PageMonHoc()
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

    private void LstMonHoc_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            MonHoc monHoc = (MonHoc)e.SelectedItem;
            Navigation.PushModalAsync(new PageChiTietLop(monHoc));
        }
    }
}