using System;
using System.Collections.Generic;
using System.Text;
using Data.API;
using RSM.Xcelerate.ETL.Service.Client;

namespace RSM.Xcelerate.Test.Shared.ETL
{
    /// <summary>
    /// Factory object to make initializing API POST request bodies easier.
    /// </summary>
    public class EtlRequestFactory
    {
        /// <summary>
        /// Object for storing the application environment, to use when building requests.
        /// </summary>
        public Config config;

        public EtlRequestFactory(Config initialConfig)
        {
            config = initialConfig;
        }

        /// <summary>
        /// Uses the configuration to build a create protocol request.
        /// </summary>
        /// <param name="config">
        /// Optional configuration for building the request. If not provided, the configuration provided on object initialization is used.
        /// </param>
        /// <returns>
        /// The body for the create protocol request.
        /// </returns>
        public CreateProtocolCommandRequest BuildCreateProtocolCommandRequest(Config? config = null)
        {
            config = config ?? this.config;

            return new CreateProtocolCommandRequest
            {
                Name = config.protocolName,
                ProtocolTypeId = new Guid(Config._protocolTypeId), 
                MdmClientId = int.Parse(Config._mdmClientId),
                Properties = new List<PropertyRequestDto>
                {
                    new PropertyRequestDto { Name = "url", Value = config.internalSftpUrl },
                    new PropertyRequestDto { Name = "folderPath", Value = config.folderPath },
                    new PropertyRequestDto { Name = "username", Value = ""},
                    new PropertyRequestDto { Name = "password", Value = ""}
                }
            };
        }

        /// <summary>
        /// Uses the configuration to build a create payload request.
        /// </summary>
        /// <param name="config">
        /// Optional configuration for building the request. If not provided, the configuration provided on object initialization is used.
        /// </param>
        /// <returns>
        /// The body for the create payload request.
        /// </returns>
        public CreatePayloadDefinitionCommandRequest BuildCreatePayloadDefinitionCommandRequest(Config? config = null)
        {
            config = config ?? this.config;

            return new CreatePayloadDefinitionCommandRequest
            {
                Name = config.payloadName,
                ProtocolId = config.protocolGuid,
                EngagementIds = config.payloadEngagementIds,
                ProjectIds = config.payloadProjectIds,
                LegalEntityIds = config.payloadLegalEntityIds,
                NotificationRecipients = config.username,
                MdmClientId = config.mdmClientId,
                Procedures = new List<ProcedureDto>
                {
                    new ProcedureDto
                    {
                        Id = config.payloadProcedureId,
                        FilePatterns = new List<FilePatternDto>
                        {
                            new FilePatternDto
                            {
                                FileId = config.payloadFileId,
                                Pattern = config.payloadFilenamePattern
                            }
                        }
                    }
                }
            };
        }
    }
}
