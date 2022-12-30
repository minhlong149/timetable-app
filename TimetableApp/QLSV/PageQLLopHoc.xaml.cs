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
        }

        private async void PopulatingPicker()
        {
            try
            {
                string uri = $"http://lno-ie307.somee.com/api/LopHoc";
                List<LopHoc> classes = new List<LopHoc>();

                HttpResponseMessage response = await client.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    classes = JsonConvert.DeserializeObject<List<LopHoc>>(content);
                }

                pckClasses.ItemsSource = classes;
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"\tERROR {0}", ex.Message);
                pckClasses.ItemsSource = new List<LopHoc>();
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
    }
}