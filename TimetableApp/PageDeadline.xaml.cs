using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TimetableApp.Class;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimetableApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageDeadline : ContentPage
    {
        public PageDeadline()
        {
            InitializeComponent();
            GetDeadlines();
        }

        async void GetDeadlines()
        {
            HttpClient httpClient = new HttpClient();
            var lstDeadline = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/Homework?MaSV=" + SinhVien.DangNhap.MaSV.ToString());
            var lstDeadlineConverted = JsonConvert.DeserializeObject<List<Deadline>>(lstDeadline);
            LstDeadline.ItemsSource = lstDeadlineConverted;
        }

        private void TIAddDeadline_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PageAdDeadline());
        }

        //private async void DelDeadline_Invoked(object sender, EventArgs e)
        //{
        //    SwipeItem swipeItem = (SwipeItem)sender;
        //    Deadline deadline = swipeItem.CommandParameter as Deadline;
        //    HttpClient httpClient = new HttpClient();

        //    HttpResponseMessage kq;
        //    kq = await httpClient.DeleteAsync("http://www.lno-ie307.somee.com/api/Homework?ID=" + deadline.ID.ToString());
        //    var kqdel = await kq.Content.ReadAsStringAsync();
        //    if (int.Parse(kqdel.ToString()) > 0)
        //        await DisplayAlert("Thông báo", "Đã xóa deadline thành công!", "OK");
        //    else
        //        await DisplayAlert("Thông báo", "Không thể xóa!\tVui lòng thử lại", "OK");
        //}

        //private void UpDateDeadline_Invoked(object sender, EventArgs e)
        //{
        //    SwipeItem swipeItem = (SwipeItem)sender;
        //    Deadline deadline = swipeItem.CommandParameter as Deadline;
        //    Navigation.PushAsync(new PageAdDeadline(deadline));
        //}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GetDeadlines();
        }

        private void LstDeadline_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Deadline deadline= (Deadline)e.Item;
            Navigation.PushAsync(new PageChiTietDeadline(deadline));
        }
    }
}