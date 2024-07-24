using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CrosshairSelect : MonoBehaviour
{
    [SerializeField] private Image _texture;
    [SerializeField] private int _crosshairId;

    private CrosshairEditor crosshairEditor;
    
    public void Initialize(CrosshairEditor _crosshairEditor,int _id,Sprite _sprite){
        crosshairEditor = _crosshairEditor;

        _crosshairId = _id;

        _texture.sprite = _sprite;
    }

    public void Select(){
        crosshairEditor.TextureSelected(_crosshairId);
    }
}
