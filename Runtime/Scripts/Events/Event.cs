using System;
using UnityEngine;

namespace TSDevs
{
  [Serializable]
  public partial class Event : IEvent
  {
    [SerializeField]
    private bool _consumable;

    [SerializeField]
    private string _name;

    [SerializeField]
    private EventLifespan _lifespan;

    [SerializeField]
    private EventState _state;

    [SerializeField]
    private DateTime _expirationDate;

    [SerializeField]
    private EventInfo _info;

    public string        Name         { get { return _name; } }
    public EventLifespan Lifespan     { get { return _lifespan; } }
    public EventState    State        { get { return _state; } }
    public EventInfo     Info         { get { return _info; } }
    public bool          IsConsumable { get { return _consumable; } }
    public bool          IsHandled    { get { return _state == EventState.Handled; } }

    public bool ShouldSave
    { 
      get 
      {
        return _state != EventState.Handled  
            && _lifespan == EventLifespan.TillHandled 
            || (_lifespan == EventLifespan.Custom && DateProvider.Instance.LocalNow < _expirationDate);
      }
    }

    public Event()
    {
    }

    public void Handle()
    {
      if (_consumable)
      {
        _state = EventState.Handled;		
      }
    }
  }
}