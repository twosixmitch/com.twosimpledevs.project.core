namespace TwoSimpleDevs.Project.Core
{
  public interface INotification 
  {
    bool Consumable { get; }
    
    string Name { get; }
    
    NotificationLifeSpan LifeSpan { get; }
    
    NotificationState State { get; }

    NotificationInfo Info { get; }

    bool WasConsumed { get; }

    bool ShouldSave { get; }
    
    void Consume();
  }
}