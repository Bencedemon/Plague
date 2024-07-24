using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTrail : MonoBehaviour
{
    public virtual IEnumerator SpawnTrail(TrailRenderer _trail, Vector3 _hit){
        float time = 0f;
        Vector3 startPosition = _trail.transform.position;

        while(time < 1){
            _trail.transform.position = Vector3.Lerp(startPosition, _hit, time);
            time += Time.deltaTime/_trail.time;

            yield return null;
        }
        _trail.transform.position = _hit;

        Destroy(this.gameObject, _trail.time);
    }
}
