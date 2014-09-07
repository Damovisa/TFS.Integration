using System;
using System.Xml;
namespace WcfNotificationEndpoint
{
    public class NotificationService : INotificationService
    {
        public void Notify(string eventXml, string tfsIdentityXml)
        {
            // write the Event and TFSIdentity XML contents out for logging purposes
            if (!string.IsNullOrEmpty(eventXml))
                System.IO.File.WriteAllText(@"C:\TfsNotifications\Event.xml", eventXml);
            if (!string.IsNullOrEmpty(tfsIdentityXml))
                System.IO.File.WriteAllText(@"C:\TfsNotifications\TFSIdentity.xml", tfsIdentityXml);

            // write important information
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(eventXml);
            var root = xmlDoc.DocumentElement;

            if (root.SelectSingleNode("/WorkItemChangedEvent") != null)
            {
                var strChanges = new System.Text.StringBuilder();

                var workItemId = root.SelectSingleNode("/WorkItemChangedEvent/CoreFields/IntegerFields/Field/Name[.=\"ID\"]/../NewValue").InnerText;
                strChanges.AppendFormat("Work Item {0} Changed{1}", workItemId, Environment.NewLine);
                
                var changes = root.SelectNodes("/WorkItemChangedEvent/ChangedFields/*/Field");
                foreach (XmlNode change in changes)
                {
                    var name = change.SelectSingleNode("Name").InnerText;
                    var oldValue = change.SelectSingleNode("OldValue").InnerText;
                    var newValue = change.SelectSingleNode("NewValue").InnerText;
                    strChanges.AppendFormat("  [{0}]:  {1} -> {2}{3}", name, oldValue, newValue, Environment.NewLine);
                }
                System.IO.File.AppendAllText(@"C:\TfsNotifications\Changes.log", strChanges.ToString());
            }

            //todo: Write some information to Table Storage in Azure?
        }
    }
}
