using System.Collections.Generic;

namespace BistHub.Api.Common
{
    public class FirebaseConfig
    {
        public string ApiKey { get; set; }
        public string AppName { get; set; }
        public string ClientId { get; set; }
    }

    public class GoogleSheetsConfig
    {
        public string ApiKey { get; set; }
        public string SheetId { get; set; }
        public Dictionary<string, string> CustomSheets { get; set; }
    }
}
