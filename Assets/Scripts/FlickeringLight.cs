using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickeringLight : MonoBehaviour
{

    public Light lightSource;


    private float newIntensity=1f;
    void Start(){
        StartCoroutine(ChangeIntensity());
    }
    void FixedUpdate()
    {
        lightSource.intensity = Mathf.Lerp(lightSource.intensity, newIntensity, 2f * Time.fixedDeltaTime);

    }
    private IEnumerator ChangeIntensity(){
        while(true){
            yield return new WaitForSeconds(Random.Range(0.25f, 1f));
            newIntensity=Random.Range(0.5f, 1.5f);
        }
    }

}
