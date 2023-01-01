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
        List<MonHoc> subjectList;
        List<LopHoc> classList;

        public PageQLLopHoc()
        {
            InitializeComponent();
            client = new HttpClient();
            //subjectList = new List<MonHoc>();
            //classList = new List<LopHoc>();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            PopulatingPicker();
        }

        private async void PopulatingPicker()
        {
            subjectList = await GetSubjectList();
            pckSubjects.ItemsSource = subjectList;

            classList = await GetClassList();
            pckClasses.ItemsSource = null;
        }

        private void pckSubjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            if (selectedIndex != -1)
            {
                MonHoc selectedSubject = (MonHoc)picker.SelectedItem;
                pckClasses.ItemsSource = null;
                pckClasses.ItemsSource = classList.FindAll(lopHoc => lopHoc.TenMon == selectedSubject.TenMon);
                Console.WriteLine(selectedSubject.MaMon);
            }
        }

        private async void pckClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

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

        private async Task<List<LopHoc>> GetClassList()
        {
            try
            {
                string uri = $"http://lno-ie307.somee.com/api/LopHoc";
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<LopHoc>>(content);
                }
                return new List<LopHoc>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"\tERROR {0}", ex.Message);
                return new List<LopHoc>();
            }
        }

        private async Task<List<MonHoc>> GetSubjectList()
        {
            try
            {
                string uri = $"http://lno-ie307.somee.com/api/MonHoc";
                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<MonHoc>>(content);
                }
                return new List<MonHoc>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"\tERROR {0}", ex.Message);
                return new List<MonHoc>();
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            LopHoc selectedClass = (LopHoc)pckClasses.SelectedItem;
            Navigation.PushAsync(selectedClass is null ? new PageThemSV() : new PageThemSV(selectedClass.MaLop));
        }
    }
}