using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimetableApp.AdminViews;
using TimetableApp.Class;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimetableApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageChiTietDeadline : ContentPage
    {
        Deadline _dl = new Deadline();

        public PageChiTietDeadline(Deadline deadline)
        {

            InitializeComponent();
            Title = "Chi tiết";
            _dl = deadline;
            AddTieuDe.Text = deadline.TieuDe;
            AddNoiDung.Text = deadline.NoiDung;
            datePicker.Date = deadline.ThoiGian;
            CotMaLop.Text = deadline.MaLop;
            CotHoanThanh.Text = deadline.HoanThanh == "false" ? "Chưa hoàn thành" : "Hoàn thành";

            AddNoiDung.Focus();
        }

        private async void Del_Clicked(object sender, EventArgs e)
        {
            Deadline deadline = _dl;
            try
            {
                HttpClient httpClient = new HttpClient();
                bool ans = await DisplayAlert("Cảnh báo", "Bạn có chắc chắn muốn xóa deadline này không?", "Có", "Không");
                if (ans)
                {
                    HttpResponseMessage kq;
                    kq = await httpClient.DeleteAsync("http://www.lno-ie307.somee.com/api/Homework?ID=" + deadline.ID.ToString());
                    var kqdl = await kq.Content.ReadAsStringAsync();
                    if (int.Parse(kqdl.ToString()) > 0)
                    {
                        await DisplayAlert("Thông báo", "Đã xóa deadline thành công!", "OK");
                    }
                    else
                        await DisplayAlert("Thông báo", "Không thể xóa!\tVui lòng thử lại", "OK");
                }
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"\tERROR {0}", ex.Message);
            }
           
        }


        private void Update_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PageAdDeadline(_dl));
        }
    }
}