using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.API.Utilities.Helpers;
using System.Collections.Generic;
using RSM.Xcelerate.ETL.Service.Client;
using Data.API;
using System;

namespace Tests.API.ETL.Files
{
    [TestClass]
    public class ETLGetFiles : ETLBaseTest
    {
        [TestMethod, TestCategory("Smoke")]
        [Description("247279 | GET / Files - valid")]
        public async Task GetAllFilesTest()
        {
            var _result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Files_GetListAsync());
            _result.Should().NotBeNull();
            VerifyFilesFields(_result.Results);
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("309828 | GET /Files with top = 2")]
        public async Task GetTopFiles()
        {
            int _fieldTop = 2;
            var _result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Files_GetListAsync(top: _fieldTop));
            _result.Should().NotBeNull();
            VerifyFilesFields(_result.Results);
            VerifyQuantity(_result.Results, _fieldTop);
        }

        [TestMethod, TestCategory("Smoke")]
        [Description("309406 | GET /Files with filter by fileType (correct)")]
        public async Task GetAllFilesFilterByFileType()
        {
            string _filterQuery = "fileType eq '" + Config._fileType + "'";
            var _result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Files_GetListAsync(filter: _filterQuery));
            _result.Should().NotBeNull();
            VerifyFilesFields(_result.Results, Config._fileType);
        }

        [TestMethod]
        [Description("309435 | GET /Files with filter by fileType (empty)")]
        public async Task GetAllFilesFilterByEmptyFileType()
        {
            string _filterQuery = "fileType eq ''";

            await EtlHttpResponseHelper.VerifyNotInternalServerErrorAsync(() => EtlUserClient.Files_GetListAsync(filter: _filterQuery));
        }

        [TestMethod]
        [Description("309437 | GET /Files with filter by fileType (null)")]
        public async Task GetAllFilesFilterByNullFileTypeTest()
        {
            string _filterQuery = "fileType eq null";

            await EtlHttpResponseHelper.VerifyNotInternalServerErrorAsync(() => EtlUserClient.Files_GetListAsync(filter: _filterQuery));
        }

        [TestMethod]
        [Description("309451 | GET /Files with filter by fileType (not existing)")]
        public async Task GetAllFilesFilterByNotExistingFileTypeTest()
        {
            string _filterQuery = "fileType eq '" + Config._notExistingFileType + "'";

            var _result = await EtlHttpResponseHelper.VerifySuccessfulStatusAsync(() => EtlUserClient.Files_GetListAsync(filter: _filterQuery));
            _result.Should().NotBeNull();
            _result.Results.Should().BeEmpty();
        }

        [TestMethod]
        [Description("247944 | GET /Files - unauthorized request")]
        public async Task GetAllFilesWithoutTokenTest()
        {
            await EtlHttpResponseHelper.VerifyUnauthorizedAsync(() => EtlNoTokenClient.Files_GetListAsync());
        }

        [TestMethod]
        [Description("309786 | GET /Files with filtering by fileType - unauthorized request")]
        public async Task GetAllFilesWithFilterWithoutTokenTest()
        {
            string _filterQuery = "fileType eq '" + Config._fileType + "'";
            await EtlHttpResponseHelper.VerifyUnauthorizedAsync(() => EtlNoTokenClient.Files_GetListAsync(filter: _filterQuery));
        }

        private static void VerifyFilesFields(ICollection<FileDto> _FilesResult, string _fileType="")
        {
            foreach (var item in _FilesResult)
            {
                VerifyFilesFields(item);
                if (!String.IsNullOrEmpty(_fileType))
                { 
                    VerifyFilesFileType(item, _fileType);
                }
            }
        }
        private static void VerifyFilesFields(FileDto FilesActualResult)
        {
            Assert.IsNotNull(FilesActualResult.Id, "Id should not be empty");
            Assert.IsNotNull(FilesActualResult.FileType, "FileType should not be empty");
            Assert.IsNotNull(FilesActualResult.SchemaType, "SchemaType should not be empty");
        }
        private static void VerifyFilesFileType(FileDto FilesActualResult, string fileType)
        {
            Assert.AreEqual(fileType, FilesActualResult.FileType.ToString(), "Wrong File Type in the response.");
        }

        private static void VerifyQuantity(ICollection<FileDto> _filesResult, int _topNmb)
        {
            Assert.AreEqual(_topNmb, _filesResult.Count, _topNmb + "Files in the response.");
        }
    }
}