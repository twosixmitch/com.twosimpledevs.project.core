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
    public override string DataPath
    {
      get {  return Application.persistentDataPath + "/notifications.dat"; }
    }

    private NotificationsList EmptyList
    {
      get { return new NotificationsList(); }
    }

    private NotificationsDictionary EmptyDict
    {
      get { return new NotificationsDictionary(); }
    }

    private NotificationsDictionary _notifications;
    private ListenersDictionary _listeners;

    public void OnApplicationQuit()
    {
      Save();
    }

    public override void InitFromData(object data)
    {
      _notifications = new NotificationsDictionary();
      _listeners = new ListenersDictionary();

      var notificationList = (NotificationsList)data;

      foreach (var notification in notificationList)
      {
        if (_notifications.ContainsKey(notification.AppEvent))
        {
          _notifications[notification.AppEvent].Add(notification);
        }
        else
        {
          _notifications.Add(notification.AppEvent, new NotificationsList(){ notification });
        }
      }
    }

    public override object GetDataToSave()
    {
      var result = new NotificationsList();

      foreach (var kvp in _notifications)
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
      foreach (var notificationGroup in _notifications)
      {
        notificationGroup.Value.RemoveAll(n => n.WasConsumed);
      }

      // Collect up the events with no notifications
      var events = _notifications.Where(n => n.Value.Count == 0).Select(n => n.Key).ToList();

      // Remove all empty events
      events.ForEach(e => _notifications.Remove(e));
    }

    public static void ListenFor(string appEvent, NotificationAction listeneer)
    {
      if (Instance == null) return;
        
      Instance.AddListener(appEvent, listeneer);
    }

    private void AddListener(string appEvent, NotificationAction listener)
    {
      NotificationUnityEvent listeners = null;

      if (_listeners.TryGetValue(appEvent, out listeners))
      {
        listeners.AddListener(listener);
      } 
      else
      {
        listeners = new NotificationUnityEvent();
        listeners.AddListener(listener);
        _listeners.Add(appEvent, listeners);
      }
    }

    public static void DoNotListenFor(string appEvent, NotificationAction listener)
    {
      if (Instance == null) return;

      Instance.RemoveListener(appEvent, listener);
    }

    private void RemoveListener(string appEvent, NotificationAction listener)
    {
      NotificationUnityEvent listeners = null;

      if (_listeners.TryGetValue(appEvent, out listeners))
      {
        listeners.RemoveListener(listener);
      }
    }

    public static NotificationsList CheckFor(string appEvent)
    {
      return Instance.GetNotificationsFor(appEvent);
    }

    private NotificationsList GetNotificationsFor(string appEvent)
    {
      if (_notifications.ContainsKey(appEvent) && _notifications[appEvent].Count > 0)
      {
        return _notifications[appEvent];
      }

      return EmptyList;
    }

    public static NotificationsDictionary CheckFor(List<string> appEvents)
    {
      return Instance.GetNotificationsFor(appEvents);
    }

    private NotificationsDictionary GetNotificationsFor(List<string> appEvents)
    {
      var result = EmptyDict;

      foreach (var appEvent in appEvents)
      {
        if (_notifications.ContainsKey(appEvent) && _notifications[appEvent].Count > 0)
        {
          result.Add(appEvent, _notifications[appEvent]);
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
        if (_notifications.ContainsKey(notification.AppEvent))
        {
          _notifications[notification.AppEvent].Add(notification);
        }
        else
        {
          _notifications.Add(notification.AppEvent, new NotificationsList() { notification });
        }
      }

      NotificationUnityEvent listeners = null;

      if (_listeners.TryGetValue(notification.AppEvent, out listeners))
      {
        // TODO: Invoke one at a time while not consumed.
        listeners.Invoke(notification);
      }
    }
  }
}