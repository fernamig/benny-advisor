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

        public MyProfileSchedulerModel GetSchedule(string id)
        {
            var my = TryGet(id) ?? new MyProfileModel();
            return my.Scheduler;
        }
        public void SetSchedule(string id, MyProfileSchedulerModel schedule)
        {
            var my = TryGet(id) ?? new MyProfileModel();
            my.Scheduler = schedule;
            Set(id, my);
        }
    }
}
