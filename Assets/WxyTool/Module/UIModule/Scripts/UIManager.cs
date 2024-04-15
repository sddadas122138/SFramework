using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Wxy.Tool
{
    public class UIManager : SuperSingleton<UIManager>
    {
        //存储场景中的UI信息
        private Dictionary<string, UIBase> UI_Dictionary = new Dictionary<string, UIBase>();

        private int CurrentSort_int = 0;
        private GameObject a;

        public Camera UI_Camera;

        //当前字典中UI的个数
        public int UICount
        {
            get { return UI_Dictionary.Count; }
        }

        public void Init(Camera UI)
        {
            UI_Camera = UI;
        }

        private void Update()
        {
            if (UI_Dictionary.Count > 0)
            {
                foreach (var key in UI_Dictionary.Keys)
                {
                    if (UI_Dictionary[key] != null)
                    {

                        UI_Dictionary[key].Update();
                    }

                }
            }
        }

        /// <summary>
        /// 打开面板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public UIBase OpenUI<T>(UIData data=null) where T : UIBase, new()
        {
            if (a == null)
            {
                a = new GameObject("UI");
            }
            if (UI_Camera == null)
            {
                UI_Camera = Camera.main;
            }
            T t = new T();
            string PrefabsPath = t.PrefabsPath_string;
            Debug.Log(PrefabsPath);
            if (UI_Dictionary.ContainsKey(PrefabsPath))
            {
                {
                    Canvas canvas = UI_Dictionary[PrefabsPath].UI_GameObject.GetComponent<Canvas>();
                    CurrentSort_int++;
                    canvas.sortingOrder = CurrentSort_int;


                }
                UI_Dictionary[PrefabsPath]._UIData = data;
                UI_Dictionary[PrefabsPath].Open();
                return UI_Dictionary[PrefabsPath];
            }



            if (string.IsNullOrEmpty(t.PrefabsPath_string))
            {
                Debug.LogError("Prefabs 路径不能为空");
                return null;
            }

            GameObject prefabs = AAResources.LoadAsset<GameObject>(t.PrefabsPath_string);
            GameObject uiGameOjbect = GameObject.Instantiate(prefabs, a.transform);

            {
                Canvas canvas = uiGameOjbect.GetComponent<Canvas>();
                CurrentSort_int++;
                canvas.sortingOrder = CurrentSort_int;
                if (UI_Camera != null)
                {
                    canvas.worldCamera = UI_Camera;
                }

            }



            t.Init(uiGameOjbect);
            t._UIData = data;
            t.Open();


            UI_Dictionary.Add(PrefabsPath, t);
            return t;
        }

        /// <summary>
        /// 关闭面板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void CloseUI<T>() where T : UIBase,new()
        {
            T t = new T();
            string PrefabsPath = t.PrefabsPath_string;

            if (UI_Dictionary.ContainsKey(PrefabsPath))
            {
                UIBase uIBase = UI_Dictionary[PrefabsPath];
                uIBase.Close();



                return;
            }
            Debug.Log(string.Format("当前的UI物体未实例化，名字：{0}", PrefabsPath));
        }

        /// <summary>
        /// 关闭所有的UI物体
        /// </summary>
        public void CloseAllPanel()
        {
            foreach (var key in UI_Dictionary.Keys)
            {
                UIBase uIBase = UI_Dictionary[key];
                if (uIBase != null)
                {
                    uIBase.Close();

                }
            }


        }

        /// <summary>
        /// 找到指定的UI面板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameObject GetGameObject<T>(string name)
        {
            Type t = typeof(T);
            string fullName = t.FullName;

            UIBase uIBase = null;
            if (!UI_Dictionary.TryGetValue(fullName, out uIBase))
            {
                Debug.Log("没有找到对应的UI面板，名字：" + fullName);
                return null;
            }

            return uIBase.GetObject(name);
        }

        private UIManager() { }
    }

    public class UIBase
    {
        #region 字段

        /// <summary>
        /// Prefabs路径
        /// </summary>
        public string PrefabsPath_string;
        /// <summary>
        /// UI面板的名字
        /// </summary>
        public string UIName_string { get; set; }
        /// <summary>
        /// 当前UI所在的场景名
        /// </summary>
        public string SceneName_string { get; set; }

        /// <summary>
        /// 当前UI的游戏物体
        /// </summary>
        public GameObject UI_GameObject { get; set; }

        public bool IsOpen_bool;

       // public Feedbacks Open_Feedbacks;
        #endregion
        public UIData _UIData;
        /// <summary>
        /// 打开面板
        /// </summary>
        public virtual void Open() { UI_GameObject.SetActive(true);// Open_Feedbacks?.PlayFeedbacks(UI_GameObject);
                                                                   }
        /// <summary>
        /// 面板初始化
        /// </summary>
        public virtual void Init(GameObject uiGameOjbect) {

            UIName_string = uiGameOjbect.name;
            SceneName_string = SceneManager.GetActiveScene().name;
            UI_GameObject = uiGameOjbect;

            uiGameOjbect.name = PrefabsPath_string.Substring(PrefabsPath_string.LastIndexOf('/') + 1);
          //  Open_Feedbacks = GetOrAddCommonent<Feedbacks>("OpenFeedbacks");
        }

        /// <summary>
        /// 每帧执行
        /// </summary>
        public virtual void Update()
        {
            if (IsOpen_bool == false)
            {
                return;
            }
        }

        /// <summary>
        /// 关闭面板
        /// </summary>
        public virtual void Close() { UI_GameObject.gameObject.SetActive(false); }


        /// <summary>
        /// 根据名称查找一个子对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public GameObject GetObject(string name)
        {
            Transform[] trans = UI_GameObject.GetComponentsInChildren<Transform>();
            foreach (var item in trans)
            {
                if (item.name == name)
                    return item.gameObject;
            }

           
            return null;
        }

        /// <summary>
        /// 根据名称获取一个子对象的组件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetOrAddCommonent<T>(string name) where T : Component
        {
            GameObject child = GetObject(name);
            if (child)
            {
                if (child.GetComponent<T>() == null)
                    child.AddComponent<T>();
                return child.GetComponent<T>();
            }
            return null;
        }


        protected UIBase() { }
    }

    public class UIData{



        }
}