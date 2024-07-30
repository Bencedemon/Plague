using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Calculator
{
    public static float CalcDamageReduction(float _armor){
        return _armor/(_armor+90f);
    }
}
