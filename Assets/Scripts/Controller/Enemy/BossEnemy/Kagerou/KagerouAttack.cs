using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KagerouAttack : MonoBehaviour {

    //フェーズ1用
    [System.Serializable]
    public class Phase1_Status {
        public bool start_Routine = true;
    }

    //フェーズ2用
    [System.Serializable]
    public class Phase2_Status {
        public bool start_Routine = true;
    }

    //フェーズ3用
    [System.Serializable]
    public class Phase3_Status {
        public bool start_Routine = true;
    }

    //フェーズ4用
    [System.Serializable]
    public class Phase4_Status {
        public bool start_Routine = true;
    }

    Phase1_Status phase1;
    Phase2_Status phase2;
    Phase3_Status phase3;
    Phase4_Status phase4;

    //オブジェクトプール
    private ObjectPoolManager pool_Manager;
    private GameObject blue_Bullet;
    private GameObject red_Bullet;
    

	// Use this for initialization
	void Start () {
        //オブジェクトプール
        pool_Manager = GameObject.FindWithTag("ScriptsTag").GetComponent<ObjectPoolManager>();
        blue_Bullet = Resources.Load("Bullet/PooledBullet/BlueBulletPool") as GameObject;
        red_Bullet = Resources.Load("Bullet/PooledBullet/RedBulletPool") as GameObject;
        pool_Manager.Create_New_Pool(blue_Bullet, 20);
        pool_Manager.Create_New_Pool(red_Bullet, 20);
    }
	
	
    //フェーズ1
    public void Phase1() {
        if (phase1.start_Routine) {
            phase1.start_Routine = false;
            StartCoroutine("Phase1_Routine");
        }
    }

    private IEnumerator Phase1_Routine() {
        yield return null;
    }

    

    //フェーズ2
    public void Phase2() {

    }

    //フェーズ3
    public void Phase3() {

    }

    //フェーズ4
    public void Phase4() {

    }

}
