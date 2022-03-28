using System;
using System.Threading.Tasks;
using FluentAssertions;
using Tests.API.Utilities.Helpers;
using System.Linq;
using RSM.Xcelerate.ETL.Service.Client;
using System.Collections.Generic;
using Data.API;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.API.Utilities.Common;

namespace Tests.API.ETL
{
    public class ETLBaseTest : CommonTokenBaseClass
    {
        public static string GenerateRandomString(int length, string context = "")
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[length];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            var finalString = new String(stringChars);

            return "Auto" + context + finalString;
        }

        public async Task<int> GetRandomMdmClientIdAsync()
        {
            var random = new Random();

            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Clients_GetClientsListAsync());
            result.Should().NotBeNull();
            var clientsTable = result.Results.ToList();
            int index = random.Next(clientsTable.Count);

            return clientsTable[index].MdmClientId;
        }

        public async Task<Guid> GetRandomProtocolIdAsync()
        {
            var random = new Random();

            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Protocols_GetProtocolsAsync());
            result.Should().NotBeNull();
            var protocolsTable = result.Results.ToList();
            int index = random.Next(protocolsTable.Count);

            return protocolsTable[index].Id;
        }

        public async Task<ProtocolTypeDto> GetRandomProtocolTypeDataAsync()
        {
            var random = new Random();

            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.ProtocolTypes_GetAsync());
            result.Should().NotBeNull();
            var protocolTypesTable = result.Results.ToList();
            int index = random.Next(protocolTypesTable.Count);

            return protocolTypesTable[index];
        }

        public async Task<Guid> GetRandomProtocolTypeIdAsync()
        {
            var random = new Random();

            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.ProtocolTypes_GetAsync());
            result.Should().NotBeNull();
            var protocolTypesTable = result.Results.ToList();
            int index = random.Next(protocolTypesTable.Count);

            return protocolTypesTable[index].Id;
        }

        public async Task<ProtocolTypeDto> GetInternalSftpProtocolTypeDataAsync()
        {
            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.ProtocolTypes_GetAsync());
            result.Should().NotBeNull();
            var protocolTypesTable = result.Results.ToList();
            int index = 0;

            return protocolTypesTable[index];
        }

        public async Task<string> GetRandomProtocolTypeNameAsync()
        {
            var random = new Random();

            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.ProtocolTypes_GetAsync());
            result.Should().NotBeNull();
            var protocolTypesTable = result.Results.ToList();
            int index = random.Next(protocolTypesTable.Count);

            return protocolTypesTable[index].Name;
        }

        public async Task<CreateProtocolCommandRequest> GeneratePostProtocolsBody()
        {
            var protocolType = await GetInternalSftpProtocolTypeDataAsync();
            Guid randomProtocolTypeId = protocolType.Id;
            int mdmClientId = int.Parse(Config._mdmClientId);
            ICollection<PropertyRequestDto> properties = new List<PropertyRequestDto> { };

            foreach (var item in protocolType.Properties)
            {
                PropertyRequestDto property = new();
                property.Name = item.Name;
                property.Value = GenerateRandomString(10, item.Name);
                properties.Add(property);
            }

            var body = new CreateProtocolCommandRequest()
            {
                Name = GenerateRandomString(10, "ProtocolName"),
                MdmClientId = mdmClientId,
                Description = GenerateRandomString(10, "Description"),
                ProtocolTypeId = randomProtocolTypeId,
                Properties = properties
            };

            return body;
        }

        public async Task<string> GetRandomApplicationIdAsync()
        {
            var random = new Random();

            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Applications_GetListAsync());
            result.Should().NotBeNull();
            var applicationsTable = result.Results.ToList();
            int index = random.Next(applicationsTable.Count);

            return applicationsTable[index].Id;
        }

        public async Task<Guid> GetRandomProcedureIdAsync()
        {
            var random = new Random();

            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Procedures_GetListAsync());
            result.Should().NotBeNull();
            var proceduresTable = result.Results.ToList();
            int index = random.Next(proceduresTable.Count);

            return proceduresTable[index].Id;
        }

        public async Task<ProcedureListDto> GetRandomProcedureDataWithFilesAsync()
        {
            var random = new Random();

            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Procedures_GetListAsync());
            result.Should().NotBeNull();
            var proceduresTable = result.Results.ToList();

            ICollection<ProcedureListDto> proceduresWithFilesList = new List<ProcedureListDto> { };
            foreach (var item in proceduresTable)
            {
                if(item.Files.Count != 0)
                    proceduresWithFilesList.Add(item);
            }

            int index = random.Next(proceduresWithFilesList.Count);
            var procedures = proceduresWithFilesList.ToList();

            return procedures[index];
        }

        public async Task<Guid> GetRandomAlteryxWorkflowIdAsync()
        {
            var random = new Random();

            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.AlteryXWorkflows_GetWorkflowsAsync());
            result.Should().NotBeNull();
            var alteryxWorkflowsTable = result.Results.ToList();
            int index = random.Next(alteryxWorkflowsTable.Count);

            return alteryxWorkflowsTable[index].Id;
        }

        public async Task<String> GetRandomAlteryxIdAsync()
        {
            var random = new Random();

            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.AlteryXJob_GetListAsync());
            result.Should().NotBeNull();
            var alteryxJobsTable = result.Results.ToList();
            int index = random.Next(alteryxJobsTable.Count);

            return alteryxJobsTable[index].AlteryXJobId;
        }

        public async Task<Guid> GetRandomTransactionIdAsync()
        {
            var random = new Random();

            var result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Transactions_GetEtlUploaderTransactionsAsync());
            result.Should().NotBeNull();
            var transactionsTable = result.Results.ToList();
            int index = random.Next(transactionsTable.Count);

            return transactionsTable[index].TransactionId;
        }
        
        public async Task<StartTransactionWithFilesCommandRequest> GeneratePostTransactionsStartwithfilesBody()
        {
            string _applicationId = await GetRandomApplicationIdAsync();
            ProcedureListDto _randomProcedure = await GetRandomProcedureDataWithFilesAsync();
            Guid _randomProcedureId = _randomProcedure.Id;
            ICollection<FileForFileTypeDto> files = new List<FileForFileTypeDto> { };

            foreach (var item in _randomProcedure.Files)
            {
                FileForFileTypeDto file = new();
                file.FileId = item.Id;
                file.FileName = GenerateRandomString(5, "FileName") + ".XLSX";
                files.Add(file);
            }

            StartTransactionWithFilesCommandRequest _body = new()
            {
                ApplicationId = _applicationId,
                ProcedureId = _randomProcedureId,
                Files = files
            };

            return _body;
        }

        public static void VerifyTransactionFields(GetTransactionDetailsQueryResponse transactionDetailsResult, Guid transactionId, string status = "")
        {
            Assert.AreEqual(transactionId, transactionDetailsResult.Id, "Transaction Id is not correct");
            Assert.IsNotNull(transactionDetailsResult.StartDate, "StartDate should not be empty");
            Assert.IsNotNull(transactionDetailsResult.Status, "Status should not be empty");
            Assert.IsNotNull(transactionDetailsResult.CallingUserId, "CallingUserId should not be empty");
            Assert.IsNotNull(transactionDetailsResult.CallingApplicationId, "CallingApplicationId should not be empty");
            Assert.IsNotNull(transactionDetailsResult.ResultFiles, "ResultFiles table should not be null");
            Assert.IsNotNull(transactionDetailsResult.MatchedFiles, "MatchedFiles table should not be null");
            if (status != "")
                Assert.AreEqual(status, transactionDetailsResult.Status.ToString(), "Transaction status is not correct");
        }
    }
}
