using System;
using Xunit;
using BennyAdvisor.api;
using BennyAdvisor.Models;

namespace UnitTests
{
    public class AwsBucketTests : GroupBase
    {
        readonly AwsBucket Bucket = new AwsBucket("test");

        [Fact]
        public void VerifyExistsFailureDoesNotThrow()
        {
            Assert.False(Bucket.ExistsAsync("dummy").Result);
            Assert.False(Bucket.Exists("dummy"));
        }

        [Fact]
        public void VerifyExistsSuccess()
        {
            Assert.True(Bucket.ExistsAsync("test.txt").Result);
            Assert.True(Bucket.Exists("test.txt"));
        }

        [Fact]
        public void VerifyDeleteFileNonExistingFile()
        {
            Assert.False(Bucket.Exists("dummy"));
            Bucket.DeleteFileAsync("dummy").Wait();

            Assert.False(Bucket.Exists("dummy"));
            Bucket.DeleteFile("dummy");

            Assert.False(Bucket.Exists("dummy"));
            Bucket.TryDeleteFileAsync("dummy").Wait();

            Assert.False(Bucket.Exists("dummy"));
            Bucket.TryDeleteFile("dummy");
            Assert.False(Bucket.Exists("dummy"));
        }

        [Fact]
        public void VerifyDeleteFileExistingFile()
        {
            const string sample = "This is sample text to delete.";
            const string keyName = "unit.text.delete.txt";

            Bucket.WriteAllText(keyName, sample);
            Assert.True(Bucket.Exists(keyName));
            Bucket.DeleteFileAsync(keyName).Wait();
            Assert.False(Bucket.Exists(keyName));

            Bucket.WriteAllText(keyName, sample);
            Assert.True(Bucket.Exists(keyName));
            Bucket.DeleteFile(keyName);
            Assert.False(Bucket.Exists(keyName));

            Bucket.WriteAllText(keyName, sample);
            Assert.True(Bucket.Exists(keyName));
            Bucket.TryDeleteFileAsync(keyName).Wait();
            Assert.False(Bucket.Exists(keyName));

            Bucket.WriteAllText(keyName, sample);
            Assert.True(Bucket.Exists(keyName));
            Bucket.TryDeleteFile(keyName);
            Assert.False(Bucket.Exists(keyName));
        }

        [Fact]
        public void VerifyReadAllText()
        {
            Assert.Equal("This is me", Bucket.ReadAllTextAsync("test.txt").Result);
            Assert.Equal("This is me", Bucket.ReadAllText("test.txt"));
            Assert.Equal("This is me", Bucket.TryReadAllTextAsync("test.txt").Result);
            Assert.Null(Bucket.TryReadAllTextAsync("dummy").Result);
            Assert.Equal("This is me", Bucket.TryReadAllText("test.txt"));
            Assert.Null(Bucket.TryReadAllText("dummy"));
        }

        [Fact]
        public void VerifyReadObject()
        {
            VerifyEqual(Group, Bucket.ReadObjectAsync<GroupCollectionModel>("test.group.json").Result);
            VerifyEqual(Group, Bucket.ReadObject<GroupCollectionModel>("test.group.json"));
            VerifyEqual(Group, Bucket.TryReadObject<GroupCollectionModel>("test.group.json"));
            Assert.Null(Bucket.TryReadObject<GroupCollectionModel>("dummy"));
        }

        [Fact]
        public void VerifyWriteAllText()
        {
            const string sample = "This is sample text.";
            const string keyName = "unit.text.txt";

            Bucket.DeleteFile(keyName);
            Assert.False(Bucket.Exists(keyName));
            Bucket.WriteAllTextAsync(keyName, sample).Wait();
            Assert.Equal(sample, Bucket.ReadAllText(keyName));

            Bucket.DeleteFile(keyName);
            Assert.False(Bucket.Exists(keyName));
            Bucket.WriteAllText(keyName, sample);
            Assert.Equal(sample, Bucket.ReadAllText(keyName));
        }

        [Fact]
        public void VerifyWriteObject()
        {
            const string keyName = "unit.text.obj.txt";

            Bucket.DeleteFile(keyName);
            Assert.False(Bucket.Exists(keyName));
            Bucket.WriteObjectAsync(keyName, Group).Wait();
            VerifyEqual(Group, Bucket.ReadObject<GroupCollectionModel>(keyName));

            Bucket.DeleteFile(keyName);
            Assert.False(Bucket.Exists(keyName));
            Bucket.WriteObject(keyName, Group);
            VerifyEqual(Group, Bucket.ReadObject<GroupCollectionModel>(keyName));
        }
    }
}
