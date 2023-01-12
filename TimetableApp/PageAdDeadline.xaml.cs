using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using TimetableApp.Class;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TimetableApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageAdDeadline : ContentPage
    {
        Deadline _dl;
        public PageAdDeadline()
        {
            InitializeComponent();
            GetMaLop();
            AddTieuDe.Focus();
            AddNoiDung.Focus();
        }

        public async void GetMaLop()
        {
            HttpClient httpClient = new HttpClient();
            var lstMaLop = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/LopHoc?MaSV=" + SinhVien.DangNhap.MaSV.ToString());
            var lstMaLopConverted = JsonConvert.DeserializeObject<List<Deadline>>(lstMaLop);
            picker.ItemsSource = lstMaLopConverted;
        }

        public PageAdDeadline(Deadline deadline)
        {
            InitializeComponent();
            Title = "Sửa Deadline";
            _dl = deadline;
            picker.SelectedItem = deadline.MaLop;//Cần chỉnh sửa
            AddTieuDe.Text = deadline.TieuDe.ToString();
            AddNoiDung.Text = deadline.NoiDung.ToString();
            datePicker.Date = deadline.ThoiGian;
            AddNoiDung.Focus();
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            if (_dl != null)
            {
                _dl.MaSV = SinhVien.DangNhap.MaSV.ToString();
                _dl.MaLop = picker.SelectedItem.ToString();// Bug
                _dl.TieuDe = AddTieuDe.Text;
                _dl.NoiDung = AddNoiDung.Text;
                _dl.ThoiGian = datePicker.Date;

                HttpClient httpClient = new HttpClient();
                string json = JsonConvert.SerializeObject(_dl);
                StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage kq;

                kq = await httpClient.PutAsync("http://www.lno-ie307.somee.com/api/Homework?ID=" + _dl.ID, stringContent);
                var kqthem = await kq.Content.ReadAsStringAsync();

                if (int.Parse(kqthem.ToString()) > 0)
                {
                    await DisplayAlert("Thông báo", "Sửa deadline thành công", "OK");
                }
                else
                    await DisplayAlert("Thông báo", "Sửa deadline không thành công", "Thử lại");
            }
            else
            {
                Deadline _deadline = new Deadline();
                _deadline.MaSV = SinhVien.DangNhap.MaSV.ToString();
                _deadline.MaLop = picker.SelectedItem.ToString();
                _deadline.NoiDung = AddNoiDung.Text;
                _deadline.ThoiGian = datePicker.Date;
                _deadline.TieuDe = AddTieuDe.Text;

                try
                {
                    HttpClient httpClient = new HttpClient();
                    string json = JsonConvert.SerializeObject(_deadline);
                    StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage kq;

                    kq = await httpClient.PostAsync("http://www.lno-ie307.somee.com/api/Homework?MaSV=" + SinhVien.DangNhap.MaSV.ToString(), stringContent);
                    var kqthem = await kq.Content.ReadAsStringAsync();
                    if (int.Parse(kqthem.ToString()) > 0)
                    {
                        await DisplayAlert("Thông báo", "Thêm deadline thành công", "OK");
                    }
                    else
                        await DisplayAlert("Thông báo", "Thêm deadline thất bại", "Thử lại");
                    await Navigation.PopAsync();
                }
                catch(Exception ex)
                {
                    Console.WriteLine(@"\tERROR {0}", ex.Message);
                }
            }
        }
    }
}