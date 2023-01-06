using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TimetableApp.Class;
using Xamarin.Plugin.Calendar.Models;

namespace TimetableApp
{
    public class PageTKBSV_VM
    {
        public EventCollection Events { get; set; }
        HttpClient client;

        public PageTKBSV_VM()
        {
            client = new HttpClient();
            Events = new EventCollection { };
            createEvent();
        }

        private async void createEvent()
        {
            List<LopHoc> studentClasses = await GetClassList();
            foreach (LopHoc studentClass in studentClasses)
            {
                // TODO: update start and end date of each class from dB
                DateTime startDate = DateTime.Now.AddDays(-10);
                DateTime endDate = DateTime.Now.AddDays(+10);

                // Find the weekdate that the class actualy start
                DateTime classDate = startDate;
                while ((int)classDate.DayOfWeek != int.Parse(studentClass.Thu) - 1)
                {
                    classDate = classDate.AddDays(1);
                }

                // Loop through every week to add the class to the date
                for (; classDate < endDate; classDate = classDate.AddDays(7))
                {
                    Events.Add(classDate, new List<LopHoc> { studentClass });

                    // TODO: handle multiple class a day
                    //if (!Events.ContainsKey(classStart))
                    //{
                    //    Events.Add(classStart, new List<LopHoc> { studentClass });
                    //}
                    //else
                    //{
                    //    List<LopHoc> icollection = (List<LopHoc>)Events[classStart];
                    //    icollection.Add(studentClass);
                    //    Events[classStart] = icollection;
                    //}
                }
            }
        }

        private async Task<List<LopHoc>> GetClassList()
        {
            try
            {
                string MaSV = SinhVien.DangNhap.MaSV;
                string uri = $"http://lno-ie307.somee.com/api/LopHoc?MaSV={MaSV}";
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
    }
}
