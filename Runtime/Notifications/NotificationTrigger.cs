namespace TwoSimpleDevs.Project.Core
{
  public static class NotificationTrigger
  {
    public static void Instant(string name)
    {
      var notification = new Notification.Builder()
                              .WithName(name)
                              .WithLifeSpan(NotificationLifeSpan.Instant)
                              .Build();

      NotificationService.Trigger(notification);
    }

    public static void Instant(string name, NotificationInfo info)
    {
      var notification = new Notification.Builder()
                              .WithName(name)
                              .WithLifeSpan(NotificationLifeSpan.Instant)
                              .WithInfo(info)
                              .Build();

      NotificationService.Trigger(notification);
    }
  }
}