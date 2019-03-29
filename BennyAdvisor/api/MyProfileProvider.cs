using System;
using BennyAdvisor.Models;

namespace BennyAdvisor.api
{
    public class MyProfileProvider : ObjectProvider<MyProfileModel>
    {
        public MyProfileProvider()
            : base("my-profile")
        {
        }

        public ScheduleSettingsModel GetSchedule(string id)
        {
            var my = TryGet(id) ?? new MyProfileModel();
            return my.Schedule;
        }
        public void SetSchedule(string id, ScheduleSettingsModel schedule)
        {
            var my = TryGet(id) ?? new MyProfileModel();
            my.Schedule = schedule;
            Set(id, my);
        }
    }
}
