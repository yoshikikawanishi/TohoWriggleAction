﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AfterEndingScene : MonoBehaviour {

   
    
    // Use this for initialization
	void Start () {
        GetComponent<FadeInOut>().Start_Fade_In();
        GetComponent<AfterEndingMovie>().Start_After_Ending_Movie();
    }
	
}