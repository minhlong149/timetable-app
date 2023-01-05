using System;
using System.Collections.Generic;
using System.Text;
using TimetableApp.Class;
using Xamarin.Plugin.Calendar.Models;

namespace TimetableApp
{
    public class PageTKBSV_VM
    {
        public EventCollection Events { get; set; }

        public PageTKBSV_VM()
        {
            Events = new EventCollection
            {
                [DateTime.Now] = new List<LopHoc> {
                    new LopHoc { MaLop = "ie101" }
                },
                [DateTime.Now.AddDays(2)] = new List<LopHoc>{
                    new LopHoc { MaLop = "ie102" },
                    new LopHoc { MaLop = "ie103" },
                }
            };
        }
    }
}
