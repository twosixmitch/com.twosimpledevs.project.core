using System;
using UnityEngine;

namespace TwoSimpleDevs.Project.Core
{
  [Serializable]
  public partial class Notification : INotification
  {
    [SerializeField]
    private bool _consumable;

    [SerializeField]
    private string _name;

    [SerializeField]
    private NotificationLifeSpan _lifeSpan;

    [SerializeField]
    private NotificationState _state;

    [SerializeField]
    private DateTime _expirationDate;

    [SerializeField]
    private NotificationInfo _info;

    public bool Consumable { get { return _consumable; } }
    public string Name { get { return _name; } }
    public NotificationLifeSpan LifeSpan { get { return _lifeSpan; } }
    public NotificationState State { get { return _state; } }
    public NotificationInfo Info { get { return _info; } }
    public bool WasConsumed { get { return _state == NotificationState.Consumed; } }

    public bool ShouldSave
    { 
      get 
      {
        return _state != NotificationState.Consumed  
            && _lifeSpan == NotificationLifeSpan.TillConsumed 
            || (_lifeSpan == NotificationLifeSpan.Custom && DateProvider.Instance.LocalNow < _expirationDate);
      }
    }

    public Notification()
    {
    }

    public void Consume()
    {
      if (_consumable)
      {
        _state = NotificationState.Consumed;		
      }
    }
  }
}