
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using Sirenix.OdinInspector;

namespace Wxy.Tool
{
    [System.Serializable]
    public class SuperObjectPoolerObject
    {
        public GameObject GameObjectToPool;
        public int Weight = 1;
        [HideInInspector]
        public SuperObjectPool _objectPool;

    }
    [System.Serializable]
    public class SuperObjectPooler : MonoBehaviour
    {
        public List<SuperObjectPoolerObject> GameObjectsToPool;

        public string PoolName;
        public int PoolSize = 5;
        private bool isInit = false;
        public static List<SuperObjectPool> _pools = new List<SuperObjectPool>();

        public static bool _onSceneLoadedRegistered = false;
        static SuperObjectPooler()
        {
            if (!_onSceneLoadedRegistered)
            {
                SceneManager.sceneLoaded += OnSceneLoaded;
                _onSceneLoadedRegistered = true;


            }
        }
        [Button]
        public  void AddGameObjectsToPool(GameObject a)
        {
            SuperObjectPoolerObject superObjectPoolerObject = new SuperObjectPoolerObject();
            superObjectPoolerObject.GameObjectToPool = a;
            GameObjectsToPool.Add(superObjectPoolerObject);
            isInit = false;
            Init();
        }
        public static SuperObjectPooler GetSuperObjectPooler(GameObject gameObjectToPool, GameObject Owner, string PoolName = "", int PoolSize = 0)
        {
            SuperObjectPooler SOP = Owner.AddComponent<SuperObjectPooler>();
            SuperObjectPoolerObject SOPO = new SuperObjectPoolerObject
            {
                GameObjectToPool = gameObjectToPool
            };
            SOP.GameObjectsToPool = new List<SuperObjectPoolerObject>();
            SOP.GameObjectsToPool.Add(SOPO);
            SOP.PoolName = PoolName;
            SOP.PoolSize = PoolSize;
            SOP.Init();
            return SOP;
        } public static SuperObjectPooler GetSuperObjectPooler(List<SuperObjectPoolerObject> SuperObjectPoolerObject_List, GameObject Owner, string PoolName = "", int PoolSize = 0)
        {
            SuperObjectPooler SOP = Owner.AddComponent<SuperObjectPooler>();

            SOP.GameObjectsToPool = SuperObjectPoolerObject_List;


            SOP.PoolName = PoolName;
            SOP.PoolSize = PoolSize;
            SOP.Init();
            return SOP;
        }public static SuperObjectPooler GetSuperObjectPooler(List<GameObject> gameObjectToPool, GameObject Owner, string PoolName = "", int PoolSize = 0)
        {
            SuperObjectPooler SOP = Owner.AddComponent<SuperObjectPooler>();

            for (int i=0;i< gameObjectToPool.Count; i++)
            {
                SuperObjectPoolerObject SOPO = new SuperObjectPoolerObject
                {
                    GameObjectToPool = gameObjectToPool[i]
                };
                SOP.GameObjectsToPool = new List<SuperObjectPoolerObject>();
                SOP.GameObjectsToPool.Add(SOPO);
            }
           
            SOP.PoolName = PoolName;
            SOP.PoolSize = PoolSize;
            SOP.Init();
            return SOP;
        }
        public static void AddPool(SuperObjectPool pool)
        {
            if (_pools == null)
            {
                _pools = new List<SuperObjectPool>();
            }
            if (!_pools.Contains(pool))
            {
                _pools.Add(pool);
            }
        }


        public static void RemovePool(SuperObjectPool pool)
        {
            _pools?.Remove(pool);
        }

      


        public static SuperObjectPool ExistingPool(string poolName)
        {
            if (_pools == null)
            {
                _pools = new List<SuperObjectPool>();
            }

            if (_pools.Count == 0)
            {
                var pools = FindObjectsOfType<SuperObjectPool>();
                if (pools.Length > 0)
                {
                    _pools.AddRange(pools);
                }
            }
            foreach (SuperObjectPool pool in _pools)
            {
                if ((pool != null) && (pool.name == poolName)/* && (pool.gameObject.scene == this.gameObject.scene)*/)
                {
                    return pool;
                }
            }
            return null;
        }
        protected virtual void CreateWaitingPool(SuperObjectPoolerObject superObjectPoolerObject)
        {
            SuperObjectPool waitingPool = ExistingPool(DetermineObjectPoolName(superObjectPoolerObject.GameObjectToPool));

            if (waitingPool != null)
            {
                superObjectPoolerObject._objectPool = waitingPool;
            }
            else
            {
                GameObject newPool = new GameObject();
                newPool.name = DetermineObjectPoolName(superObjectPoolerObject.GameObjectToPool);
                superObjectPoolerObject._objectPool = newPool.AddComponent<SuperObjectPool>();
                superObjectPoolerObject._objectPool.PooledGameObjects = new List<GameObject>();
                AddPool(superObjectPoolerObject._objectPool);
            }
        }


        public string DetermineObjectPoolName(GameObject gameObjectToPool)
        {
            if (PoolName == null || PoolName == "")
            {
                return ($"[SuperObjectPooler]{gameObjectToPool.name}");
            }
            return ($"[SuperObjectPooler][{PoolName}]{gameObjectToPool.name}");
        }


