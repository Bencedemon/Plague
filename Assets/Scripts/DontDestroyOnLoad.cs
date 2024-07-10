using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private static bool exists;
    // Start is called before the first frame update
    void Awake()
    {
        if(!exists)
        {
            exists = true;
            DontDestroyOnLoad(transform.gameObject);
        }else{
            Destroy(gameObject);
        }
    }
}
