using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP_E2E
{
    public static class TestSettings
    {
        
        public static string BaseUrl =>
            Environment.GetEnvironmentVariable("BASE_URL")
            ?? "http://localhost:53135"; 
    }
}
