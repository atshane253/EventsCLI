using System;
using System.Diagnostics.Eventing.Reader;
using System.Xml.Linq;

namespace EventsCLI
{
    public class Watcher
    {
        EventLogQuery query;
        EventLogWatcher watch;
        EventRecord last;
        const string queryString ="<QueryList><Query Id=\"0\" Path=\"Application\"><Select Path=\"Application\">*</Select><Select Path=\"System\">*</Select></Query></QueryList>";

        public Watcher(string text = queryString)
        {
            query = new EventLogQuery("Application", PathType.LogName, text);
            watch = new EventLogWatcher(query);
        }

        public Watcher(string text, string server)
        {
            query = new EventLogQuery("Application", PathType.LogName, text);
            query.Session = new EventLogSession(server);
            watch = new EventLogWatcher(query);
        }

        public void StartWatch()
        {
            watch.Enabled = true;
            watch.EventRecordWritten += Watch_EventRecordWritten;
        }

        private void Watch_EventRecordWritten(object sender, EventRecordWrittenEventArgs e)
        {
            last = e.EventRecord;
            Console.WriteLine(last.FormatDescription());
        }

        public void WriteLastXML()
        {
            if (last != null)
                Console.WriteLine(XDocument.Parse(last.ToXml()));            
        }
    }
}
