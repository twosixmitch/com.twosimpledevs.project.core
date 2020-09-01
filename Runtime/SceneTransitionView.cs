using System;
using UnityEngine;

namespace TwoSimpleDevs.Project.Core
{
  public class SceneTransitionView : MonoBehaviour
  {
    public Animator Anim;

    private Action _callback;

    public void Start()
    {
      Anim.ResetTrigger("TransitionIn");
      Anim.ResetTrigger("TransitionOut");
    }

    public void TransitionIn()
    {
      Anim.SetTrigger("TransitionIn");
    }

    public void TransitionOut(Action callback)
    {
      _callback = callback;
      
      Anim.SetTrigger("TransitionOut");
    }

    public void OnTransitionIn()
    {
      // Completed the transition in
      Log.Debug("[SceneTransitionView] OnTranstionIn");

      Anim.ResetTrigger("TransitionIn");
      
      NotificationTrigger.Instant(AppEvent.SceneTransitionedIn);
    }

    public void OnTransitionOut()
    {
      // Completed the transition out
      Log.Debug("[SceneTransitionView] OnTranstionOut");

      Anim.ResetTrigger("TransitionOut");

      NotificationTrigger.Instant(AppEvent.SceneTransitionedOut);

      _callback?.Invoke();
    }
  }
}
