using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

using NotificationAction      = UnityEngine.Events.UnityAction<TwoSimpleDevs.Project.Core.INotification>;
using NotificationsList       = System.Collections.Generic.List<TwoSimpleDevs.Project.Core.INotification>;
using NotificationsDictionary = System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<TwoSimpleDevs.Project.Core.INotification>>;
using ListenersDictionary     = System.Collections.Generic.Dictionary<string, TwoSimpleDevs.Project.Core.NotificationUnityEvent>;

namespace TwoSimpleDevs.Project.Core
{
  public class NotificationService : ServiceSingletonBase<NotificationService> 
  {
    private ListenersDictionary _listeners;
    private NotificationsDictionary _notifications;

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

    private NotificationsDictionary Notifications 
    {
      get
      {
        if (_notifications == null)
        {
          _notifications = new NotificationsDictionary();
        }

        return _notifications;
      }
    }

    public override string DataPath
    {
      get {  return Application.persistentDataPath + "/notifications.dat"; }
    }

    public void OnApplicationQuit()
    {
      Save();
    }

    public override void InitFromData(object data)
    {
      if (data == null)
      {
        return;
      }

      var notificationList = (NotificationsList)data;

      foreach (var notification in notificationList)
      {
        if (Notifications.ContainsKey(notification.AppEvent))
        {
          Notifications[notification.AppEvent].Add(notification);
        }
        else
        {
          Notifications.Add(notification.AppEvent, new NotificationsList()
          {
            notification 
          });
        }
      }
    }

    public override object GetDataToSave()
    {
      var result = new NotificationsList();

      foreach (var kvp in Notifications)
      {
        foreach (var notification in kvp.Value)
        {
          if (notification.ShouldSave)
          {
            result.Add(notification);
          }
        }
      }

      return result;
    }

    public static void Refresh()
    {
      Instance.RefreshInternal();
    }

    private void RefreshInternal()
    {
      // Clear out consumed notifications
      foreach (var notificationGroup in Notifications)
      {
        notificationGroup.Value.RemoveAll(n => n.WasConsumed);
      }

      // Collect up the events with no notifications
      var events = Notifications.Where(n => n.Value.Count == 0).Select(n => n.Key).ToList();

      // Remove all empty events
      events.ForEach(e => Notifications.Remove(e));
    }

    public static void ListenFor(string appEvent, NotificationAction action)
    {
      if (Instance == null) return;
        
      Instance.AddListener(appEvent, action);
    }

    private void AddListener(string appEvent, NotificationAction action)
    {
      NotificationUnityEvent notificationEvent = null;

      if (Listeners.TryGetValue(appEvent, out notificationEvent))
      {
        notificationEvent.AddListener(action);
      } 
      else
      {
        notificationEvent = new NotificationUnityEvent();
        notificationEvent.AddListener(action);
        Listeners.Add(appEvent, notificationEvent);
      }
    }

    public static void DoNotListenFor(string appEvent, NotificationAction action)
    {
      if (Instance == null) return;

      Instance.RemoveListener(appEvent, action);
    }

    private void RemoveListener(string appEvent, NotificationAction action)
    {
      NotificationUnityEvent notificationEvent = null;

      if (Listeners.TryGetValue(appEvent, out notificationEvent))
      {
        notificationEvent.RemoveListener(action);
      }
    }

    public static NotificationsList CheckFor(string appEvent)
    {
      return Instance.GetNotificationsFor(appEvent);
    }

    private NotificationsList GetNotificationsFor(string appEvent)
    {
      if (Notifications.ContainsKey(appEvent) && Notifications[appEvent].Count > 0)
      {
        return Notifications[appEvent];
      }

      return new NotificationsList();
    }

    public static NotificationsDictionary CheckFor(List<string> appEvents)
    {
      return Instance.GetNotificationsFor(appEvents);
    }

    private NotificationsDictionary GetNotificationsFor(List<string> appEvents)
    {
      var result = new NotificationsDictionary();

      foreach (var appEvent in appEvents)
      {
        if (Notifications.ContainsKey(appEvent) && Notifications[appEvent].Count > 0)
        {
          result.Add(appEvent, Notifications[appEvent]);
        }
      }

      return result;
    }

    public static void Trigger(INotification notification)
    {
      Instance.TriggerNotification(notification);
    }

    private void TriggerNotification(INotification notification)
    {
      Log.Debug($"[NotificationService] Triggered notification for {notification.AppEvent} with lifespan of {notification.LifeSpan}");

      if (notification.LifeSpan != NotificationLifeSpan.Instant)
      {
        if (Notifications.ContainsKey(notification.AppEvent))
        {
          Notifications[notification.AppEvent].Add(notification);
        }
        else
        {
          Notifications.Add(notification.AppEvent, new NotificationsList()
          { 
            notification 
          });
        }
      }

      NotificationUnityEvent notificationEvent = null;

      if (Listeners.TryGetValue(notification.AppEvent, out notificationEvent))
      {
        // TODO: Invoke one at a time while not consumed.
        notificationEvent.Invoke(notification);
      }
    }
  }
}