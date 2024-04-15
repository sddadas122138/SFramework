using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Wxy.Tool
{
    public class SuperPoolableObject : MonoBehaviour
    {
        public float LifeTime_float;


        public void OnEnable()
        {if(LifeTime_float!=0)
            DelayDestoyGameObject(LifeTime_float);
        }

        private async void DelayDestoyGameObject(float Time_S)
        {


            int Time_MS = (int)LifeTime_float * 1000 + 200;
            await UniTask.Delay(Time_MS);
            if (gameObject != null)
            {
                gameObject.SetActive(false);
            }




        }
    }
}