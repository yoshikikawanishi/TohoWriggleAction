﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GoodsPanel : MonoBehaviour {

    //OnEnable
    private void OnEnable() {
        StartCoroutine("Wait_One_Frame");
    }

    private IEnumerator Wait_One_Frame() {
        yield return null;
        //ボタン選択
        transform.GetChild(4).GetComponent<Button>().Select();
    }


    
}