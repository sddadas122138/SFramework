using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wxy.Tool
{
    public enum SoundManagerTracks { Sfx, Background, UI, Master, Other }
    public class SoundManager : SuperSingleton<SoundManager>
    {

        public SuperObjectPooler SingSoundPlay_SuperObjectPooler;
        private GameObject BackgroundSingSoundPlay_GameObjec;

        private async void DelayDestoyGameObject(GameObject a, float Time_S)
        {


            int Time_MS = ((int)(Time_S * 1000)) + 200;
            await UniTask.Delay(Time_MS);
            if (a != null)
            {
                a.SetActive(false);
            }




        }


        public float SfxVolume;
        public float BackgroundVolume;
        public float UIVolume;
        [Button]
        protected override void Awake()
        {
            base.Awake();
            SfxVolume = SuperSaveData.Get<float>("SfxVolume", 1);
            BackgroundVolume = SuperSaveData.Get<float>("BackgroundVolume", 1);
            UIVolume = SuperSaveData.Get<float>("UIVolume", 1);
        }

        [Button]
        public void SetSfxVolume(float value)
        {
            SfxVolume = value;
            SuperSaveData.Set("SfxVolume", SfxVolume);
        }
        [Button]
        public void SetBackgroundVolume(float value)
        {
            BackgroundVolume = value;
            if (BackgroundSingSoundPlay_GameObjec == null)
            {
                BackgroundSingSoundPlay_GameObjec = SingSoundPlay_SuperObjectPooler.GetPooledGameObject();
            }
            AudioSource AS = BackgroundSingSoundPlay_GameObjec.GetComponent<AudioSource>();
            if (AS != null)
            {
                AS.volume = BackgroundVolume;
            }
            SuperSaveData.Set("BackgroundVolume", BackgroundVolume);
        }
        [Button]
        public void SetUIVolume(float value)
        {
            UIVolume = value;
            SuperSaveData.Set("BackgroundVolume", BackgroundVolume);
        }

        [Button]
        public GameObject PlaySound(SoundManagerTracks SMT, AudioClip AC, bool Isloop = false, float Voluem = 1)
        {
            if (SMT == SoundManagerTracks.UI)
            {
                GameObject a = SingSoundPlay_SuperObjectPooler.GetPooledGameObject();
                AudioSource AS = a.GetComponent<AudioSource>();
                AS.clip = AC;
                AS.loop = Isloop;
                AS.volume = UIVolume * Voluem;
                a.SetActive(true);
                AS.Play();

                if (!Isloop)
                {
                    float currentTime = AC.length;
                    DelayDestoyGameObject(a, currentTime);
                }

                return a;
            }
            if (SMT == SoundManagerTracks.Sfx)
            {
                GameObject a = SingSoundPlay_SuperObjectPooler.GetPooledGameObject();
                AudioSource AS = a.GetComponent<AudioSource>();
                AS.clip = AC;
                AS.loop = Isloop;
                AS.volume = SfxVolume * Voluem;
                a.SetActive(true);
                AS.Play();



                if (!Isloop)
                {
                    float currentTime = AC.length;
                    DelayDestoyGameObject(a, currentTime);
                }

                return a;
            }

            if (SMT == SoundManagerTracks.Background)
            {
                if (BackgroundSingSoundPlay_GameObjec == null)
                {
                    BackgroundSingSoundPlay_GameObjec = SingSoundPlay_SuperObjectPooler.GetPooledGameObject();
                }

                AudioSource AS = BackgroundSingSoundPlay_GameObjec.GetComponent<AudioSource>();
                AS.loop = true;
                AS.clip = AC;
                AS.volume = BackgroundVolume * Voluem;
                BackgroundSingSoundPlay_GameObjec.SetActive(true);
                AS.Play();

                float currentTime = AC.length;
                Debug.Log(currentTime);
                return BackgroundSingSoundPlay_GameObjec;
                //  DelayDestoyGameObject(BackGroundSingSoundPlay_GameObjec, currentTime);
            }

            return null;

        }
    }
}