using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ProfilePictureSelect : MonoBehaviour
{
    [SerializeField] private Image _texture;
    [SerializeField] private int _profilePictureId;

    private PlayerProfileEditor profilePictureEditor;
    
    public void Initialize(PlayerProfileEditor _profilePictureEditor,int _id,Sprite _sprite){
        profilePictureEditor = _profilePictureEditor;

        _profilePictureId = _id;

        _texture.sprite = _sprite;
    }

    public void Select(){
        profilePictureEditor.TextureSelected(_profilePictureId);
    }
}
