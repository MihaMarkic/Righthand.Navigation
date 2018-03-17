using System;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Widget;
using Java.Interop;

namespace Righthand.Navigation.Sample.Droid
{
    /// <summary>
    /// Animatable LinearLayout per fraction of TranslateX
    /// </summary>
    /// <remarks>Based on http://trickyandroid.com/fragments-translate-animation/</remarks>
    public class NLinearLayout : LinearLayout
    {
        #region Constructors
        public NLinearLayout(Context context) : base(context)
        {
        }

        public NLinearLayout(Context context, IAttributeSet attrs) : base(context, attrs)
        {
        }

        public NLinearLayout(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
        {
        }

        public NLinearLayout(Context context, IAttributeSet attrs, int defStyleAttr, int defStyleRes) : base(context, attrs, defStyleAttr, defStyleRes)
        {
        }

        protected NLinearLayout(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }
        #endregion
        [Export("getTranslateXFraction")]
        public float GetTranslateXFraction()
        {
            return Height > 0 ? TranslationX / Height : 0;
        }
        [Export("setTranslateXFraction")]
        public void SetTranslateXFraction(float value)
        {
            TranslationX = value * Height;
        }
    }
}
