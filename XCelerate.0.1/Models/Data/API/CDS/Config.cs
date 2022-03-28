using System;

namespace Data.API.CDS
{
    // Config data class for CDS API tests
    public static class Config
    {
        public static readonly long Timestamp = new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds();

        // Valid data
        public const int MdmClientId = 7715692;
        public const string ClientName = "Acors Canopy Ltd.";

        // No access
        public const int NoAccessMdmClientId = 6557155;
        public const string NoAccessLegalEntityId = "55acda94-7a22-4734-9925-08d9a8469610";

        // Not existing data
        public const int NotExistingMdmClientId = 1111111;
    }
}