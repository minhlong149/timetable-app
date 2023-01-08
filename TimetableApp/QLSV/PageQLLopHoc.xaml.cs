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
            await updateStudentList(pckClasses);
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
            filterClassListBySubject(pckSubjects);

            pckClasses.SelectedIndex = currentClassIndex;
        }

        private void pckSubjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;
            filterClassListBySubject(picker);
        }

        private void filterClassListBySubject(Picker picker)
        {
            int selectedIndex = picker.SelectedIndex;
            if (selectedIndex != -1)
            {
                MonHoc selectedSubject = (MonHoc)picker.SelectedItem;
                pckClasses.ItemsSource = null;
                pckClasses.ItemsSource = LopHoc.DanhSach.FindAll(lopHoc => lopHoc.TenMon == selectedSubject.TenMon);
            }
        }

        private async void pckClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;
            await updateStudentList(picker);
        }

        private async Task updateStudentList(Picker picker)
        {
            int selectedIndex = picker.SelectedIndex;

            // Update listview if class has already been selected
            if (selectedIndex != -1)
            {
                LopHoc selectedClass = (LopHoc)picker.SelectedItem;
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

        private void DeleteStudent(object sender, EventArgs e)
        {
            ImageButton imgBtn = (ImageButton)sender;
            Console.WriteLine(imgBtn.CommandParameter);
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
            Navigation.PushAsync(selectedClass is null ? new PageThemSV() : new PageThemSV(selectedClass.MaLop));
        }
    }
}