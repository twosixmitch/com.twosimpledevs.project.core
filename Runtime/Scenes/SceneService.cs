using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TwoSimpleDevs.Project.Core
{
  /*
    Handles scene transitions and passing information between scenes.
  */
  public class SceneService : SingletonBehaviour<SceneService> 
  {
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

        Instance.StartCoroutine(Instance.LoadYourAsyncScene(scene, context));

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

    private IEnumerator LoadYourAsyncScene(string newScene, ISceneContext context)
    {
      string oldScene = _currentScene;

      Log.Debug($"[AppService] LOADING: {newScene}, UNLOADING: {oldScene}");

      // Begin loading the new scene immediately in the background.
      AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);
      asyncLoad.allowSceneActivation = false;

      if (_transitionController != null)
      {
        // Play the white-out animation.
        yield return _transitionController.PerformTransitionOut();
      }

      // Allow the new scene to popup in now when its ready.
      asyncLoad.allowSceneActivation = true;

      // Just wait till it's done.
      while (!asyncLoad.isDone)
      {
        yield return null;
      }

      // Scene now ready!
      _currentScene = newScene;
      
      // Activate the new scene.
      var newActiveScene = SceneManager.GetSceneByName(newScene);
      SceneManager.SetActiveScene(newActiveScene);

      Log.Debug($"[AppService] ActiveScene: {newActiveScene.name}");

      // Perform the new scene transition.
      _transitionController = FindSceneTransitionController(newActiveScene);

      // Notify the new scene's context handlers.
      var handlers = FindSceneContextHandlers(newActiveScene);
      handlers.ForEach(h => h.OnSceneStart(context));

      // Unload the old scene.
      if (!string.IsNullOrEmpty(oldScene))
      {
        Log.Debug($"[AppService] UNLOADING: {oldScene}");

        // Unload the previous scene in the background.
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(oldScene);

        while (!asyncUnload.isDone)
        {
          yield return null;
        }
      }

      Log.Debug($"[AppService] DONE - LOADING: {newScene}, UNLOADING: {oldScene}");

      _transitioning = false;
    }

    private List<SceneContextBehaviour> FindSceneContextHandlers(Scene scene)
    {
      // Find the gameobjects with SceneContextBehaviour scripts in the scene.
      var handlerGameObjects = scene.GetRootGameObjects().Where(go => go.GetComponent<SceneContextBehaviour>() != null);

      var handlers = new List<SceneContextBehaviour>();

      // Generate a list of all the SceneContextBehaviour scripts in the scene.
      var handlerBehaviours = handlerGameObjects.Select(go => go.GetComponents<SceneContextBehaviour>());
      foreach (var behaviours in handlerBehaviours)
      {
        handlers.AddRange(behaviours);
      }
  
      return handlers;
    }

    private SceneTransitionController FindSceneTransitionController(Scene scene)
    {
      // Scan the root objects for the controller script.
      var transitionGO = scene.GetRootGameObjects().Where(go => go.GetComponent<SceneTransitionController>() != null);

      // Return the first controller we find.
      return transitionGO.Select(go => go.GetComponent<SceneTransitionController>()).FirstOrDefault();
    }
  }
}