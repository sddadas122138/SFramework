
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
#endif
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Wxy.Tool
{
    public class AAResources
    {
        public static T LoadAsset<T>(string path)
        {
            Addressables.InitializeAsync();//Adddressanles 的安卓Bug 等 团队解决  删除


            var request = LoadAssetAsync<T>(path);
            return request.WaitForCompletion();
        }

        public static AsyncOperationHandle<T> LoadAssetAsync<T>(string path)
        {
            var request = Addressables.LoadAssetAsync<T>(path);
            return request;
        }

        public static AsyncOperationHandle<SceneInstance> LoadSceneAsync(string level, LoadSceneMode loadSceneMode)
        {
            var async = Addressables.LoadSceneAsync(level, loadSceneMode);
            return async;
        }

        public static GameObject Instantiate(string path, Transform parent)
        {
            var request = InstantiateAsync(path, parent);
            return request.WaitForCompletion();
        }

        public static AsyncOperationHandle<GameObject> InstantiateAsync(string path, Transform parent)
        {
            try
            {
                return Addressables.InstantiateAsync(path, parent);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }

            return default;
        }

        public static void ReleaseInstance(GameObject instance)
        {
            Addressables.ReleaseInstance(instance);
        }

        public static void Release<T>(T target)
        {
            Addressables.Release(target);
        }


#if UNITY_EDITOR


        public  static void AutoAddAddressadle()
        {
            
                var guids = AssetDatabase.FindAssets("t:" + typeof(UnityEngine.Object).Name, new[] { "Assets/Loop/Res" });

                foreach (var guid in guids)
                {
                    var assetPath = AssetDatabase.GUIDToAssetPath(guid);
                AddToAddressable(assetPath, "", "Default Local Group");
                    Debug.Log($"Found asset at path: {assetPath}");
                }
            
        }
        public static void AddToAddressable(string path, string address = "", string groupName = "PlayAssetDelivery", bool isAdd = true)
        {
            if (string.IsNullOrEmpty(path)) return;
            var assetSettingPath = "Assets/AddressableAssetsData/AddressableAssetSettings.asset";
            AddressableAssetSettings addressableAssetSettings = AssetDatabase.LoadAssetAtPath<AddressableAssetSettings>(assetSettingPath);

            SetAaEntry(addressableAssetSettings, groupName, path, isAdd, address);
        }

        static void SetAaEntry(AddressableAssetSettings aaSettings, string groupName, string path, bool create, string address = "")
        {
            AddressableAssetGroup assetGroup = null;
            if (!string.IsNullOrEmpty(groupName))
            {
                assetGroup = aaSettings.FindGroup(groupName);
            }
            else
            {
                assetGroup = aaSettings.DefaultGroup;
            }


            if (create && assetGroup.ReadOnly)
            {
                Debug.LogError("Current default group is ReadOnly.  Cannot add addressable assets to it");
                return;
            }

            Undo.RecordObject(aaSettings, "AddressableAssetSettings");
            var guid = string.Empty;
            //if (create || EditorUtility.DisplayDialog("Remove Addressable Asset Entries", "Do you want to remove Addressable Asset entries for " + targets.Length + " items?", "Yes", "Cancel"))
            {
                var entriesAdded = new List<AddressableAssetEntry>();
                var modifiedGroups = new HashSet<AddressableAssetGroup>();

                guid = AssetDatabase.AssetPathToGUID(path);
                if (create)
                {
                    var e = aaSettings.CreateOrMoveEntry(guid, assetGroup, false, false);
                    if (!string.IsNullOrEmpty(address)) e.address = address;
                    entriesAdded.Add(e);
                    modifiedGroups.Add(e.parentGroup);
                }
                else
                {
                    aaSettings.RemoveAssetEntry(guid);
                }

                if (create)
                {
                    foreach (var g in modifiedGroups)
                        g.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, false, true);
                    aaSettings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entriesAdded, true, false);
                }
            }
        }
#endif


    }
}