using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtSlerp : MonoBehaviour {

    public void Start_LookAt_Routine(Vector3 target, Vector2 forward, float rotate_Time) {
        StartCoroutine(LookAtRoutine(target, forward, rotate_Time));
    }

    public IEnumerator LookAtRoutine(Vector3 target, Vector2 forward, float rotate_Time) {
        var forwardDiff = GetForwardDiffPoint(forward);
        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg % 360f;
        for (float t = 0; t < rotate_Time; t += Time.deltaTime) {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, 0, angle - forwardDiff), t);
            yield return null;
        }
    }

    /// <summary>
    /// 正面の方向の差分を算出する
    /// </summary>
    /// <returns>The forward diff point.</returns>
    /// <param name="forward">Forward.</param>
    private float GetForwardDiffPoint(Vector2 forward) {
        if (Equals(forward, Vector2.up))
            return 90;
        if (Equals(forward, Vector2.right))
            return 0;
        return 0;
    }
}
