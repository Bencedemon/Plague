using UnityEngine;


[CreateAssetMenu(fileName = "CardProperty", menuName = "ScriptableObjects/CardProperty", order = 2)]
public class CardProperty : ScriptableObject
{
    public int cardId;
    public string cardName;
    public string cardDesc;
    public Sprite cardTexture;
}
