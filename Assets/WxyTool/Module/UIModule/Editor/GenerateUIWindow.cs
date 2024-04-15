using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Wxy.Engine
{
    public class GenerateUIWindow : OdinMenuEditorWindow
	{
		[MenuItem("Tools/奇妙工具")]
		private static void OpenWindow()
		{
			var window = GetWindow<GenerateUIWindow>();
			window.position = GUIHelper.GetEditorWindowRect().AlignCenter(720, 720);
			window.titleContent = new GUIContent("生成UI");
		}

		protected override OdinMenuTree BuildMenuTree()
		{
			var tree = new OdinMenuTree();
			//这里的第一个参数为窗口名字，第二个参数为指定目录，第三个参数为需要什么类型，第四个参数为是否在家该文件夹下的子文件夹
			tree.AddAllAssetsAtPath("生成UI", "Assets/WxyTool/Module/UIModule/Editor", typeof(ScriptableObject), true);
			return tree;
		}

                        

 




    }

}