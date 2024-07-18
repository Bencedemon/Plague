using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPerformance : MonoBehaviour
{
    public int kills,damageDealt,damageTaken,deaths;

    public void SetValues(int _kills,int _damageDealt,int _damageTaken,int _deaths){
        kills= _kills;
        damageDealt= _damageDealt;
        damageTaken= _damageTaken;
        deaths= _deaths;
    }
}
