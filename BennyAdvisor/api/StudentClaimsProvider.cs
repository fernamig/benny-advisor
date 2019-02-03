using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BennyAdvisor.api
{
    public class StudentClaimsProvider
    {
        readonly CollectionProvider<string> Provider;

        public StudentClaimsProvider()
        {
            Provider = new CollectionProvider<string>("student-claims");
        }

        public IEnumerable<string> HasClaims(string studentId, string claims)
        {
            var matchingClaims = Enumerable.Empty<string>();
            var studentClaims = Provider.Get(studentId);
            if (studentClaims != null)
                matchingClaims = studentClaims.Intersect(claims.Split(','));
            return matchingClaims;
        }
    }
}
