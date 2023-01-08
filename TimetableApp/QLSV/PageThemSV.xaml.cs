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
        List<LopHoc> classList;
 
        public PageThemSV(string MaLop = null)
        {
            InitializeComponent();
            client = new HttpClient();

            updatClassPicker();
            if (MaLop != null)
            {
                pckClasses.SelectedIndex = classList.FindIndex(lopHoc => lopHoc.MaLop == MaLop);
            }
        }

        private void updatClassPicker()
        {
            classList = LopHoc.DanhSach.OrderBy(lopHoc => lopHoc.MaLop).ToList();
            pckClasses.ItemsSource = null;
            pckClasses.ItemsSource = classList;
        }

        private async void pckClasses_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            // Update listview if class has already been selected
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

    }
}