using System;
using UnityEngine.Events;

namespace TwoSimpleDevs.Project.Core
{
  [Serializable]
  public class NotificationUnityEvent : UnityEvent<INotification>
  {
  }
}