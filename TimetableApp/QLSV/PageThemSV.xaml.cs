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

namespace TimetableApp.QLSV
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageThemSV : ContentPage
    {
        HttpClient client;
        List<MonHoc> subjectList;
        List<LopHoc> classList;

        public PageThemSV(LopHoc _lopHoc = null)
        {
            InitializeComponent();
            client = new HttpClient();

            updateSubjectPicker();
            if (_lopHoc != null)
            {
                pckSubjects.SelectedIndex = subjectList.FindIndex(monHoc => monHoc.TenMon == _lopHoc.TenMon);

                updatClassPicker();
                pckClasses.SelectedIndex = classList.FindIndex(lopHoc => lopHoc.MaLop == _lopHoc.MaLop);
            }
        }

        private void updateSubjectPicker()
        {
            subjectList = MonHoc.DanhSach.OrderBy(monHoc => monHoc.TenMon).ToList();
            pckSubjects.ItemsSource = null;
            pckSubjects.ItemsSource = subjectList;
        }

        private void updatClassPicker()
        {
            filterClassListBySubject(pckSubjects);
        }

        private void filterClassListBySubject(Picker picker)
        {
            int selectedIndex = picker.SelectedIndex;
            if (selectedIndex != -1)
            {
                MonHoc selectedSubject = (MonHoc)picker.SelectedItem;
                classList = LopHoc.DanhSach.FindAll(lopHoc => lopHoc.TenMon == selectedSubject.TenMon).OrderBy(lopHoc => lopHoc.MaLop).ToList();

                pckClasses.ItemsSource = null;
                pckClasses.ItemsSource = classList;
            }
        }

        private void pckSubjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;
            filterClassListBySubject(picker);
        }

        private async void pckClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            if (selectedIndex != -1)
            {
                LopHoc selectedClass = (LopHoc)picker.SelectedItem;
                lstStudents.ItemsSource = null;
                lstStudents.ItemsSource = await SelectStudentCanRegisterClass(selectedClass.MaLop);
            }
        }

        private async Task<List<SinhVien>> SelectStudentCanRegisterClass(string MaLop)
        {
            try
            {
                string uri = $"http://lno-ie307.somee.com/api/SinhVien?MaLop={MaLop}&ChuaDangKy=true";
                List<SinhVien> students = new List<SinhVien>();

                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    students = JsonConvert.DeserializeObject<List<SinhVien>>(content);
                }
                return students;
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"\tERROR {0}", ex.Message);
                return new List<SinhVien>();
            }
        }

        private async void lstStudents_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            SinhVien sinhVien = (SinhVien)e.SelectedItem;
            LopHoc lopHoc = (LopHoc)pckClasses.SelectedItem;
            bool isAdded = await DisplayAlert("Xác nhận", $"Thêm {sinhVien.TenSV} vào lớp {lopHoc.MaLop}?", "Thêm", "Huỷ");
            if (isAdded)
            {
                await InsertStudentClass(sinhVien.MaSV, lopHoc.MaLop);
            }
        }

        private async Task InsertStudentClass(string MaSV, string MaLop)
        {
            try
            {
                string uri = $"http://lno-ie307.somee.com/api/SinhVien?MaSV={MaSV}&MaLop={MaLop}";
                HttpResponseMessage response = await client.PostAsync(uri, null);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    
                    string alertTitle = content == "1" ? "Thành công" : "Thất bại";
                    string alertDesc = content == "1" ? $"Đã thêm {content} sinh viên vào lớp {MaLop}" : $"Không thể thêm sinh viên {MaSV} vào lớp {MaLop}";
                    string alertApcept = content == "1" ? "Tiếp tục" : "Thử lại";
                    string alertCancel = "Thoát";

                    bool continueToAdd = await DisplayAlert(alertTitle, alertDesc, alertApcept, alertCancel);
                    if (!continueToAdd)
                    {
                        await Navigation.PopAsync();
                    }
                }
                else
                {
                    await resolveError(MaSV, MaLop);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"\tERROR {0}", ex.Message);
                await resolveError(MaSV, MaLop);
            }
        }

        private async Task resolveError(string MaSV, string MaLop)
        {
            string alertTitle = "Lỗi";
            string alertDesc = $"Đã có lỗi xảy ra khi thêm sinh viên {MaSV} vào lớp {MaLop}";
            string alertApcept = "Thử lại";
            string alertCancel = "Thoát";

            bool continueToAdd = await DisplayAlert(alertTitle, alertDesc, alertApcept, alertCancel);
            if (!continueToAdd)
            {
                await Navigation.PopAsync();
            }
        }
    }
}