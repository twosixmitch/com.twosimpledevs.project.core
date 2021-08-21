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
    private string _nextScene;
    private string _currentScene;
    private ISceneContext _nextContext;
    private SceneTransitionController _transitionController;

    public void Update()
    {
      if (!_transitioning && !string.IsNullOrEmpty(_nextScene))
      {
        GoTo(_nextScene, _nextContext);
      }
    }

    public static void GoTo(string scene, ISceneContext context = null)
    {
      Instance.GoToInternal(scene, context);
    }

    private void GoToInternal(string scene, ISceneContext context)
    {
      if (_currentScene == null)
      {
        _currentScene = SceneManager.GetActiveScene().name;
      }

      if (_currentScene == scene)
      {
        return;
      }

      if (!_transitioning)
      {
        _transitioning = true;

        Instance.StartCoroutine(Instance.PerformTransition(scene, context));

        if (_nextScene == scene)
        {
          _nextScene = null;
          _nextContext = null;
        }
      }
      else
      {
        _nextScene = scene;
        _nextContext = context;
      }
    }

    private IEnumerator PerformTransition(string newScene, ISceneContext context)
    {
      string oldScene = _currentScene;

      if (LoggingEnabled)
      {
        Log.Debug($"[SceneService] Started loading: {newScene}");
      }

      // Play a transition out animation.
      yield return PerformTransitionOut();

      // Begin loading the new scene immediately in the background.
      AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);
      
      // Allow the new scene to popup in now when its ready.
      asyncLoad.allowSceneActivation = true;

      // Wait till it's done.
      while (!asyncLoad.isDone)
      {
        yield return null;
      }

      if (LoggingEnabled)
      {
        Log.Debug($"[SceneService] Finished loading: {newScene}");
      }

      // Scene now ready!
      _currentScene = newScene;
      
      // Activate the new scene.
      var newActiveScene = SceneManager.GetSceneByName(newScene);
      SceneManager.SetActiveScene(newActiveScene);

      // Unload the old scene.
      yield return UnloadScene(oldScene);

      // Notify the new scene's context handlers.
      var sceneBehaviours = FindSceneBehaviours(newActiveScene);
      sceneBehaviours.ForEach(h => h.OnSceneStart(context));

      // Play a transition in animation.
      yield return PerformTransitionIn();
      
      // Notify the new scene's context handlers.
      sceneBehaviours.ForEach(h => h.OnSceneTransitionInFinished());

      if (LoggingEnabled)
      {
        Log.Debug($"[SceneService] Finished transition from: {oldScene} to: {newScene}");
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