using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TarotCard : MonoBehaviour
{
    [HideInInspector] public CardProperty cardProperty;
    [SerializeField] private Image _texture;
    [SerializeField] private TMP_Text descText;
    [SerializeField] private Toggle toggle;
    [SerializeField] private Button selectButton;

    private UpgradesPanel upgradesPanel;
    
    public void Initialize(CardProperty _cardProperty,ToggleGroup _toggleGroup,UpgradesPanel _upgradesPanel,Button _selectButton,Ability _ability=null){
        upgradesPanel = _upgradesPanel;
        selectButton = _selectButton;
        cardProperty = _cardProperty;

        string extraText="";
        switch (cardProperty.cardId)
        {
            case 22:
                    extraText = _ability.cooldownUpgrade+"s.";
            break;
            case 23:
                extraText = _ability.strengthUpgrade+"%.";
            break;
            case 24:
                extraText = _ability.durationUpgrade+"s.";
            break;
            case 25:
                extraText = _ability.rangeUpgrade+"m.";
            break;
            default:
                extraText = "";
            break;
        }
        

        _texture.sprite=cardProperty.cardTexture;
        descText.text=cardProperty.cardDesc+" "+extraText;

        toggle.group = _toggleGroup;
    }

    public void Select(){
        upgradesPanel.selectetCardId=cardProperty.cardId;
        selectButton.interactable = true;
    }
}
