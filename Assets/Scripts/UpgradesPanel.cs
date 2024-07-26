using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradesPanel : MonoBehaviour
{
    
    [SerializeField] private PlayerUpgrades playerUpgrades;
    [SerializeField] private PlayerAbility playerAbility;

    [SerializeField] private TarotCard cardPrefab;

    [SerializeField] private CardProperty[] statCards; 
    [SerializeField] private CardProperty[] abilityCards; 

    [Space]
    [SerializeField] private CardProperty[] cupsCards; 
    [SerializeField] private CardProperty[] swordsCards; 
    [SerializeField] private CardProperty[] pentaclesCards; 
    [SerializeField] private CardProperty[] wandsCards; 

    [Space]
    [SerializeField] private List<CardProperty> cardProperties = new List<CardProperty>(); 

    [SerializeField] private ToggleGroup toggleGroup;

    [SerializeField] private Transform cardParent;
    [SerializeField] private Button selectButton;
    [SerializeField] private TMP_Text countDownText;

    [Space]
    [SerializeField] private AudioSource audioSource;
    
    public TarotCard[] tarotCards = new TarotCard[3];

    public int selectetCardId = 0;

    private int countDown = 30;


    
    public float maxWeight;
    public List<float> weights = new List<float>();
    // Start is called before the first frame update
    void Awake()
    {
    }

    public void ShowCrads(){
        maxWeight=0;
        weights.Clear();
        cardProperties.Clear();
        foreach (var card in statCards)
        {
            maxWeight+=card.weight;
            weights.Add(maxWeight);
            cardProperties.Add(card);
        }
        if(playerAbility.ability==null){
            foreach (var card in abilityCards)
            {
                maxWeight+=card.weight;
                weights.Add(maxWeight);
                cardProperties.Add(card);
            }
        }else{
            if(playerAbility.ability.strength!=0 && playerAbility.ability.strengthLevel<swordsCards.Length){
                CardProperty card = swordsCards[playerAbility.ability.strengthLevel];
                maxWeight+=card.weight;
                weights.Add(maxWeight);
                cardProperties.Add(card);
            }
            if(playerAbility.ability.range!=0 && playerAbility.ability.rangeLevel<wandsCards.Length){
                CardProperty card = wandsCards[playerAbility.ability.rangeLevel];
                maxWeight+=card.weight;
                weights.Add(maxWeight);
                cardProperties.Add(card);
            }
            if(playerAbility.ability.duration!=0 && playerAbility.ability.durationLevel<pentaclesCards.Length){
                CardProperty card = pentaclesCards[playerAbility.ability.durationLevel];
                maxWeight+=card.weight;
                weights.Add(maxWeight);
                cardProperties.Add(card);
            }
            if(playerAbility.ability.cooldown!=0 && playerAbility.ability.cooldownLevel<cupsCards.Length){
                CardProperty card = cupsCards[playerAbility.ability.cooldownLevel];
                maxWeight+=card.weight;
                weights.Add(maxWeight);
                cardProperties.Add(card);
            }
        }

        StartCoroutine(CountDown());
        CreateCards();
        selectetCardId = tarotCards[0].cardProperty.cardId;
    }
    public void ShowCradsTheFool(){
        countDown=30;
        foreach (var card in tarotCards)
        {
            Destroy(card.gameObject);
        }
        CreateCards();
        /*for (int i = 0; i < 3; i++)
        {
            TarotCard newCard = Instantiate(cardPrefab,cardParent);
            tarotCards[i]=newCard;
            newCard.Initialize(cardProperties[Random.Range(0,cardProperties.Length)],toggleGroup,this,selectButton);
        }*/
        //selectetCardId = tarotCards[0].cardProperty.cardId;
    }
    private void CreateCards(){
        List<int> ids = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            TarotCard newCard = Instantiate(cardPrefab,cardParent);
            tarotCards[i]=newCard;
            float random;
            int cardId = 0;
            bool exists = false;
            do
            {
                exists = false;
                random = Random.Range(0f,maxWeight);
                for (int j = 0; j < weights.Count; j++){
                    if(random<=weights[j]){
                        cardId=j;
                        break;
                    }
                }
                foreach (var id in ids)
                {
                    if(id==cardId){
                        exists = true;
                        break;
                    }
                }
            } while (exists);
            ids.Add(cardId);
            newCard.Initialize(cardProperties[cardId],toggleGroup,this,selectButton);
            audioSource.Play();
        }
    }

    public void Select(){
        playerUpgrades.SelectUpgrade(selectetCardId);
        if(selectetCardId==0) return;
        selectButton.interactable = false;
        StopCoroutine(CountDown());
        foreach (var card in tarotCards)
        {
            Destroy(card.gameObject);
        }
    }

    private IEnumerator CountDown(){
        countDown=30;
        while (countDown>0)
        {
            countDownText.text=""+countDown;
            yield return new WaitForSecondsRealtime(1f);
            countDown--;
        }
        countDownText.text=""+countDown;
        yield return new WaitForSecondsRealtime(1f);
        Select();
    }
}
