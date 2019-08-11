using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceObject : MonoBehaviour {

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Init() {
        var obj = Instantiate(Resources.Load("CommonScripts")) as GameObject;
        obj.transform.position = new Vector3(0, 0, 0);
        var obj2 = Instantiate(Resources.Load("UsualSoundEffects")) as GameObject;
        obj2.transform.position = new Vector3(0, 0, 0);
        var obj3 = Instantiate(Resources.Load("BGM")) as GameObject;
        obj3.transform.position = new Vector3(0, 0, 0);
    }
}
