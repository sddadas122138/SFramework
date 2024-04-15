using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Wxy.Tool
{
    public class FeedbackSound : Feedback
    {
        public float Voluem_float=1;
        public AudioClip AC;
        public SoundManagerTracks _SoundManagerTracks = SoundManagerTracks.Sfx;
        public override void PlayFeedback(int value)
        {
            base.PlayFeedback(value);
            if (AC == null)
            {
                return;
            }
            SoundManager.Instance.PlaySound(_SoundManagerTracks, AC, false,Voluem_float);
        }
    }
}