using Azure.Storage.Blobs;
using Magenic.Maqs.BaseTest;
using Magenic.Maqs.Utilities.Logging;
using Magenic.Maqs.Utilities.Performance;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RSM.Xcelerate.ETL.Test.AzureStorageBlob
{
    /// <summary>
    /// Test object for the <see cref="BlobContainerClient"/>.
    /// </summary>
    /// <remarks>
    /// The MAQS classes <see cref="AzureStorageDriverManager"/> and <see cref="AzureStorageTestObject"/> 
    /// provide a layer of indirection to take advantage of MAQS features like the lazy loading of 
    /// <see cref="BlobContainerClient"/> (test cases that don't make calls to Azure will never instantiate 
    /// the <see cref="BlobContainerClient"/>), while also providing a layer of abstraction that seperates 
    /// test case logic from Azure Blob Storage implementation details.
    /// </remarks>
    public class AzureStorageTestObject : BaseTestObject
    {
        public AzureStorageTestObject(string connection, string bucket, ITestObject baseTestObject) : base(baseTestObject)
        {
            ManagerStore.Add(typeof(AzureStorageDriverManager).FullName, new AzureStorageDriverManager(connection, bucket, this));
        }

        public AzureStorageTestObject(string connection, string bucket, ILogger logger, string fullyQualifiedTestName) : base(logger, fullyQualifiedTestName)
        {
            ManagerStore.Add(typeof(AzureStorageDriverManager).FullName, new AzureStorageDriverManager(connection, bucket, this));
        }

        public AzureStorageTestObject(string connection, string bucket, ILogger logger, ISoftAssert softAssert, IPerfTimerCollection collection, string fullyQualifiedTestName) : base(logger, softAssert, collection, fullyQualifiedTestName)
        {
            ManagerStore.Add(typeof(AzureStorageDriverManager).FullName, new AzureStorageDriverManager(connection, bucket, this));
        }

        /// <summary>
        /// Upload file to Azure blob storage in the respective location.
        /// </summary>
        /// <param name="username">
        /// The RSM ETL username to upload the file under.
        /// </param>
        /// <param name="mdmClientId">
        /// The client ID for the payload by which the file is to be consumed.
        /// </param>
        /// <param name="foldername">
        /// The foldername specified when creating the ETL protocol.
        /// </param>
        /// <param name="filename">
        /// A filename that will match the convention specified when creating the ETL payload.
        /// </param>
        public async Task UploadFile(string username, int mdmClientId, string foldername, string filename)
        {
            var client = Manager.GetClient().GetBlobClient($"{username}/{mdmClientId}/{foldername}/{filename}");

            await client.UploadAsync(filename);
        }
        
        public AzureStorageDriverManager Manager
        {
            get
            {
                return ManagerStore.GetManager<AzureStorageDriverManager>();
            }
        }
    }
}
