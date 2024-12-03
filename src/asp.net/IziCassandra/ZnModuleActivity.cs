using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZnModelModule.OpenTelemetry
{
    public static class ZnModuleActivity
    {
        public readonly static ActivitySource ActivitySource = new ActivitySource(SOURCE_NAME);
        public const string SERVICE_NAME = "ZnModuleActivity.Service";
        public const string SOURCE_NAME = "ZnModuleActivity.Source";
    }
}
