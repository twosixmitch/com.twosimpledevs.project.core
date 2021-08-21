using System;
using UnityEngine;

public abstract class SceneTransitionView : MonoBehaviour
{
  public abstract void TransitionIn(Action callback);
  public abstract void TransitionOut(Action callback);
}