﻿namespace TwoSimpleDevs.Project.Core
{
  public interface INotification 
  {
    bool Consumable { get; }
    
    string AppEvent { get; }
    
    NotificationLifeSpan LifeSpan { get; }
    
    NotificationState State { get; }

    NotificationInfo Info { get; }

    bool WasConsumed { get; }

    bool ShouldSave { get; }
    
    void Consume();
  }
}