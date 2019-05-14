using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//画面を揺らすスクリプト
public class CameraShake : MonoBehaviour {

    public void Shake(float duration, float magnitude) {
        StartCoroutine(DoShake(duration, magnitude));
    }

    private IEnumerator DoShake(float duration, float magnitude) {
        GameObject main_Camera = GameObject.Find("Main Camera");
        var pos = main_Camera.transform.localPosition;

        var elapsed = 0f;

        while (elapsed < duration) {
            var x = pos.x + Random.Range(-1f, 1f) * magnitude;
            var y = pos.y + Random.Range(-1f, 1f) * magnitude;

            main_Camera.transform.localPosition = new Vector3(x, y, pos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        main_Camera.transform.localPosition = pos;
    }
}


/*使用例
    public CameraShake shake;

    private void Update()
    {
        if ( Input.GetKeyDown( KeyCode.Z ) )
        {
            shake.Shake( 0.25f, 0.1f );
        }
    } 
     */
