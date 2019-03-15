using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using BennyAdvisor.Models;

namespace BennyAdvisor.api
{
    public class NotesProvider
    {
        readonly CollectionProvider<NoteModel> Provider;

        public NotesProvider()
        {
            Provider = new CollectionProvider<NoteModel>("notes");
        }

        public IEnumerable<NoteModel> Get(string id)
        {
            return Provider.Get(id).OrderByDescending(x => x.Date);
        }

        public void Add(string id, NoteModel note)
        {
            note.Date = DateTime.UtcNow;
            Provider.Set(id, Provider.Get(id).Concat(new[] { note }));
        }
    }
    public class Notes2Provider
    {
        readonly CollectionProvider<NoteModel> Provider;
        readonly string AccessToken;

        public Notes2Provider()
        {
            Provider = new CollectionProvider<NoteModel>("notes");
            AccessToken = GetAccessToken().Result;
        }

        public async Task<IEnumerable<Note2Model>> Get(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://oregonstateuniversity-test.apigee.net");

                // Add the Authorization header with the AccessToken.
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken);

                var response = await client.GetAsync($"/v1/notes?studentId={id}");
                var json = JObject.Parse(await response.Content.ReadAsStringAsync());
                return json["data"].ToAsyncEnumerableValue<Note2Model[]>().OrderByDescending(x => x.DateCreated);
            }
        }

        public void Add(string id, NoteModel note)
        {
            note.Date = DateTime.UtcNow;
            Provider.Set(id, Provider.Get(id).Concat(new[] { note }));
        }

        async Task<string> GetAccessToken()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.oregonstate.edu");

                // Build up the data to POST.
                List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                postData.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
                postData.Add(new KeyValuePair<string, string>("client_id", ""));
                postData.Add(new KeyValuePair<string, string>("client_secret", ""));

                FormUrlEncodedContent content = new FormUrlEncodedContent(postData);

                // Post to the Server and parse the response.
                var response = await client.PostAsync("/oauth2/token", content);
                var json = JObject.Parse(await response.Content.ReadAsStringAsync());

                return json["access_token"].Value<string>();
            }
        }
    }
}
