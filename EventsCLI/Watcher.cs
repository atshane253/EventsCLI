using System;
using System.Diagnostics.Eventing.Reader;
using System.Xml.Linq;

namespace EventsCLI
{
    public class Watcher
    {
        bool PrintXML = false;
        EventLogWatcher watch;
        private bool printedLastXML;
        private EventRecord last;

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

        public void ToggleXML()
        {
            PrintXML = !PrintXML;
            if (PrintXML && !printedLastXML)
            {
                printedLastXML = true;
                Console.WriteLine(XDocument.Parse(last.ToXml()));
            }
        }

        private void Watch_EventRecordWritten(object sender, EventRecordWrittenEventArgs e)
        {
            last = e.EventRecord;
            var (BackgroundColor, ForegroundColor) = (Console.BackgroundColor, Console.ForegroundColor);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine();
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"Event {last.RecordId} at {last.TimeCreated}");
            Console.BackgroundColor = BackgroundColor;
            Console.ForegroundColor = ForegroundColor;
            Console.WriteLine(last.FormatDescription());
            printedLastXML = PrintXML;
            if (PrintXML)
                Console.WriteLine(XDocument.Parse(last.ToXml()));
        }
    }
}
