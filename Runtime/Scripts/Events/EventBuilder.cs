using System;

namespace TSDevs
{
  public partial class Event
  {
    public class Builder
    {
      private string _name;
      private EventLifespan _lifespan;
      private DateTime _expirationDate;
      private EventInfo _info;
      private bool _consumable = true;
    
      public Builder WithName(string name)
      {
        _name = name;
        return this;
      }

      public Builder WithLifespan(EventLifespan lifespan)
      {
        _lifespan = lifespan;
        return this;
      }

      public Builder WithInfo(EventInfo info)
      {
        _info = info;
        return this;
      }

      public Builder WithExpirationDate(DateTime date)
      {
        _expirationDate = date;
        return this;
      }

      public Builder IsConsumable(bool consumable)
      {
        _consumable = consumable;
        return this;
      }

      public IEvent Build()
      {
        return new Event()
        {
          _name           = _name,
          _lifespan       = _lifespan,
          _consumable     = _consumable,
          _expirationDate = _expirationDate,
          _info           = _info,
        };
      }
    }
  }
}
