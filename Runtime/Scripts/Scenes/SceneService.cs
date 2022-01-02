using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TSDevs
{
  /// <summary>
  /// Handles scene transitions and passing information between scenes.
  /// </summary>
  public class SceneService : SingletonBehaviour<SceneService> 
  {
    public bool LoggingEnabled = true;

    private bool _transitioning;
    private string _nextSceneName;
    private string _currentSceneName;
    private ISceneContext _nextContext;
    private SceneTransitionController _transitionController;

    public void Update()
    {
      if (!_transitioning && !string.IsNullOrEmpty(_nextSceneName))
      {
        GoTo(_nextSceneName, _nextContext);
      }
    }

    public static void GoTo(string sceneName, ISceneContext context = null)
    {
      Instance.GoToInternal(sceneName, context);
    }

    private void GoToInternal(string sceneName, ISceneContext context)
    {
      if (_currentSceneName == null)
      {
        _currentSceneName = SceneManager.GetActiveScene().name;
      }

      if (_currentSceneName == sceneName)
      {
        return;
      }

      if (!_transitioning)
      {
        _transitioning = true;

        Instance.StartCoroutine(Instance.PerformTransition(sceneName, context));

        if (_nextSceneName == sceneName)
        {
          _nextSceneName = null;
          _nextContext = null;
        }
      }
      else
      {
        _nextSceneName = sceneName;
        _nextContext = context;
      }
    }

    private IEnumerator PerformTransition(string newSceneName, ISceneContext context)
    {
      string oldSceneName = _currentSceneName;

      if (LoggingEnabled)
      {
        Log.Debug($"[SceneService] Started loading: {newSceneName}");
      }
      
      var oldScene = SceneManager.GetSceneByName(oldSceneName);

      var oldSceneBehaviours = FindSceneBehaviours(oldScene);
      oldSceneBehaviours.ForEach(h => h.OnSceneTransitionOutStarted());
     
      // Play a transition out animation.
      yield return PerformTransitionOut();

      oldSceneBehaviours.ForEach(h => h.OnSceneLeave());

      // Begin loading the new scene immediately in the background. (TODO: Do this during the transition out)
      AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(newSceneName, LoadSceneMode.Additive);
      
      // Allow the new scene to popup in now when its ready.
      asyncLoad.allowSceneActivation = true;

      // Wait till it's done.
      while (!asyncLoad.isDone)
      {
        yield return null;
      }

      if (LoggingEnabled)
      {
        Log.Debug($"[SceneService] Finished loading: {newSceneName}");
      }

      // Scene now ready!
      _currentSceneName = newSceneName;
      
      // Activate the new scene.
      var newActiveScene = SceneManager.GetSceneByName(newSceneName);
      SceneManager.SetActiveScene(newActiveScene);

      // Unload the old scene.
      yield return UnloadScene(oldSceneName);

      // Notify the new scene's context handlers.
      var newSceneBehaviours = FindSceneBehaviours(newActiveScene);
      newSceneBehaviours.ForEach(h => h.OnSceneEnter(context));

      // Play a transition in animation.
      yield return PerformTransitionIn();
      
      // Notify the new scene's context handlers.
      newSceneBehaviours.ForEach(h => h.OnSceneTransitionInFinished());

      if (LoggingEnabled)
      {
        Log.Debug($"[SceneService] Finished transition from: {oldSceneName} to: {newSceneName}");
      }

      _transitioning = false;
    }

    private IEnumerator PerformTransitionOut()
    {
      var transitionController = GetSceneTransitionController();

      if (transitionController != null)
      {
        yield return transitionController.PerformTransitionOut();
      }
    }

    private IEnumerator PerformTransitionIn()
    {
      var transitionController = GetSceneTransitionController();

      if (transitionController != null)
      {
        yield return transitionController.PerformTransitionIn();
      }
    }

    private IEnumerator UnloadScene(string sceneName)
    {
      if (!string.IsNullOrEmpty(sceneName))
      {
        if (LoggingEnabled)
        {
          Log.Debug($"[SceneService] Started unloading: {sceneName}");
        }

        // Unload the scene in the background.
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneName);

        while (!asyncUnload.isDone)
        {
          yield return null;
        }

        if (LoggingEnabled)
        {
          Log.Debug($"[SceneService] Finished unloading: {sceneName}");
        }
      }

      yield return null;
    }

    private List<SceneBehaviour> FindSceneBehaviours(Scene scene)
    {
      // Find the gameobjects with SceneBehaviour scripts in the scene.
      var gameObjects = scene.GetRootGameObjects().Where(go => go.GetComponent<SceneBehaviour>() != null);

      // Generate a list of all the SceneBehaviour scripts in the scene.
      return gameObjects.Select(go => go.GetComponent<SceneBehaviour>()).ToList();
    }

    private SceneTransitionController GetSceneTransitionController()
    {
      if (_transitionController == null)
      {
        _transitionController = gameObject.GetComponentInChildren(typeof(SceneTransitionController)) as SceneTransitionController;
      }

      return _transitionController;
    }
  }
}