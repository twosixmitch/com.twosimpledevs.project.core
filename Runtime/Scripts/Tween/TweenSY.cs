using UnityEngine;

namespace TSDevs
{
    public class TweenSY : TweenVec1S
    {
        public static TweenSY Add(GameObject g, float duration)
        {
            return Add<TweenSY>(g, duration);
        }

        public static TweenSY Add(GameObject g, float duration, float to)
        {
            return Add<TweenSY>(g, duration, to);
        }

        protected override float Value
        {
            get { return Vector.y; }
            set
            {
                var v = Vector;
                v.y = value;
                Vector = v;
            }
        }
    }
}