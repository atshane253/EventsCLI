using System;
using System.Diagnostics.Eventing.Reader;
using System.Xml.Linq;

namespace EventsCLI
{
    public class Watcher
    {
        bool PrintXML = true;
        EventLogWatcher watch;

        public Watcher(string text = "<QueryList><Query Id=\"0\" Path=\"Application\"><Select Path=\"Application\">*</Select><Select Path=\"System\">*</Select></Query></QueryList>")
        {
            watch = new EventLogWatcher(new EventLogQuery("Application", PathType.LogName, text));
        }

        public Watcher(string text, string server)
        {
            watch = new EventLogWatcher(new EventLogQuery("Application", PathType.LogName, text) { Session = new EventLogSession(server) });
        }
        
        public void StartWatch()
        {
            watch.Enabled = true;
            watch.EventRecordWritten += Watch_EventRecordWritten;
        }

        public void EndWatch()
        {
            watch.Enabled = false;
            watch.EventRecordWritten -= Watch_EventRecordWritten;
        }

        public void ToggleXML() => PrintXML = !PrintXML;

        private void Watch_EventRecordWritten(object sender, EventRecordWrittenEventArgs e)
        {
            var last = e.EventRecord;
            Console.WriteLine(last.FormatDescription());
            if (PrintXML)
                Console.WriteLine(XDocument.Parse(last.ToXml()));    
        }
    }
}
