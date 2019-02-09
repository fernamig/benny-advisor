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
    }
}
