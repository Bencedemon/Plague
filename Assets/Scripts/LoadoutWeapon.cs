using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadoutWeapon : MonoBehaviour
{
    [SerializeField] private Image _texture;

    [SerializeField] private Button button;
    [SerializeField] private GameObject lockPanel;
    [SerializeField] private TMP_Text levelText;
    
    private LoadoutEditor.WeaponCategory weaponCategory;
    private WeaponProperty weaponProperty;

    private LoadoutEditor loadoutEditor;

    private PlayerProfileManager playerProfileManager;
    
    public void Initialize(LoadoutEditor _loadoutEditor,WeaponProperty _weaponProperty,LoadoutEditor.WeaponCategory _weaponCategory, PlayerProfileManager _playerProfileManager){
        playerProfileManager = _playerProfileManager;

        loadoutEditor = _loadoutEditor;
        weaponProperty = _weaponProperty;
        weaponCategory = _weaponCategory;

        _texture.sprite = _weaponProperty.iconSmall;

        bool unlocked = (playerProfileManager.playerProfile.playerLevel>=_weaponProperty.level);
        if(!unlocked){
            levelText.text="Level "+weaponProperty.level;
        }
        button.interactable = unlocked;
        lockPanel.SetActive(!unlocked);
    }

    public void Select(){
        switch (weaponCategory)
        {
            case LoadoutEditor.WeaponCategory.primary:
                loadoutEditor.selectPrimarie(weaponProperty.weaponId);
            break;
            case LoadoutEditor.WeaponCategory.secondary:
                loadoutEditor.selectSecondary(weaponProperty.weaponId);
            break;
            case LoadoutEditor.WeaponCategory.meele:
                loadoutEditor.selectMeele(weaponProperty.weaponId);
            break;
            default:
                Debug.LogError("Error");
            break;
        }
    }
}