        public virtual void Init()
        {
            if (isInit == true)
            {
                return;
            }
            if (GameObjectsToPool == null || GameObjectsToPool.Count == 0)
            {
                return;
            }
            for (int i = 0; i < GameObjectsToPool.Count; i++)
            {
                if (GameObjectsToPool[i].GameObjectToPool == null)
                {
                    continue;
                }
                if (GameObjectsToPool[i]._objectPool != null)
                {
                    continue;
                }
                CreateWaitingPool(GameObjectsToPool[i]);
                int objectsToSpawn = PoolSize - GameObjectsToPool[i]._objectPool.PooledGameObjects.Count;
                for (int j = 0; j < objectsToSpawn; j++)
                {
                    AddOneObjectToThePool(GameObjectsToPool[i]);
                }
            }
            isInit = true;
        }


        protected virtual GameObject GetPooledGameObject(SuperObjectPoolerObject superObjectPoolerObject)
        {
            Init();
            for (int i = 0; i < superObjectPoolerObject._objectPool.PooledGameObjects.Count; i++)
            {
                if (!superObjectPoolerObject._objectPool.PooledGameObjects[i].activeSelf)
                {
                    return superObjectPoolerObject._objectPool.PooledGameObjects[i];
                }
            }


            return AddOneObjectToThePool(superObjectPoolerObject);


        }

        public void DestoryAll()
        {
            if (!isInit)
            {
                return;
            }
            for (int i = 0; i < GameObjectsToPool.Count; i++)
            {
                for (int j = 0; j < GameObjectsToPool[i]._objectPool.PooledGameObjects.Count; j++)
                {
                    GameObjectsToPool[i]._objectPool.PooledGameObjects[j].SetActive(false);

                }
            }
        }
        public int GetCount()
        {
            if (isInit == false)
            {
                return 0;
            }
            int Sum = 0;
            for (int i = 0; i < GameObjectsToPool.Count; i++)
            {

                Sum += GameObjectsToPool[i]._objectPool.PooledGameObjects.Count;

            }
            return Sum;
        }
        public GameObject DestoryOne()
        {
           
            if (isInit == false)
            {
                return null;
            }
            for (int i = 0; i < GameObjectsToPool.Count; i++)
            {
                for (int j = 0; j < GameObjectsToPool[i]._objectPool.PooledGameObjects.Count; j++)
                {
                    if (GameObjectsToPool[i]._objectPool.PooledGameObjects[j].activeInHierarchy)
                    {
                        GameObjectsToPool[i]._objectPool.PooledGameObjects[j].SetActive(false);
                        return GameObjectsToPool[i]._objectPool.PooledGameObjects[j];

                    }

                }
            }
            return null;
        }




        public virtual GameObject GetPooledGameObject()
        {
           
            if (isInit == false)
            {
                Init(); 
            }
            int[] Arr = GameObjectsToPool.Select(a => a.Weight).ToArray();
            return GetPooledGameObject(GameObjectsToPool[RandArr(Arr)]);

        }
        public int RandArr(int[] rate)
        {
            int total = 0; // 修改total的初始值为0
            for (int i = 0; i < rate.Length; i++)
            {
                total += rate[i];
            }
            if (total == 0) return -1; // 如果所有权重都是0，则返回一个明显的错误值

            int r = Random.Range(1, total + 1); // 修改随机数生成范围
            int t = 0;

            for (int i = 0; i < rate.Length; i++)
            {
                t += rate[i];
                if (r <= t) // 修改比较条件
                {
                    return i;
                }
            }

            return -1; // 理论上，这个返回是不必要的
        }
        protected virtual GameObject AddOneObjectToThePool(SuperObjectPoolerObject superObjectPoolerObject)
        {

            if (superObjectPoolerObject.GameObjectToPool == null)
            {
                Debug.LogWarning("The " + gameObject.name + " ObjectPooler doesn't have any GameObjectToPool defined.", gameObject);
                return null;
            }


            bool initialStatus = superObjectPoolerObject.GameObjectToPool.activeSelf;
            superObjectPoolerObject.GameObjectToPool.SetActive(false);
            GameObject newGameObject = (GameObject)Instantiate(superObjectPoolerObject.GameObjectToPool);
            superObjectPoolerObject.GameObjectToPool.SetActive(initialStatus);
            // if (this.gameObject != null)
            // {
            //  SceneManager.MoveGameObjectToScene(newGameObject, this.gameObject.scene);
            // }

            newGameObject.transform.SetParent(superObjectPoolerObject._objectPool.transform);

            newGameObject.name = superObjectPoolerObject.GameObjectToPool.name + "-" + superObjectPoolerObject._objectPool.PooledGameObjects.Count;

            superObjectPoolerObject._objectPool.PooledGameObjects.Add(newGameObject);

            return newGameObject;
        }



        private static void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            for (int i = _pools.Count - 1; i >= 0; i--)
            {
                if (_pools[i] == null)
                {
                    _pools.RemoveAt(i);
                }
            }
        }





    }
    public class SuperObjectPool : MonoBehaviour
    {

        public List<GameObject> PooledGameObjects;
    }

}