namespace GeekBurger.Users.Application.AzureServices.Configs
{
    public class ServiceBusConfiguration
    {
        public string ResourceGroup { get; set; }

        public string NamespaceName { get; set; }

        public string ConnectionString { get; set; }

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string SubscriptionId { get; set; }

        public string TenantId { get; set; }

        /*
         "resourceGroup": "fiap",
    "namespaceName": "GeekBurger",
    "connectionString": "\"Endpoint=sb://geekburger.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;Shar",
    "edAccessKey=/PGLAJOC7WDV5QkBNz+GodPhlnBPEL6Iwd/ThkKnBcs=\",": null,
    "clientId": "31d24bf2-5475-41e7-86c4-3e3d971ad2cb",
    "clientSecret": "lovetoteach",
    "subscriptionId": "dbc49a7f-caee-46b5-a6a6-7eac85bf97f1",
    "tenantId": "11dbbfe2-89b8-4549-be10-cec364e59551"
         
         */
    }
}
