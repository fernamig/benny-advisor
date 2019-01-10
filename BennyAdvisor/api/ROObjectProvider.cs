using System;
using System.Threading.Tasks;
using BennyAdvisor.Models;

namespace BennyAdvisor.api
{
    public class ROObjectProvider<T> where T : class
    {
        protected AwsBucket Bucket { get; set; }

        public ROObjectProvider(string root)
        {
            Bucket = new AwsBucket(root);
        }

        public async Task<T> GetAsync(string id)
        {
            return await Bucket.ReadObjectAsync<T>($"{id}.json");
        }
        public T Get(string id)
        {
            return Bucket.ReadObject<T>($"{id}.json");
        }

        public async Task<T> TryGetAsync(string id)
        {
            return await Bucket.TryReadObjecAsync<T>($"{id}.json");
        }
        public T TryGet(string id)
        {
            return Bucket.TryReadObject<T>($"{id}.json");
        }
    }
}
