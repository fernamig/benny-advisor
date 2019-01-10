using System;
using System.Threading.Tasks;
using BennyAdvisor.Models;

namespace BennyAdvisor.api
{
    public class ObjectProvider<T> : ROObjectProvider<T> where T : class
    {
        public ObjectProvider(string root)
            : base(root)
        {
        }

        public async Task SetAsync(string id, T obj)
        {
            await Bucket.WriteObjectAsync<T>($"{id}.json", obj);
        }
        public void Set(string id, T obj)
        {
            Bucket.WriteObject<T>($"{id}.json", obj);
        }

        public async Task DeleteAsync(string id)
        {
            await Bucket.DeleteFileAsync($"{id}.json");
        }
        public void Delete(string id)
        {
            Bucket.DeleteFile($"{id}.json");
        }
    }
}
