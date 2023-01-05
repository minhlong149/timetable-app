using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        List<MonHoc> mons;
        public PageMonHoc()
        {
            InitializeComponent();
            Title = "Danh sách các môn học";
            ListViewInit();
        }
        async void ListViewInit()
        {
            mons= new List<MonHoc>();
			HttpClient httpClient = new HttpClient();
            var lstMon = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/MonHoc");
            var lstMonConverted = JsonConvert.DeserializeObject<List<MonHoc>>(lstMon);
            mons = lstMonConverted;
            LstMonHoc.ItemsSource = lstMonConverted;
        }


        private void LstMonHoc_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            MonHoc monHoc = (MonHoc)e.Item;
            Navigation.PushAsync(new PageChiTietLop(monHoc));
        }

		private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
		{
			var texto = searchBar.Text;
			LstMonHoc.ItemsSource = mons.Where(x => x.TenMon.ToLower().Contains(texto));
		}
    }
}