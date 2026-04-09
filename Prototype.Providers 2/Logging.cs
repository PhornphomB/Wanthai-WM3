using System;
using System.Diagnostics;
using System.IO;
using System.Text;


namespace Prototype.Providers
{
    public class Logging : IDisposable
    {

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void Raise(global::Prototype.Providers.EventHandler _event)
        {
            if (_event != null)
                _event(Object, new global::Prototype.Providers.EventArgsCustom(this, Object));
        }

        /// <summary>
        /// Provides the base functionality and properties for creating Loggings.
        /// </summary>
        public Logging()
        {
            Source = this.GetType().ToString();
            EventID = 0;
            Exception = null;
            EntryType = System.Diagnostics.EventLogEntryType.Information;
            WriteTo = EventLogWriteTo.SystemAndText;
        }


        // Logging case exception from web page
        public Logging(object _source, Exception _exception)
            : this(_source, _exception, EventLogEntryType.Error, 1090,
            Message.Message_PageException.LoggingText((object)_source), Message.Message_StringNothing)
        { }

        public Logging(object _source)
            : this(_source, null, EventLogEntryType.Information) { }

        public Logging(object _source, Exception _exception, EventLogEntryType _entryType)
            : this(_source, _exception, _entryType, 0) { }

        public Logging(object _source, Exception _exception, EventLogEntryType _entryType, int _eventID)
            : this(_source, _exception, _entryType, _eventID, string.Empty) { }

        public Logging(object _source, int _eventID, string _description)
            : this(_source, null, EventLogEntryType.Information, _eventID, _description) { }

        public Logging(object _source, Exception _exception, EventLogEntryType _entryType, int _eventID, string _description)
            : this(_source, _exception, _entryType, _eventID, _description, Message.Message_StringNothing) { }

        public Logging(object _source, EventLogEntryType _entryType, int _eventID, string _description)
            : this(_source, null, _entryType, _eventID, _description, Message.Message_StringNothing) { }

        public Logging(object _source, EventLogEntryType _entryType, int _eventID, string _description, string _solution)
            : this(_source, null, _entryType, _eventID, _description, _solution) { }


        public Logging(object _source, Exception _exception, EventLogEntryType _entryType, int _eventID, string _description, string _solution)
        {
            Object = _source;
            Source = _source.GetType().ToString();
            Exception = _exception;
            EntryType = _entryType;
            EventID = _eventID;
            Description = _description.Replace("{T}", _source.GetType().Name);
            Solution = _solution.Replace("{T}", _source.GetType().Name);
            if (Exception != null)
            {
                ExceptionMessage = this.GetExceptionMessage().Trim();
                Write();
            }
        }

        public string ExceptionMessage { get; set; }

        public string Description { get; set; }

        public string Solution { get; set; }

        public object Object { get; set; }

        public string Source { get; set; }

        public int EventID { get; set; }

        public Exception Exception { get; set; }

        public EventLogEntryType EntryType { get; set; }

        public EventLogWriteTo WriteTo { get; set; }

        public void Write()
        {
            try
            {

                switch (WriteTo)
                {
                    case EventLogWriteTo.SystemWindows:
                        //this.WriteSystemLogging();
                        break;
                    case EventLogWriteTo.ExceptionText:
                        LoggingFile.WriteTextLogging(LoggingText(), LoggingFile.LogFileName(Source));
                        break;
                    case EventLogWriteTo.SystemAndText:
                        //WriteSystemLogging();
                        LoggingFile.WriteTextLogging(LoggingText(), LoggingFile.LogFileName(Source));
                        break;
                    default:
                        //WriteSystemLogging();
                        LoggingFile.WriteTextLogging(LoggingText(), LoggingFile.LogFileName(Source));
                        break;
                }
            }
            catch { }
        }
        /// <summary>
        /// Write the system application event log of current specifies logging.
        /// </summary>
        private void WriteSystemLogging()
        {
            EventLog eventlog = new EventLog();
            eventlog.MachineName = System.Environment.MachineName;
            eventlog.Source = this.Source;
            eventlog.WriteEntry("Exception : " + this.GetExceptionMessage()
                + "\n\nStack Trace : " + Exception.StackTrace.Trim(), this.EntryType, this.EventID);
        }

        private string GetExceptionMessage()
        {
            var ex_deep = this.Exception;

            while (ex_deep.InnerException != null)
                ex_deep = ex_deep.InnerException;

            return ex_deep.Message;
        }

        public string LoggingText()
        {
            StringBuilder strLog = new StringBuilder();
            strLog.AppendLine("  LOG DATE    : " + System.DateTime.Now.ToString());
            strLog.AppendLine("  MACHINE     : " + System.Environment.MachineName);
            strLog.AppendLine("  DOMAIN/USER : " + System.Environment.UserDomainName + "/" + System.Environment.UserName);
            strLog.AppendLine("  IP ADDRESS  : " + Extension.IPAddress);
            strLog.AppendLine("  SOURCE      : " + Source);
            strLog.AppendLine("  EVENT ID    : " + EventID.ToString());
            if (Exception != null)
            {
                strLog.AppendLine("  ASSEMBLY    : " + Exception.Source);
                strLog.AppendLine("  MESSAGE     : " + Exception.Message);
                strLog.AppendLine("  INNER MESSAGE     : " + this.GetExceptionMessage().Trim());
                strLog.AppendLine("  STACK TRACE : " + Exception.StackTrace ?? "");
            }
            else
            {
                strLog.AppendLine("  ASSEMBLY    : " + Source);
                strLog.AppendLine("  DESCRIPTON  : " + Description);
                if (Solution != string.Empty)
                    strLog.AppendLine("  SOLUTION    : " + Solution);
            }
            return strLog.ToString();
        }
    }
    /// <summary>
    /// Specifies the options for Logging write method. 
    /// </summary>
    public enum EventLogWriteTo
    {
        SystemAndText,
        SystemWindows,
        ExceptionText
    }

    public static class LoggingFile
    {
        public static string LogFileName(string _sourceClass)
        {
            string folderLog = @"C:\_WMS_Logs";
            if (!Directory.Exists(folderLog))
                Directory.CreateDirectory(folderLog);

            string projectLog = folderLog + @"\" + _sourceClass.Split('.')[0];
            if (!Directory.Exists(projectLog))
                Directory.CreateDirectory(projectLog);

            string folderName = projectLog + @"\LOG_" + DateTime.Now.Year.ToString("0000") + "-" + DateTime.Now.Month.ToString("00");
            if (!Directory.Exists(folderName))
                Directory.CreateDirectory(folderName);

            string fileName = folderName + @"\" + DateTime.Now.Year.ToString("0000") + "-" + DateTime.Now.Month.ToString("00") + "-" + DateTime.Now.Day.ToString("00") + ".log";

            return fileName;
        }
        public static bool WriteTextLogging(string _exception, string _filename)
        {
            try
            {
                using (var writer = new StreamWriter(_filename, true))
                {
                    writer.WriteLine(_exception);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

