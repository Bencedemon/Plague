using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradesPanel : MonoBehaviour
{
    
    [SerializeField] private PlayerUpgrades playerUpgrades;

    [SerializeField] private TarotCard cardPrefab;

    [SerializeField] private CardProperty[] cardProperties; 

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
        maxWeight=0;
        foreach (var card in cardProperties)
        {
            maxWeight+=card.weight;
            weights.Add(maxWeight);
        }
    }

    public void ShowCrads(){
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
