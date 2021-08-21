namespace TSDevs
{
  public interface IEvent 
  {
    bool IsConsumable { get; }
    
    string Name { get; }
    
    EventLifespan Lifespan { get; }
    
    EventState State { get; }

    EventInfo Info { get; }

    bool IsHandled { get; }

    bool ShouldSave { get; }
    
    void Handle();
  }
}