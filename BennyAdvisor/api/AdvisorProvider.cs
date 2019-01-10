using System;
using System.Collections.Generic;
using System.Linq;
using BennyAdvisor.Models;

namespace BennyAdvisor.api
{
    public class AdvisorProvider : CollectionProvider
    {
        public AdvisorProvider()
            : base("advisor")
        {
        }
    }
}
