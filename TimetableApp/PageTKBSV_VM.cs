using System;
using System.Collections.Generic;
using System.Text;
using TimetableApp.Class;
using Xamarin.Plugin.Calendar.Models;

namespace TimetableApp
{
    internal class PageTKBSV_VM
    {
        public EventCollection Events { get; set; }

        public PageTKBSV_VM()
        {
            Events = new EventCollection
            {
                //[DateTime.Now] = new List<LopHoc>
                //{
                //    new LopHoc { Name = "Cool event1", Description = "This is Cool event1's description!" },
                //    new LopHoc { Name = "Cool event2", Description = "This is Cool event2's description!" }
                //},
            };
        }
    }
}
