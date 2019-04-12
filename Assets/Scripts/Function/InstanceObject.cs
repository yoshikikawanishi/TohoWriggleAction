using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceObject : MonoBehaviour {

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init() {
        var obj = Instantiate(Resources.Load("CommonScripts")) as GameObject;
    }
}
