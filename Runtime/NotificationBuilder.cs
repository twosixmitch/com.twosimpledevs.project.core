using System;

namespace TwoSimpleDevs.Project.Core
{
  public partial class Notification
  {
    public class Builder
    {
      private bool _consumable = true; 
      private string _appEvent;
      private NotificationLifeSpan _lifeSpan;
      private DateTime _expirationDate;
      private NotificationInfo _info;
    
      public Builder IsConsumable(bool consumable)
      {
        _consumable = consumable;
        return this;
      }

      public Builder WithAppEvent(string appEvent)
      {
        _appEvent = appEvent;
        return this;
      }

      public Builder WithLifeSpan(NotificationLifeSpan lifeSpan)
      {
        _lifeSpan = lifeSpan;
        return this;
      }

      public Builder WithInfo(NotificationInfo info)
      {
        _info = info;
        return this;
      }

      public Builder WithExpirationDate(DateTime date)
      {
        _expirationDate = date;
        return this;
      }

      public INotification Build()
      {
        return new Notification()
        {
          _appEvent       = _appEvent,
          _lifeSpan       = _lifeSpan,
          _consumable     = _consumable,
          _expirationDate = _expirationDate,
          _info           = _info,
        };
      }
    }
  }
}
