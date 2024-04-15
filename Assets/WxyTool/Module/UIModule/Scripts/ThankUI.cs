﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wxy.Tool;
namespace Wxy.Engine
{
    public class ThankUI : UIBase
    {
       public ThankUI()
        {
            PrefabsPath_string = "Assets/WxyTool/UIModule/Prefab/ThankUI.prefab";
        }
        public override void Init(GameObject uiGameOjbect)
        {
            base.Init(uiGameOjbect);

            GetOrAddCommonent<TouchButton>("Close_Button").ButtonDown_UnityEvent.AddListener(() =>
            {
                
                Close();
            });

        }
    }
}