namespace TwoSimpleDevs.Project.Core
{
  public class NotificationTrigger
  {
    public static void Instant(string appEvent)
    {
      var notification = new Notification.Builder()
                              .WithAppEvent(appEvent)
                              .WithLifeSpan(NotificationLifeSpan.Instant)
                              .Build();

      NotificationService.Trigger(notification);
    }

    public static void Instant(string appEvent, NotificationInfo info)
    {
      var notification = new Notification.Builder()
                              .WithAppEvent(appEvent)
                              .WithLifeSpan(NotificationLifeSpan.Instant)
                              .WithInfo(info)
                              .Build();

      NotificationService.Trigger(notification);
    }
  }
}