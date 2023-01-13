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
using System.Collections.ObjectModel;

namespace TimetableApp.QLSV
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageQLLopHoc : ContentPage
    {
        HttpClient client;
        ObservableCollection<SinhVien> studentList;

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
            pckSubjects.ItemsSource = null;
            pckSubjects.ItemsSource = MonHoc.DanhSach;
            pckSubjects.SelectedIndex = currentSubjectIndex;
        }

        private void updatClassPicker(int currentClassIndex = -1)
        {
            filterClassPicker();
            pckClasses.SelectedIndex = currentClassIndex;
        }

        private void pckSubjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            filterClassPicker();
            lstStudents.ItemsSource = null;
            txtClassName.IsVisible = false;
            sbSinhVien.IsVisible = false;
        }

        private void filterClassPicker()
        {
            // Only update class picker if a subject has been selected
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
                studentList = await GetStudentByClass(selectedClass.MaLop);
                updateListView(studentList);
                sbSinhVien.IsVisible = studentList.Any();
            }
        }

        private void updateListView(IEnumerable<SinhVien> studentList)
        {
            lstStudents.ItemsSource = studentList;
            txtClassName.IsVisible = !studentList.Any();
        }

        private async Task<ObservableCollection<SinhVien>> GetStudentByClass(string MaLop)
        {
            try
            {
                string uri = $"http://lno-ie307.somee.com/api/SinhVien?MaLop={MaLop}";
                ObservableCollection<SinhVien> students = new ObservableCollection<SinhVien>();

                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    students = JsonConvert.DeserializeObject<ObservableCollection<SinhVien>>(content);
                }

                return students;
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"\tERROR {0}", ex.Message);
                return new ObservableCollection<SinhVien>();
            }
        }

        private async void DeleteStudent(object sender, EventArgs e)
        {
            ImageButton imgBtn = (ImageButton)sender;
            SinhVien sinhVien = (SinhVien)imgBtn.CommandParameter;
            LopHoc lopHoc = (LopHoc)pckClasses.SelectedItem;

            bool isDeleted = await DisplayAlert("Cảnh báo", $"Xoá sinh viên {sinhVien.TenSV} khỏi lớp {lopHoc.MaLop}?", "Xoá", "Huỷ");
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
                        string alertTitle = content == "1" ? "Thành công" : "Thất bại";
                        string alertDesc = content == "1" ? $"Đã xoá {content} sinh viên khỏi lớp {MaLop}" : $"Không thể xoá sinh viên {MaSV} khỏi lớp {MaLop}";
                        string alertApcept = content == "1" ? "Thoát" : "Thử lại";

                        await DisplayAlert(alertTitle, alertDesc, alertApcept);
                        await updateStudentList();
                    }
                    else
                    {
                        await resolveError(MaSV, MaLop);
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

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            string keyword = searchBar.Text.ToLower();
            IEnumerable<SinhVien> newList = studentList.Where(sinhVien => sinhVien.TenSV.ToLower().Contains(keyword));
            updateListView(newList);
        }
    }
}