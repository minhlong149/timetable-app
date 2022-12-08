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
        
        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PageAdThemMon());
        }

        private void LstMonHoc_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            MonHoc monHoc = (MonHoc)e.Item;
            Navigation.PushAsync(new PageChiTietLop(monHoc));
        }

        private async void DelMon_Invoked(object sender, EventArgs e)
        {
               SwipeItem swipeItem = (SwipeItem)sender;
                MonHoc mon = swipeItem.CommandParameter as MonHoc;
                HttpClient httpClient = new HttpClient();

                HttpResponseMessage kq;
                kq = await httpClient.DeleteAsync("http://www.lno-ie307.somee.com/api/MonHoc?MaMon=" + mon.MaMon.ToString());
                var kqdel = await kq.Content.ReadAsStringAsync();
                if (int.Parse(kqdel.ToString()) > 0)
                    await DisplayAlert("Thông báo", "Đã xóa môn " + mon.MaMon.ToString() + " thành công!", "OK");
                else
                    await DisplayAlert("Thông báo", "Không thể xóa!\tVui lòng thử lại", "OK");
        }

        private void UpDateMon_Invoked(object sender, EventArgs e)
        {
            SwipeItem swipeItem = (SwipeItem)sender;
            MonHoc mon = swipeItem.CommandParameter as MonHoc;
            Navigation.PushAsync(new PageAdThemMon(mon));
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ListViewInit();
        }

      
    }
}