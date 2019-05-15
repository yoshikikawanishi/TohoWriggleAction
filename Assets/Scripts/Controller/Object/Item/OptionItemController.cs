using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionItemController : MonoBehaviour {

    //種類
    [SerializeField] private string option_Type;


    //OnTriggerEnter
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "PlayerBodyTag") {
            GameObject.FindWithTag("CommonScriptsTag").GetComponent<PlayerManager>().Set_Option_Type(option_Type);
            Destroy(gameObject);
        }    
    }


}
