using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DZLib.Logging
{
    /// <summary>
    /// The Log Severity enum
    /// </summary>
    [DefaultValue(Medium)]
    enum LogSeverity
    {
        Warning, Error, Low, Medium, Severe
    }
}
