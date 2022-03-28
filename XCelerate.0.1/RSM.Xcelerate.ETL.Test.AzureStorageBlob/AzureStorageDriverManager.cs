using Azure.Storage.Blobs;
using Magenic.Maqs.BaseTest;
using System;

namespace RSM.Xcelerate.ETL.Test.AzureStorageBlob
{
    /// <summary>
    /// MAQS wrapper for the Azure <see cref="BlobContainerClient"/>.
    /// </summary>
    /// <remarks>
    /// The MAQS classes <see cref="AzureStorageDriverManager"/> and <see cref="AzureStorageTestObject"/> 
    /// provide a layer of indirection to take advantage of MAQS features like the lazy loading of 
    /// <see cref="BlobContainerClient"/> (test cases that don't make calls to Azure will never instantiate 
    /// the <see cref="BlobContainerClient"/>), while also providing a layer of abstraction that seperates 
    /// test case logic from Azure Blob Storage implementation details.
    /// </remarks>
    public class AzureStorageDriverManager : DriverManager
    {

        public AzureStorageDriverManager(string connection, string bucket, ITestObject testObject) 
            : base(() => new BlobContainerClient(connection, bucket), testObject)
        {
        }

        public override object Get()
        {
            return GetClient();
        }

        public BlobContainerClient GetClient()
        {
            return GetBase() as BlobContainerClient;
        }

        protected override void DriverDispose()
        {
        }
    }
}
