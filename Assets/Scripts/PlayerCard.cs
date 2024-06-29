using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCard : MonoBehaviour
{
    [SerializeField] private TMP_Text id_text, name_text;

    public void Initialize(string id){
        id_text.text = id;
    }

    public void SetName(string name){
        name_text.text = name;
    }
}
