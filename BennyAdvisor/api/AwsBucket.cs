using System;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace BennyAdvisor.api
{
    public class AwsBucket
    {
        const string BucketName = "osu-mock-api";

        readonly string Root;
        readonly JsonSerializerSettings SerializerSettings;

        public AwsBucket(string root)
        {
            Root = root;
            SerializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
        }

        public async Task<bool> ExistsAsync(string keyName)
        {
            try
            {
                using (var client = new AmazonS3Client(RegionEndpoint.USWest2))
                {
                    var request = new GetObjectMetadataRequest
                    {
                        BucketName = BucketName,
                        Key = $"{Root}/{keyName}"
                    };
                    await client.GetObjectMetadataAsync(request);
                    return true;
                }
            }
            catch (AmazonS3Exception ex) when ((ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                || (ex.StatusCode == System.Net.HttpStatusCode.Forbidden))
            {
                return false;
            }
        }
        public bool Exists(string keyName)
        {
            return ExistsAsync(keyName).Result;
        }

        public async Task<string> ReadAllTextAsync(string keyName)
        {
            using (var client = new AmazonS3Client(RegionEndpoint.USWest2))
            {
                var request = new GetObjectRequest
                {
                    BucketName = BucketName,
                    Key = $"{Root}/{keyName}"
                };
                using (var response = await client.GetObjectAsync(request))
                using (var responseStream = response.ResponseStream)
                using (var reader = new StreamReader(responseStream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        public async Task<string> TryReadAllTextAsync(string keyName)
        {
            try
            {
                return await ReadAllTextAsync(keyName);
            }
            catch
            {
                return null;
            }
        }
        public string ReadAllText(string keyName)
        {
            return ReadAllTextAsync(keyName).Result;
        }
        public string TryReadAllText(string keyName)
        {
            try
            {
                return ReadAllText(keyName);
            }
            catch
            {
                return null;
            }
        }

        public async Task<T> ReadObjectAsync<T>(string keyName)
        {
            return JObject.Parse(await ReadAllTextAsync(keyName)).ToObject<T>();
        }
        public T ReadObject<T>(string keyName)
        {
            return JObject.Parse(ReadAllText(keyName)).ToObject<T>();
        }

        public async Task<T> TryReadObjecAsync<T>(string keyName) where T : class
        {
            try
            {
                return await ReadObjectAsync<T>(keyName);
            }
            catch
            {
                return null;
            }
        }
        public T TryReadObject<T>(string keyName) where T : class
        {
            try
            {
                return ReadObject<T>(keyName);
            }
            catch
            {
                return null;
            }
        }

        public T[] ReadArray<T>(string keyName)
        {
            return JArray.Parse(ReadAllText(keyName)).ToObject<T[]>();
        }

        public async Task WriteAllTextAsync(string keyName, string body)
        {
            using (var client = new AmazonS3Client(RegionEndpoint.USWest2))
            {
                var request = new PutObjectRequest
                {
                    BucketName = BucketName,
                    Key = $"{Root}/{keyName}",
                    ContentBody = body
                };

                await client.PutObjectAsync(request);
            }
        }
        public void WriteAllText(string keyName, string body)
        {
            WriteAllTextAsync(keyName, body).Wait();
        }

        public async Task WriteObjectAsync<T>(string keyName, T obj)
        {
            await WriteAllTextAsync(keyName, JsonConvert.SerializeObject(obj, SerializerSettings));
        }
        public void WriteObject<T>(string keyName, T obj)
        {
            WriteAllText(keyName, JsonConvert.SerializeObject(obj, SerializerSettings));
        }

        public async Task DeleteFileAsync(string keyName)
        {
            using (var client = new AmazonS3Client(RegionEndpoint.USWest2))
            {
                var request = new DeleteObjectRequest
                {
                    BucketName = BucketName,
                    Key = $"{Root}/{keyName}"
                };
                await client.DeleteObjectAsync(request);
            }
        }
        public void DeleteFile(string keyName)
        {
            DeleteFileAsync(keyName).Wait();
        }
        public async Task TryDeleteFileAsync(string keyName)
        {
            try
            {
                await DeleteFileAsync(keyName);
            }
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
            {
            }
        }
        public void TryDeleteFile(string keyName)
        {
            try
            {
                DeleteFile(keyName);
            }
#pragma warning disable RECS0022 // A catch clause that catches System.Exception and has an empty body
            catch
#pragma warning restore RECS0022 // A catch clause that catches System.Exception and has an empty body
            {
            }
        }
    }
}
