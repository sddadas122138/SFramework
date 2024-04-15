using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Wxy.Tool
{

    public class Feedbacks : MonoBehaviour
    {
        public List<Feedback> Feedback_List;
        [ReadOnly]
        public GameObject Owner_GameObject;
     
        public bool IsInit_bool = false;
        public void Init(GameObject owner)
        {
            
            Owner_GameObject = owner;
            InitFeedback_List();
            IsInit_bool = true;
        }
        public  void InitFeedback_List()
        {
            Feedback[] FD = gameObject.GetComponents<Feedback>();
            Feedback_List.AddRange(FD);

          
        }
        [Button]
        public  void PlayFeedbacks(GameObject owner=null,int Value=1)
        {
            if (IsInit_bool == false)
            {
                 Init(owner);
            }
           

            for (int i = 0; i < Feedback_List.Count; i++)
            {
                Feedback_List[i].Play(this,Value);
            }
        } [Button]
        public  void PlayFeedbacks(int Value=1)
        {
            PlayFeedbacks(null, Value);
        }
        [Button]
        public void StopFeedbacks()
        {
          
            for (int i = 0; i < Feedback_List.Count; i++)
            {
                Feedback_List[i].Stop();
            }
        }
    }
}