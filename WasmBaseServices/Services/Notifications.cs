using Radzen;

namespace WasmBaseServices.Services
{
    public class Notifications
    {
        private int _DelayTimeSeconds = 2;

        private readonly Radzen.NotificationService Ns;

        public Notifications(Radzen.NotificationService ns) {

            Ns = ns;
        }

        public void NotifyThemDanger(string title)
        {
            NotifyDry(title, NotificationSeverity.Error);

            //var msg = new NotificationMessage { Severity = NotificationSeverity.Error, Summary = title, Duration = 2000 };
            //Ns.Notify(msg);
        }

        public void NotifyThemSuccess(string title)
        {
            NotifyDry(title, NotificationSeverity.Success);

            //var msg = new NotificationMessage { Severity = NotificationSeverity.Success, Summary = title, Duration = 2000 };
            //Ns.Notify(msg);
        }


        private void NotifyDry(string title, NotificationSeverity severity)
        {
            var msg = new NotificationMessage { Severity = severity, Summary = title, Duration = (_DelayTimeSeconds * 1000) };
            Ns.Notify(msg);
        }
    }
}