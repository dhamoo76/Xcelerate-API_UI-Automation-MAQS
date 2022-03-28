using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.API.Utilities.Helpers;
using System.Collections.Generic;
using RSM.Xcelerate.ETL.Service.Client;
using Data.API;
using System;

namespace Tests.API.ETL.FileTypes
{
    [TestClass]
    public class ETLGetFilesTypes : ETLBaseTest
    {
        [TestMethod, TestCategory("Smoke")]
        [Description("309818 | GET / FileTypes - valid")]
        public async Task GetAllFileTypesTest()
        {
            var _result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.FileTypes_GetListAsync());
            _result.Should().NotBeNull();
            VerifyFileTypesFields(_result.Results);
        }
        [TestMethod, TestCategory("Smoke")]
        [Description("310014 | GET /FileTypes with top = 2")]
        public async Task GetTopFileTypes()
        {
            int _fieldTop = 2;
            var _result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.FileTypes_GetListAsync(top: _fieldTop));
            _result.Should().NotBeNull();
            VerifyFileTypesFields(_result.Results);
            VerifyQuantity(_result.Results, _fieldTop);
        }

        [TestMethod]
        [Description("309820 | GET /FileTypes - unauthorized request")]
        public async Task GetAllFileTypesWithoutTokenTest()
        {
            await EtlHttpResponseHelper.VerifyUnauthorizedAsync(() => EtlNoTokenClient.Files_GetListAsync());
        }

        private static void VerifyFileTypesFields(ICollection<FileTypeDto> _results)
        {
            foreach (var item in _results)
            {
                VerifyFileTypesFields(item);
            }
        }
        private static void VerifyFileTypesFields(FileTypeDto FilesActualResult)
        {
            Assert.IsNotNull(FilesActualResult.Id, "Id should not be empty");
            Assert.IsNotNull(FilesActualResult.Name, "Name should not be empty");
        }
        private static void VerifyQuantity(ICollection<FileTypeDto> _fileTypesResult, int _topNmb)
        {
            Assert.AreEqual(_topNmb, _fileTypesResult.Count, _topNmb + "FileTypes in the response.");
        }
    }

}