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
      Log.Debug("[SceneTransitionView] OnTranstionIn");

      Anim.ResetTrigger("TransitionIn");
      
      EventService.TriggerEvent(EventNames.SceneTransitionedIn);
    }

    public void OnTransitionOut()
    {
      Log.Debug("[SceneTransitionView] OnTranstionOut");

      Anim.ResetTrigger("TransitionOut");

      EventService.TriggerEvent(EventNames.SceneTransitionedOut);

      _callback?.Invoke();
    }
  }
}
