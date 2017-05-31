using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace TestPatterns.FixtureManagement
{

    public class PersistentFreshFixtureTests : IClassFixture<FileInputFixture>
    {
        private readonly FileInputFixture _fixture;

        public PersistentFreshFixtureTests(FileInputFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void ImplicitFixtureTeardown()
        {
            const int amount = 1234;
            AccountReader accountReader = CreateAccountReader("./account1.txt", amount);

            accountReader.Load();

            Assert.Equal(amount, accountReader.Amount);
        }

        private AccountReader CreateAccountReader(string filePath, int amount)
        {
            _fixture.CreateInputFile(filePath, amount);
            return new AccountReader(filePath);
        }
    }

    public class AccountReader
    {
        private readonly string _filePath;

        public AccountReader(string filePath)
        {
            _filePath = filePath;
        }

        public int Amount{ get; private set; }

        public void Load()
        {
            string fileContent = File.ReadAllText(_filePath);
            Amount = int.Parse(fileContent);
        }
    }

    public class FileInputFixture : IDisposable
    {
        private readonly List<string> _createdFiles = new List<string>();

        public void CreateInputFile(string filePath, int amount)
        {
            CreateAppendToFile(filePath, amount);
            LogCreatedFile(filePath);
        }

        private static void CreateAppendToFile(string filePath, int amount)
        {
            // deliberately appending to force an issue when the file is not deleted in teardown
            using (var stream = File.AppendText(filePath))
            {
                stream.Write(amount);
                stream.Flush();
            }
        }

        private void LogCreatedFile(string filePath)
        {
            _createdFiles.Add(filePath);
        }

        public void Dispose()
        {
            _createdFiles.ForEach(File.Delete);
        }
    }
}
