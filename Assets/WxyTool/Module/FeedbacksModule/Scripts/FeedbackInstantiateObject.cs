using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Wxy.Tool
{
    public class FeedbackInstantiateObject : Feedback
    {


        private SuperObjectPooler InstantiateObject_SuperObjectPooler;

        public List< GameObject> InstantiateGameObject_List;
        public string SuperObjectPoolerName_string;

        public bool isParent=false;
        public int PoolSize_int = 5;

        public override void Init(Feedbacks Owner)
        {
            base.Init(Owner);

           
        }
       

        public override void PlayFeedback(int value)
        {
            base.PlayFeedback(value);
            if (InstantiateObject_SuperObjectPooler == null) {

                InstantiateObject_SuperObjectPooler = SuperObjectPooler.GetSuperObjectPooler(InstantiateGameObject_List, gameObject, SuperObjectPoolerName_string, PoolSize_int);

            }
            GameObject a = InstantiateObject_SuperObjectPooler.GetPooledGameObject();
            a.transform.position = transform.position;
            if (isParent)
            {
                a.transform.parent = transform;
                a.transform.localScale = transform.lossyScale;
                a.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            a.transform.localScale = transform.lossyScale;
            a.SetActive(true);
        }
    }
}