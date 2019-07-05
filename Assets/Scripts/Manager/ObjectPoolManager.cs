using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour {

    private Dictionary<string, ObjectPool> pool_Dictinary = new Dictionary<string, ObjectPool>();

    //オブジェクトプールの追加
    public void Create_New_Pool(GameObject obj, int num) {
        //もうすでに存在する場合作らない
        if (pool_Dictinary.ContainsKey(obj.name)) {
            return;
        }
        ObjectPool _pool = gameObject.AddComponent<ObjectPool>();
        _pool.CreatePool(obj, num);
        pool_Dictinary.Add(obj.name, _pool);
    }

    //オブジェクトの生成
    public ObjectPool Get_Pool(GameObject obj) {
        if (pool_Dictinary.ContainsKey(obj.name)) {
            return pool_Dictinary[obj.name];
        }
        else {
            Create_New_Pool(obj, 10);
            return pool_Dictinary[obj.name];
        }
    }
}
