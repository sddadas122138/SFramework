using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Wxy.Tool;
namespace Wxy.Engine
{
    [CreateAssetMenu(menuName = "GeneratenUI")]
    public class GeneratenUI : ScriptableObject
    {
     

        public GameObject UIPrefab;


        public string GenGameobjectPath_string = "Assets/Loop/Res/Prefab/UI";
        public string ScriptTemplatePath_string = "Assets/Tools/WxyEngine/UI/StoreUI.cs";
        public string NewScriptPath_string = "Assets/Tools/WxyEngine/UI";
     



       

        [Button("生成UI")]
        public void GenerateUIAndScript(string EnglishName)
        {







            GameObject UI = GenerateUI($"{EnglishName}UI");



            CreatUIScript(UI);



            EditorUtility.SetDirty(UI);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

       

        } 
        public GameObject GenerateUI(string name)
        {
            return GenerateObject(GenGameobjectPath_string, UIPrefab, name);
        } 

        private GameObject GenerateObject(string AmmotargetDirectory, GameObject AmmoobjectPrefab, string Objecname)
        {
            //AmmotargetDirectory = "Assets/Loop";
            if (AmmoobjectPrefab == null)
            {
                Debug.LogError("Object Prefab is not assigned!");
                return null;
            }

            if (!AssetDatabase.IsValidFolder(AmmotargetDirectory))
            {
                Debug.LogError("Target Directory does not exist!");
                return null;
            }

            GameObject newObject = PrefabUtility.InstantiatePrefab(AmmoobjectPrefab) as GameObject;
            string objectName = Objecname;
            string objectPath = AmmotargetDirectory + "/" + objectName + ".prefab";
            string uniquePath = AssetDatabase.GenerateUniqueAssetPath(objectPath);

            GameObject nesw = PrefabUtility.SaveAsPrefabAsset(newObject, uniquePath);
            DestroyImmediate(newObject);

            Debug.Log("Object generated at: " + uniquePath);

            AAResources.AddToAddressable(objectPath, "", "Default Local Group");
            return nesw;
        }

     

       
        public void CreatUIScript(GameObject prefab)
        {
            string ScriptName = prefab.name;
            Debug.Log(ScriptName);
            // Path to the template script
            string templatePath = ScriptTemplatePath_string;

            // Read the template
            string templateCode;
            try
            {
                templateCode = File.ReadAllText(templatePath);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to read the template: " + e.Message);
                return;
            }

            // The path to save the new script
            string newScriptPath = $"{NewScriptPath_string}/{ScriptName}.cs";
            templateCode = templateCode.Replace(UIPrefab.name, ScriptName);

            // Create a new script based on the template
            try
            {
                File.WriteAllText(newScriptPath, templateCode);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to write the new script: " + e.Message);
                return;
            }



            EditorUtility.SetDirty(prefab);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

        } 
     
       

       
    }
}