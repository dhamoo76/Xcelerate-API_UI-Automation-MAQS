using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RSM.Xcelerate.ETL.Service.Client;
using RSM.Xcelerate.CEM.Service.Client;
using RSM.Xcelerate.CDS.Service.Client;
using Tests.API.Utilities.Helpers;
using System.Threading;
using ApiException = RSM.Xcelerate.ETL.Service.Client.ApiException;
using CognizantSoftvision.Maqs.Utilities.Helper;
using RSM.Xcelerate.Test.Shared;
using Tests.Common.Etl;

namespace Tests.API.ETL
{
    [TestClass]
    public class InternalSftpTests: EtlUserTokenBaseClass
    {
        [TestMethod]
        [Description("312424 | Internal SFTP file processed by ETL")]
        public async Task FileProcessedByETL()
        {
            // Fetch username from Okta 
            EtlRequestFactory.config.username = await Etl.GetUsername();

            // Setup Protocol
            var createProtocolResult = await Etl.AddProtocol(EtlRequestFactory.BuildCreateProtocolCommandRequest());

            // Update the test data so it is aware of protocol id
            EtlRequestFactory.config.protocolGuid = createProtocolResult.Id;

            // Setup Payload
            await Etl.AddPayload(EtlRequestFactory.BuildCreatePayloadDefinitionCommandRequest());

            // Upload file to blob storage
            await Azure.UploadFile(
                EtlRequestFactory.config.username,
                EtlRequestFactory.config.mdmClientId, 
                EtlRequestFactory.config.folderPath, 
                "legalentitiestest.xlsx"
            );

            GetTransactionWithPayloadDefinitionResponse result = null;

            // The jobs to import and process the file should be completed within 7 minutes.
            // The below waits are set to check that the file has been processed every 30 seconds.
            // Add an additonal check (30 seconds) to account for 'perfect timing' scenarios.
            
            // Check every 30 seconds for the file blob to be imported into ETL.
            await Utility.WaitAsync(async () =>
            {
                result = await Etl.GetPayloadDefinition(EtlRequestFactory.config.mdmClientId, EtlRequestFactory.config.payloadName);

                return result.Results.Count > 0;
            }, new TimeSpan(0, 0, 30), new TimeSpan(0, 7, 30), true);

            // Check every 30 seconds for the file to be processed by the ETL procedure. 
            await Utility.WaitAsync(async () =>
            {
                result = await Etl.GetPayloadDefinition(EtlRequestFactory.config.mdmClientId, EtlRequestFactory.config.payloadName);

                return result.Results.FirstOrDefault().Status == TransactionStatus.Success;
            }, new TimeSpan(0, 0, 30), new TimeSpan(0, 7, 30), true);

            var payload = result.Results.FirstOrDefault();

            Assert.IsNotNull(payload, $"Payload was not found.");
            Assert.IsTrue(payload.Status == TransactionStatus.Success, $"Payload was not imported successfully. Status is {Enum.GetName(payload.Status)}.");
        }
    }
}
