using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using EventAction         = UnityEngine.Events.UnityAction<TwoSimpleDevs.Project.Core.IEvent>;
using EventsList          = System.Collections.Generic.List<TwoSimpleDevs.Project.Core.IEvent>;
using EventsDictionary    = System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<TwoSimpleDevs.Project.Core.IEvent>>;
using ListenersDictionary = System.Collections.Generic.Dictionary<string, TwoSimpleDevs.Project.Core.TSDUnityEvent>;

namespace TwoSimpleDevs.Project.Core
{
  public class EventService : ServiceSingletonBase<EventService> 
  {
    private ListenersDictionary _listeners;
    private EventsDictionary _events;

    private ListenersDictionary Listeners 
    {
      get
      {
        if (_listeners == null)
        {
          _listeners = new ListenersDictionary();
        }

        return _listeners;
      }
    }

    private EventsDictionary Events 
    {
      get
      {
        if (_events == null)
        {
          _events = new EventsDictionary();
        }

        return _events;
      }
    }

    public override string DataPath
    {
      get {  return Application.persistentDataPath + "/event_service.dat"; }
    }

    public void OnApplicationQuit()
    {
      Save();
    }

    public override void Initialize(object data)
    {
      if (data == null)
      {
        return;
      }

      var eventList = (EventsList)data;

      foreach (var savedEvent in eventList)
      {
        if (Events.ContainsKey(savedEvent.Name))
        {
          Events[savedEvent.Name].Add(savedEvent);
        }
        else
        {
          Events.Add(savedEvent.Name, new EventsList() { savedEvent });
        }
      }
    }

    public override object Serialize()
    {
      var result = new EventsList();

      foreach (var kvp in Events)
      {
        foreach (var currentEvent in kvp.Value)
        {
          if (currentEvent.ShouldSave)
          {
            result.Add(currentEvent);
          }
        }
      }

      return result;
    }

    public static void Refresh()
    {
      if (Instance == null) return;

      Instance.RefreshInternal();
    }

    private void RefreshInternal()
    {
      // Clear out handled events
      foreach (var eventGroup in Events)
      {
        eventGroup.Value.RemoveAll(n => n.IsHandled);
      }

      // Collect up the groups with no active events
      var eventGroups = Events.Where(n => n.Value.Count == 0).Select(n => n.Key).ToList();

      // Remove all empty events groups
      eventGroups.ForEach(e => Events.Remove(e));
    }

    public static void AddListener(string name, EventAction action)
    {
      if (Instance == null) return;
        
      Instance.AddListenerInternal(name, action);
    }

    private void AddListenerInternal(string name, EventAction action)
    {
      TSDUnityEvent unityEvent = null;

      if (Listeners.TryGetValue(name, out unityEvent))
      {
        unityEvent.AddListener(action);
      } 
      else
      {
        unityEvent = new TSDUnityEvent();
        unityEvent.AddListener(action);
        Listeners.Add(name, unityEvent);
      }
    }

    public static void RemoveListener(string name, EventAction action)
    {
      if (Instance == null) return;

      Instance.RemoveListenerInternal(name, action);
    }

    private void RemoveListenerInternal(string name, EventAction action)
    {
      TSDUnityEvent unityEvent = null;

      if (Listeners.TryGetValue(name, out unityEvent))
      {
        unityEvent.RemoveListener(action);
      }
    }

    public static EventsList GetEventsFor(string name)
    {
      if (Instance == null) return new EventsList();;

      return Instance.GetEventsForInternal(name);
    }

    private EventsList GetEventsForInternal(string name)
    {
      if (Events.ContainsKey(name) && Events[name].Count > 0)
      {
        return Events[name];
      }

      return new EventsList();
    }

    public static EventsDictionary GetEventsForAll(List<string> names)
    {
      if (Instance == null) return new EventsDictionary();

      return Instance.GetEventsForAllInternal(names);
    }

    private EventsDictionary GetEventsForAllInternal(List<string> names)
    {
      var result = new EventsDictionary();

      foreach (var name in names)
      {
        if (Events.ContainsKey(name) && Events[name].Count > 0)
        {
          result.Add(name, Events[name]);
        }
      }

      return result;
    }

    public static void TriggerEvent(string name, EventLifespan lifespan = EventLifespan.Instant)
    {
      if (Instance == null) return;
      
      var newEvent = new Event.Builder().WithName(name).WithLifespan(lifespan).Build();

      Instance.TriggerEventInternal(newEvent);
    }

    public static void TriggerEvent(string name, EventInfo info, EventLifespan lifespan = EventLifespan.Instant)
    {
      if (Instance == null) return;

      var newEvent = new Event.Builder().WithName(name).WithLifespan(lifespan).WithInfo(info).Build();

      Instance.TriggerEventInternal(newEvent);
    }

    public static void TriggerEvent(IEvent triggeredEvent)
    {
      if (Instance == null) return;

      Instance.TriggerEventInternal(triggeredEvent);
    }

    private void TriggerEventInternal(IEvent triggeredEvent)
    {
      Log.Debug($"[EventService] Triggered event for {triggeredEvent.Name} with lifespan of {triggeredEvent.Lifespan}");

      if (triggeredEvent.Lifespan != EventLifespan.Instant)
      {
        if (Events.ContainsKey(triggeredEvent.Name))
        {
          Events[triggeredEvent.Name].Add(triggeredEvent);
        }
        else
        {
          Events.Add(triggeredEvent.Name, new EventsList() { triggeredEvent });
        }
      }

      TSDUnityEvent unityEvent = null;

      if (Listeners.TryGetValue(triggeredEvent.Name, out unityEvent))
      {
        if (!triggeredEvent.IsHandled)
        {
          unityEvent.Invoke(triggeredEvent);
        }
      }
    }
  }
}