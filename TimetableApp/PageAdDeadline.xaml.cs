

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimetableApp.Class;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace TimetableApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageAdDeadline : ContentPage
    {
        Deadline _dl;
        List<LopHoc> lstMaLopConverted;
        public PageAdDeadline()
        {
            InitializeComponent();
            GetMaLop();
            AddTieuDe.Focus();
            AddNoiDung.Focus();
        }

        public async Task GetMaLop()
        {
            HttpClient httpClient = new HttpClient();
            var lstMaLop = await httpClient.GetStringAsync("http://www.lno-ie307.somee.com/api/LopHoc?MaSV=" + SinhVien.DangNhap.MaSV.ToString());
            lstMaLopConverted = JsonConvert.DeserializeObject<List<LopHoc>>(lstMaLop);
            picker.ItemsSource = lstMaLopConverted;
        }

        public PageAdDeadline(Deadline deadline)
        {
            InitializeComponent();
            assignClassPicker(deadline);
            Title = "Sửa Deadline";
            _dl = deadline;

            AddTieuDe.Text = deadline.TieuDe.ToString();
            AddNoiDung.Text = deadline.NoiDung.ToString();

            datePicker.Date = deadline.ThoiGian;
            AddNoiDung.Focus();
        }

        private async void assignClassPicker(Deadline deadline)
        {
            await GetMaLop();
            picker.SelectedIndex = lstMaLopConverted.FindIndex(lopHoc => lopHoc.MaLop == deadline.MaLop);
        }

        private async void Save_Clicked(object sender, EventArgs e)
        {
            if (_dl != null)
            {
                LopHoc selectedClass = (LopHoc)picker.SelectedItem;
                _dl.MaSV = SinhVien.DangNhap.MaSV.ToString();
                _dl.MaLop = selectedClass.MaLop;
                _dl.TieuDe = AddTieuDe.Text;
                _dl.NoiDung = AddNoiDung.Text;
                _dl.ThoiGian = datePicker.Date;

                try
                {
                    HttpClient httpClient = new HttpClient();
                    string json = JsonConvert.SerializeObject(_dl);
                    StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpResponseMessage kq;
                    // DAM BAO LAY VE DUOC MA LOP TRUOC KHI GOI API
                    Console.WriteLine(json);

                    kq = await httpClient.PutAsync("http://www.lno-ie307.somee.com/api/Homework?ID=" + _dl.ID, stringContent);
                    var kqthem = await kq.Content.ReadAsStringAsync();

                    if (int.Parse(kqthem.ToString()) > 0)
                    {
                        await DisplayAlert("Thông báo", "Sửa deadline thành công", "OK");
                    }
                    else
                        await DisplayAlert("Thông báo", "Sửa deadline không thành công", "Thử lại");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(@"\tERROR {0}", ex.Message);
                }
                await Navigation.PopAsync();
            }
            else
            {
                // CAN KIEM TRA XEM PICKER DA DUOC CHON HAY CHƯA
                if (picker.SelectedIndex != -1)
                {
                    LopHoc selectedClass = (LopHoc)picker.SelectedItem;
                    Deadline _deadline = new Deadline();
                    _deadline.MaSV = SinhVien.DangNhap.MaSV.ToString();
                    _deadline.MaLop = selectedClass.MaLop;
                    _deadline.NoiDung = AddNoiDung.Text;
                    _deadline.ThoiGian = datePicker.Date;
                    _deadline.TieuDe = AddTieuDe.Text;

                    try
                    {
                        HttpClient httpClient = new HttpClient();
                        string json = JsonConvert.SerializeObject(_deadline);
                        StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");
                        HttpResponseMessage kq;
                        // DAM BAO LAY VE DUOC MA LOP TRUOC KHI GOI API
                        Console.WriteLine(json);

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
                    catch (Exception ex)
                    {
                        Console.WriteLine(@"\tERROR {0}", ex.Message);
                    }
                }
                else await DisplayAlert("Thông báo", "Vui lòng chọn môn học", "OK");
            }
        }
    }
}