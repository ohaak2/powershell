﻿using System.Text.Json.Serialization;

namespace PnP.PowerShell.Commands.Model.AzureAD
{
    /// <summary>
    /// Info section within an Azure Active Directory Service Principal entity
    /// </summary>
    public class AzureADServicePrincipalInfo
    {
        [JsonPropertyName("logoUrl")]
        public string LogoUrl { get; set; }

        [JsonPropertyName("marketingUrl")]
        public string MarketingUrl { get; set; }

        [JsonPropertyName("privacyStatementUrl")]
        public string PrivacyStatementUrl { get; set; }

        [JsonPropertyName("supportUrl")]
        public string SupportUrl { get; set; }

        [JsonPropertyName("termsOfServiceUrl")]
        public string TermsOfServiceUrl { get; set; }
    }
}
