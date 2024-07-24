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
    
    public void Initialize(CardProperty _cardProperty,ToggleGroup _toggleGroup,UpgradesPanel _upgradesPanel,Button _selectButton){
        upgradesPanel = _upgradesPanel;
        selectButton = _selectButton;
        cardProperty = _cardProperty;

        _texture.sprite=cardProperty.cardTexture;
        descText.text=cardProperty.cardDesc;

        toggle.group = _toggleGroup;
    }

    public void Select(){
        upgradesPanel.selectetCardId=cardProperty.cardId;
        selectButton.interactable = true;
    }
}
