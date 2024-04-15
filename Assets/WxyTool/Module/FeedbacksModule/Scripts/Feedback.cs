using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Wxy.Tool
{


public class Feedback : MonoBehaviour
{[ReadOnly]
    public Feedbacks Owner_Feedbacks;

        public float DeleyTime_float=0;

        public bool IsInit_bool = false;
    public virtual void Init(Feedbacks Owner)
    {
        Owner_Feedbacks = Owner;
            IsInit_bool = true;
           
        }

        public  virtual void  PlayFeedback(int value)
        {
            
        }
        public async  void _PlayAsync(int value)
        {
            int DeleyTime_Ms = (int)(DeleyTime_float * 1000);
            await UniTask.Delay(DeleyTime_Ms);
            PlayFeedback(value);
        }

        public virtual void Play(Feedbacks Owner, int value)
        {
          
            if (!IsInit_bool)
            {
                Init(Owner);
            }
           
            if (DeleyTime_float == 0)
            {
                PlayFeedback(value);
                return;
            }
            _PlayAsync(value);
        }


        public  virtual void Stop()
        {
           
        }
}
}