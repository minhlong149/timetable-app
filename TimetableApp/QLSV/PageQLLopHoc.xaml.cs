using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using TimetableApp.Class;
using System.Net.Http;
using Newtonsoft.Json;

namespace TimetableApp.QLSV
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageQLLopHoc : ContentPage
    {
        HttpClient client;

        public PageQLLopHoc()
        {
            InitializeComponent();
            client = new HttpClient();

            PopulatingPicker();
            RefreshData();
        }

        public void RefreshData()
        {
            lstStudents.RefreshCommand = new Command(() =>
            {
                PopulatingPicker();
                lstStudents.IsRefreshing = false;
            });
        }

        private async void PopulatingPicker()
        {
            // Save the picker index before update item source
            int currentSubjectIndex = pckSubjects.SelectedIndex;
            int currentClassIndex = pckClasses.SelectedIndex;

            await GetSubjectList();
            await GetClassList();

            updateSubjectPicker(currentSubjectIndex);
            updatClassPicker(currentClassIndex);
            await updateStudentList();
        }

        private void updateSubjectPicker(int currentSubjectIndex = -1)
        {
            // Update subject picker's item source
            pckSubjects.ItemsSource = null;
            pckSubjects.ItemsSource = MonHoc.DanhSach;
            pckSubjects.SelectedIndex = currentSubjectIndex;
        }

        private void updatClassPicker(int currentClassIndex = -1)
        {
            // Update class pickersubject picker
            pckClasses.ItemsSource = null;

            // Only update class picker if a subject has been selected
            filterClassListBySubject();

            pckClasses.SelectedIndex = currentClassIndex;
        }

        private void pckSubjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            filterClassListBySubject();
        }

        private void filterClassListBySubject()
        {
            int selectedIndex = pckSubjects.SelectedIndex;
            if (selectedIndex != -1)
            {
                MonHoc selectedSubject = (MonHoc)pckSubjects.SelectedItem;
                pckClasses.ItemsSource = null;
                pckClasses.ItemsSource = LopHoc.DanhSach.FindAll(lopHoc => lopHoc.TenMon == selectedSubject.TenMon);
            }
        }

        private async void pckClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            await updateStudentList();
        }

        private async Task updateStudentList()
        {
            int selectedIndex = pckClasses.SelectedIndex;

            // Update listview if class has already been selected
            if (selectedIndex != -1)
            {
                LopHoc selectedClass = (LopHoc)pckClasses.SelectedItem;
                lstStudents.ItemsSource = null;
                lstStudents.ItemsSource = await GetStudentByClass(selectedClass.MaLop);
            }
        }

        private async Task<List<SinhVien>> GetStudentByClass(string MaLop)
        {
            try
            {
                string uri = $"http://lno-ie307.somee.com/api/SinhVien?MaLop={MaLop}";
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

        private async void DeleteStudent(object sender, EventArgs e)
        {
            ImageButton imgBtn = (ImageButton)sender;
            SinhVien sinhVien = (SinhVien)imgBtn.CommandParameter;
            LopHoc lopHoc = (LopHoc)pckClasses.SelectedItem;

            bool isDeleted = await DisplayAlert("Cảnh báo", $"Xoá sinh viên {sinhVien.MaSV} khỏi lớp {lopHoc.MaLop}?", "Xoá", "Huỷ");
            if (isDeleted)
            {
                await InsertStudentClass(sinhVien.MaSV, lopHoc.MaLop);
            }
        }

        private async Task InsertStudentClass(string MaSV, string MaLop)
        {
            try
            {
                string uri = $"http://lno-ie307.somee.com/api/SinhVien?MaSV={MaSV}&MaLop={MaLop}";
                HttpResponseMessage response = await client.DeleteAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (content == "1")
                    {
                        await updateStudentList();
                    }

                    string alertTitle = content == "1" ? "Thành công" : "Thất bại";
                    string alertDesc = content == "1" ? $"Đã xoá {content} sinh viên khỏi lớp {MaLop}" : $"Không thể xoá sinh viên {MaSV} khỏi lớp {MaLop}";
                    string alertApcept = content == "1" ? "Thoát" : "Thử lại";

                    await DisplayAlert(alertTitle, alertDesc, alertApcept);
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
            string alertDesc = $"Đã có lỗi xảy ra khi xoá sinh viên {MaSV} khỏi lớp {MaLop}";
            string alertApcept = "Thử lại";
            await DisplayAlert(alertTitle, alertDesc, alertApcept);
        }

        private async Task GetClassList()
        {
            try
            {
                string uri = $"http://lno-ie307.somee.com/api/LopHoc";
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    LopHoc.DanhSach = JsonConvert.DeserializeObject<List<LopHoc>>(content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"\tERROR {0}", ex.Message);
            }
        }

        private async Task GetSubjectList()
        {
            try
            {
                string uri = $"http://lno-ie307.somee.com/api/MonHoc";
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    MonHoc.DanhSach = JsonConvert.DeserializeObject<List<MonHoc>>(content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"\tERROR {0}", ex.Message);
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            LopHoc selectedClass = (LopHoc)pckClasses.SelectedItem;
            Navigation.PushAsync(selectedClass is null ? new PageThemSV() : new PageThemSV(selectedClass));
        }
    }
}