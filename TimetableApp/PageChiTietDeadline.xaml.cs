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
        Deadline _dl;
        public PageChiTietDeadline(Deadline deadline)
        {

            InitializeComponent();
            Title = "Chi tiết";
            _dl = deadline;
            AddTieuDe.Text = deadline.TieuDe;
            AddNoiDung.Text = deadline.NoiDung;
            datePicker.Date = deadline.ThoiGian;
            AddNoiDung.Focus();
        }

        private async void Del_Clicked(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            Deadline deadline = (Deadline)bt.BindingContext;
            HttpClient httpClient = new HttpClient();
            bool ans = await DisplayAlert("Cảnh báo", "Bạn có chắc chắn muốn xóa deadline " + deadline.TieuDe + " ?", "Có", "Không");
            if (ans)
            {
                HttpResponseMessage kq;
                kq = await httpClient.DeleteAsync("http://www.lno-ie307.somee.com/api/api/Homework?ID=" + deadline.ID.ToString());
                var kqdk = await kq.Content.ReadAsStringAsync();
                if (int.Parse(kqdk.ToString()) > 0)
                {
                    await DisplayAlert("Thông báo", "Đã xóa lớp " + deadline.TieuDe + " thành công!", "OK");

                }
                else
                    await DisplayAlert("Thông báo", "Đã có lỗi xảy ra!\tVui lòng thử lại", "OK");
            }
            await Navigation.PushAsync(new PageDeadline());
        }

        private void Update_Clicked(object sender, EventArgs e)
        {
            Button bt = (Button)sender;
            Deadline deadline = (Deadline)bt.BindingContext;
            Navigation.PushAsync(new PageAdDeadline(deadline));
        }
    }
}