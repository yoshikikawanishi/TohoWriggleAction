using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour {
    private List<GameObject> _poolObjList;
    private GameObject _poolObj;

    //オブジェクトプールされているかどうか
    private bool is_Pooled = false;

    // オブジェクトプールを作成
    public void CreatePool(GameObject obj, int maxCount) {
        _poolObj = obj;
        _poolObjList = new List<GameObject>();
        for (int i = 0; i < maxCount; i++) {
            var newObj = CreateNewObject();
            newObj.SetActive(false);
            _poolObjList.Add(newObj);
        }
        is_Pooled = true;
    }

    public GameObject GetObject() {
        // 使用中でないものを探して返す
        foreach (var obj in _poolObjList) {
            if (obj.activeSelf == false) {
                obj.SetActive(true);
                return obj;
            }
        }

        // 全て使用中だったら新しく作って返す
        var newObj = CreateNewObject();
        newObj.SetActive(true);
        _poolObjList.Add(newObj);

        return newObj;
    }

    private GameObject CreateNewObject() {
        var newObj = Instantiate(_poolObj);
        newObj.name = _poolObj.name + (_poolObjList.Count + 1);

        return newObj;
    }

    //オブジェクトプールされているかどうか
    public bool Is_Pooled() {
        return is_Pooled;
    }

}
