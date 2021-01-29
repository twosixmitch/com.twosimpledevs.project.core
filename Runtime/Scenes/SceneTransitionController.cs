using System.Collections;
using UnityEngine;

namespace TSDevs
{
  public class SceneTransitionController : MonoBehaviour
  {
    public SceneTransitionView TransitionView;

    private bool _completedTransition;

    public void PerformTransitionIn()
    {
      if (TransitionView != null)
      {
        Log.Debug("[SceneTransitionController] Performing Transition In");
        TransitionView.TransitionIn();
      }
      else
      {
        Log.Warning("[SceneTransitionController] Cannot perform Transition In anim - TransitionView is NULL");
      }
    }

    public IEnumerator PerformTransitionOut()
    {
      _completedTransition = false;

      if (TransitionView != null)
      {
        Log.Debug("[SceneTransitionController] Performing Transition Out");

        TransitionView.TransitionOut(() =>
        { 
          _completedTransition = true; 
        });

        yield return new WaitUntil(() => _completedTransition);
      }
      else
      {
        Log.Warning("[SceneTransitionController] Cannot perform Transition Out anim - TransitionView is NULL");
        yield return null;
      }
    }
  }
}
