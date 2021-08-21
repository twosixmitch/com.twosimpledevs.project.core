using System;
using UnityEngine;
using UnityEngine.UI;

namespace TSDevs
{
  public class SceneFadeTransitionView : SceneTransitionView
  {
    public GameObject Content;
    public GameObject Image;
    public float FadeInDuration = 0.5f;
    public float FadeOutDuration = 0.5f;

    public override void TransitionIn(Action callback)
    {
      SetImageAlpha(Image, 1f);

      TweenA.Add(Image, FadeInDuration, 0f).Then(() => { callback(); });
    }

    public override void TransitionOut(Action callback)
    {
      SetImageAlpha(Image, 0f);

      TweenA.Add(Image, FadeOutDuration, 1f).Then(() => { callback(); });
    }

    private void SetImageAlpha(GameObject image, float value)
    {
      var graphic = image.GetComponent<Graphic>();
      var color = graphic.color;
      color.a = value;
      graphic.color = color;
    }
  }
}
