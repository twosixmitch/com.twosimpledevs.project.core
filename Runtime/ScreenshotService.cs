using UnityEngine;
using UnityEngine.SceneManagement;

namespace TwoSimpleDevs.Project.Core
{
  public class ScreenshotService : SingletonBehaviour<ScreenshotService> 
  {
    public void Update()
    {
      #if UNITY_EDITOR
      if (Input.GetKeyDown(KeyCode.P))
      {
        var activeScene = SceneManager.GetActiveScene().name;
        var imageName = $"Screenshot-{activeScene}-{Screen.width}-{Screen.height}";
        var extension = ".png";

        var fullPath = System.IO.Path.Combine(Application.dataPath, imageName + extension);

        if (System.IO.File.Exists(fullPath))
        {
          int count = 0;
          while (System.IO.File.Exists(fullPath))
          {
            fullPath = System.IO.Path.Combine(Application.dataPath, imageName + $"({++count})" + extension);
          }
        }

        ScreenCapture.CaptureScreenshot(fullPath);
      }
      #endif
    }
  }
}