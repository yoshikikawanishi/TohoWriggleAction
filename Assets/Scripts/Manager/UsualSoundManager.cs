using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsualSoundManager : MonoBehaviour {

    //ショット音
    static public AudioSource shot_Sound;
    static public AudioSource small_Shot_Sound;
    static public AudioSource familiar_Appear_Sound;

    //シングルトン用
    public static UsualSoundManager instance;
    void Awake() {
        //シングルトン
        if (instance != null) {
            Destroy(this.gameObject);
        }
        else if (instance == null) {
            instance = this;
        }
        //シーンを遷移してもオブジェクトを消さない
        DontDestroyOnLoad(gameObject);

        shot_Sound = GetComponents<AudioSource>()[0];
        small_Shot_Sound = GetComponents<AudioSource>()[1];
        familiar_Appear_Sound = GetComponents<AudioSource>()[2];
    }


    //ショット音
    static public void Shot_Sound() {
        shot_Sound.Play();
    }

    //はじける音
    static public void Small_Shot_Sound() {
        small_Shot_Sound.Play();
    }

    //使い魔実体化音
    static public void Familiar_Appear_Sound() {
        familiar_Appear_Sound.Play();
    }
}
