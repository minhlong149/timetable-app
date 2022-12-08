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
            GetDeadlineByMaSVAndMaLop();
        }
        async void GetDeadlineByMaSVAndMaLop()
        {
            HttpClient httpClient = new HttpClient();
            var lstDeadline = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/Homework?MaSV=" + SinhVien.DangNhap.MaSV.ToString());
            var lstDeadlineConverted = JsonConvert.DeserializeObject<List<Deadline>>(lstDeadline);
            LstDeadline.ItemsSource = lstDeadlineConverted;
        }

        private void LstDeadline_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Deadline deadline= (Deadline)e.SelectedItem;
            Navigation.PushModalAsync(new PageChiTietDeadline(deadline));
        }
    }
}