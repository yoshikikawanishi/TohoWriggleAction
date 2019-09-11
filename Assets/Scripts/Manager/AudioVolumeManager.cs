using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using LitJson;
using System.Text;
using System.IO;

public class AudioVolumeManager : MonoBehaviour {

    [SerializeField] private GameObject BGM_Volume_Slider;
    [SerializeField] private GameObject SE_Volume_Slider;

    private Slider BGM_Slider;
    private Slider SE_Slider;

    [SerializeField] AudioMixer audio_Mixer;

    private float now_BGM_Volume;
    private float now_SE_Volume;
    private float old_BGM_Volume;
    private float old_SE_Volume;


    // Use this for initialization
    void Start () {
        BGM_Slider = BGM_Volume_Slider.GetComponent<Slider>();
        SE_Slider = SE_Volume_Slider.GetComponent<Slider>();

        //初期値をセット
        Load_Audio_Setting();
	}
	
	// Update is called once per frame
	void Update () {
        now_BGM_Volume = BGM_Slider.value;
        now_SE_Volume = SE_Slider.value;
        Set_Audio_Volume();

    }

    //スライダーの値をAudioMixerに反映
    private void Set_Audio_Volume() {
        if (now_BGM_Volume != old_BGM_Volume) {
            old_BGM_Volume = now_BGM_Volume;
            audio_Mixer.SetFloat("BGMvol", now_BGM_Volume);
        }
        if(now_SE_Volume != old_SE_Volume) {
            old_SE_Volume = now_SE_Volume;
            audio_Mixer.SetFloat("SEvol", now_SE_Volume);
        }
    }


    //設定のロード
    public void Load_Audio_Setting() {
        //ファイル読み込み
        string filePath = Application.dataPath + @"\StreamingAssets\AudioSetting.txt";
        TextFileReader text = new TextFileReader();
        text.Read_Text_File(filePath);

        //読み込み
        float bgm = float.Parse(text.textWords[1, 1]);
        float se = float.Parse(text.textWords[2, 1]);
        BGM_Slider.value = bgm;
        SE_Slider.value = se;
    }


    //設定の保存
    public void Save_Audio_Setting() {
        //ファイル読み込み
        string filePath = Application.dataPath + @"\StreamingAssets\AudioSetting.txt";
        TextFileReader text = new TextFileReader();
        text.Read_Text_File(filePath);

        //書き換え
        StreamWriter sw_Clear = new StreamWriter(filePath, false);
        sw_Clear.Write(text.textWords[0, 0] + "\t" + text.textWords[0, 1] + "\n");
        sw_Clear.Flush();
        sw_Clear.Close();

        StreamWriter sw = new StreamWriter(filePath, true);
        sw.Write("BGM\t" + now_BGM_Volume.ToString() + "\n");
        sw.Write("SE\t" + now_SE_Volume.ToString());
        sw.Flush();
        sw.Close();
    }
}
