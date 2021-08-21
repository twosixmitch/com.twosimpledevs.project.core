using System.Collections;
using UnityEngine;

namespace TSDevs
{
  public class SceneTransitionController : MonoBehaviour
  {
    public GameObject TransitionContainer;
    public GameObject FadeTransitionPrefab;
    public bool LoggingEnabled = true;

    private bool _completedTransition;
    private SceneTransitionView _transitionView;

    public IEnumerator PerformTransitionOut()
    {
      _completedTransition = false;

      var transitionView = GetTransitionView();

      if (transitionView != null)
      {
        if (LoggingEnabled)
        {
          Log.Debug($"[SceneTransitionController] Performing transition out");
        }

        transitionView.TransitionOut(() =>
        { 
          _completedTransition = true;

          if (LoggingEnabled)
          {
            Log.Debug($"[SceneTransitionController] Finished transition out");
          }
        });

        yield return new WaitUntil(() => _completedTransition);
      }
      else
      {
        if (LoggingEnabled)
        {
          Log.Warning("[SceneTransitionController] Cannot perform transition out - no transition view");
        }
        yield return null;
      }
    }

    public IEnumerator PerformTransitionIn()
    {
      _completedTransition = false;

      var transitionView = GetTransitionView();

      if (transitionView != null)
      {
        if (LoggingEnabled)
        {
          Log.Debug($"[SceneTransitionController] Started transition in");
        }

        transitionView.TransitionIn(() =>
        { 
          _completedTransition = true;
          
          if (LoggingEnabled)
          {
            Log.Debug($"[SceneTransitionController] Finished transition in");
          }
        });

        yield return new WaitUntil(() => _completedTransition);

        DestroyTransitionView();
      }
      else
      {
        if (LoggingEnabled)
        {
          Log.Warning("[SceneTransitionController] Cannot perform transition in - no transition view");
        }

        yield return null;
      }
    }

    private SceneTransitionView GetTransitionView()
    {
      if (_transitionView == null)
      {
        var gameObject = Instantiate(FadeTransitionPrefab, TransitionContainer.transform);
        _transitionView = gameObject.GetComponent<SceneTransitionView>();
      }

      return _transitionView;
    }

    private void DestroyTransitionView()
    {
      if (_transitionView != null)
      {
        Destroy(_transitionView.gameObject);
        _transitionView = null;
      }
    }
  }
}
