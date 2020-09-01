using System.Collections;
using UnityEngine;

namespace TwoSimpleDevs.Project.Core
{
  public class SceneTransitionController : MonoBehaviour
  {
    public SceneTransitionView TransitionView;

    private bool _completedTransition;

    public void PerformTransitionIn()
    {
      Log.Debug("[SceneTransitionController] PerformTransitionIn");

      if (TransitionView != null)
      {
        TransitionView.TransitionIn();
      }
    }

    public IEnumerator PerformTransitionOut()
    {
      Log.Debug("[SceneTransitionController] PerformTransitionOut");
      
      _completedTransition = false;

      if (TransitionView != null)
      {
        TransitionView.TransitionOut(() =>
        { 
          _completedTransition = true; 
        });

        yield return new WaitUntil(() => _completedTransition);
      }
      else
      {
        yield return null;
      }
    }
  }
}
