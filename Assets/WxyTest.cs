using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wxy.Tool;
using Wxy.Engine;
using Sirenix.OdinInspector;

public class WxyTest : MonoBehaviour
{

    [Button]
    public  void OPen()
    {
        UIManager.Instance.OpenUI<TestUI>();
    }
}
