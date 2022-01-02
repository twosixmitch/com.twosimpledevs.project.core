using UnityEngine;

namespace TSDevs
{
  public class SceneBehaviour : MonoBehaviour
  {
    public virtual void OnSceneEnter(ISceneContext context)
    {
    }
    
    public virtual void OnSceneLeave()
    {
    }

    public virtual void OnSceneTransitionOutStarted()
    {
    }
    
    public virtual void OnSceneTransitionInFinished()
    {
    }
  }
}
