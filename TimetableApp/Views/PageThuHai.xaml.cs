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

namespace TimetableApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageThuHai : ContentPage
    {
        public PageThuHai()
        {
            InitializeComponent();
            Title = "Thời khóa biểu";
            ListViewInit();
        }
        async void ListViewInit()
        {
            HttpClient httpClient = new HttpClient();
            var lstLop = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/LopHoc?MaSV=" + SinhVien.DangNhap.MaSV.ToString());
            var lstLopConverted = JsonConvert.DeserializeObject<List<LopHoc>>(lstLop);
            List<LopHoc> dsLop = new List<LopHoc>();
            foreach (LopHoc lop in lstLopConverted)
                if (lop.Thu == "2")
                    dsLop.Add(lop);
            LsLopHN.ItemsSource = dsLop;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ListViewInit();
        }

        
    }
}