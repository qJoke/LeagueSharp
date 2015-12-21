using System.ComponentModel;

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
