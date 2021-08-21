using UnityEngine;

namespace TSDevs
{
    public class TweenRX : TweenVec1R
    {
        public static TweenRX Add(GameObject g, float duration)
        {
            return Add<TweenRX>(g, duration);
        }

        public static TweenRX Add(GameObject g, float duration, float to)
        {
            return Add<TweenRX>(g, duration, to);
        }

        protected override float Value
        {
            get { return Vector.x; }
            set
            {
                var v = Vector;
                v.x = value;
                Vector = v;
            }
        }
    }
}