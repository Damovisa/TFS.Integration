using System;
using Microsoft.TeamFoundation.Common;
using Microsoft.TeamFoundation.Framework.Server;
using Microsoft.TeamFoundation.VersionControl.Server;

namespace ServerObjectModel
{
    public class PerformanceCommentCheckinSubscriber : ISubscriber
    {
        public Type[] SubscribedTypes()
        {
            return new[] {typeof (CheckinNotification)}; 
        }

        public string Name
        {
            get { return "Performance Comment Checkin Subscriber"; }
        }

        public SubscriberPriority Priority
        {
            get { return SubscriberPriority.Low; }
        }

        public EventNotificationStatus ProcessEvent(TeamFoundationRequestContext requestContext,
            NotificationType notificationType,
            object notificationEventArgs, out int statusCode, out string statusMessage,
            out ExceptionPropertyCollection properties)
        {
            // set defaults
            statusCode = 0;
            properties = null;
            statusMessage = string.Empty;

            if (notificationType == NotificationType.DecisionPoint)
            {
                try
                {
                    if (notificationEventArgs is CheckinNotification)
                    {
                        var notification = notificationEventArgs as CheckinNotification;

                        // make sure we have "Performance Improvement" in our checkin
                        if (!notification.Comment.ToUpperInvariant().Contains("PERFORMANCE IMPROVEMENT"))  
                        {
                            statusMessage =
                                "Sorry, you can't check in without the words 'Performance Improvement'!";
                            return EventNotificationStatus.ActionDenied;
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Plugins cannot throw any exceptions or they'll get disabled by TFS. Log it and eat it instead!.
                    TeamFoundationApplicationCore.LogException("Performance Comment Checkin Prevention failed! {0}", ex);
                }
            }
            return EventNotificationStatus.ActionPermitted;
        }
    }
}
