using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        List<LopHoc> classList;
        ObservableCollection<SinhVien> studentList;

        public PageThemSV(LopHoc _lopHoc = null)
        {
            InitializeComponent();
            client = new HttpClient();

            updateSubjectPicker();
            if (_lopHoc != null)
            {
                pckSubjects.SelectedIndex = MonHoc.DanhSach.FindIndex(monHoc => monHoc.TenMon == _lopHoc.TenMon);
                filterClassListBySubject();
                pckClasses.SelectedIndex = classList.FindIndex(lopHoc => lopHoc.MaLop == _lopHoc.MaLop);
            }
        }

        private void updateSubjectPicker()
        {
            pckSubjects.ItemsSource = null;
            pckSubjects.ItemsSource = MonHoc.DanhSach;
        }

        private void filterClassListBySubject()
        {
            int selectedIndex = pckSubjects.SelectedIndex;
            if (selectedIndex != -1)
            {
                MonHoc selectedSubject = (MonHoc)pckSubjects.SelectedItem;
                classList = LopHoc.DanhSach.FindAll(lopHoc => lopHoc.TenMon == selectedSubject.TenMon);

                pckClasses.ItemsSource = null;
                pckClasses.ItemsSource = classList;
            }
        }

        private void pckSubjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            filterClassListBySubject();
            lstStudents.ItemsSource = null;
            txtClassName.IsVisible = false;
            sbSinhVien.IsVisible = false;
        }

        private async void pckClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            await updateStudentList();
        }

        private async Task updateStudentList()
        {
            int selectedIndex = pckClasses.SelectedIndex;
            if (selectedIndex != -1)
            {
                LopHoc selectedClass = (LopHoc)pckClasses.SelectedItem;
                studentList = await SelectStudentCanRegisterClass(selectedClass.MaLop);
                updateListView(studentList);
                sbSinhVien.IsVisible = studentList.Any();
            }
        }

        private void updateListView(IEnumerable<SinhVien> studentList)
        {
            lstStudents.ItemsSource = studentList;
            txtClassName.IsVisible = !studentList.Any();
        }

        private async Task<ObservableCollection<SinhVien>> SelectStudentCanRegisterClass(string MaLop)
        {
            try
            {
                string uri = $"http://lno-ie307.somee.com/api/SinhVien?MaLop={MaLop}&ChuaDangKy=true";
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

                    if (content == "1")
                    {
                        string alertTitle = content == "1" ? "Thành công" : "Thất bại";
                        string alertDesc = content == "1" ? $"Đã thêm {content} sinh viên vào lớp {MaLop}" : $"Không thể thêm sinh viên {MaSV} vào lớp {MaLop}";
                        string alertApcept = content == "1" ? "Tiếp tục" : "Thử lại";
                        string alertCancel = "Thoát";

                        bool continueToAdd = await DisplayAlert(alertTitle, alertDesc, alertApcept, alertCancel);
                        if (!continueToAdd)
                        {
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            await updateStudentList();
                        }
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
            string alertDesc = $"Đã có lỗi xảy ra khi thêm sinh viên {MaSV} vào lớp {MaLop}";
            string alertApcept = "Thử lại";
            string alertCancel = "Thoát";

            bool continueToAdd = await DisplayAlert(alertTitle, alertDesc, alertApcept, alertCancel);
            if (!continueToAdd)
            {
                await Navigation.PopAsync();
            }
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