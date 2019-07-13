using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MystiaScalesBullet : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine("Action");	
	}
	
    private IEnumerator Action() {
        yield return new WaitForSeconds(24f / 7f);
        GameObject[] bullet = new GameObject[2];
        for (int i = 0; i < transform.childCount; i++) {
            bullet[i] = transform.GetChild(i).gameObject;
        }
        for(int i = 0; i < transform.childCount; i++) {
            bullet[i].transform.Rotate(new Vector3(0, 0, 180f * (i - 0.5f)));
            bullet[i].GetComponent<Rigidbody2D>().velocity = bullet[i].transform.right * 50f;
        }
        
    }
	
}
