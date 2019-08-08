using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsualSoundManager : MonoBehaviour {

    //ショット音
    static public AudioSource shot_Sound;
    static public AudioSource small_Shot_Sound;
    static public AudioSource familiar_Appear_Sound;
    static public AudioSource charge_Sound;
    static public AudioSource pause_In_Sound;
    static public AudioSource pause_Out_Sound;


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

        AudioSource[] audios = GetComponents<AudioSource>();
        shot_Sound = audios[0];
        small_Shot_Sound = audios[1];
        familiar_Appear_Sound = audios[2];
        charge_Sound = audios[3];
        pause_In_Sound = audios[4];
        pause_Out_Sound = audios[5];
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

    //パワーチャージ音
    static public void Charge_Sound() {
        charge_Sound.Play();
    }

    //ポーズ
    static public void Pause_In_Sound() {
        pause_In_Sound.Play();
    }

    //ポーズ解除
    static public void Pause_Out_Sound() {
        pause_In_Sound.Play();
    }
    
}
