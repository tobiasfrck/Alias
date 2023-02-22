using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    [SerializeField] 
    GameObject card;
    GameObject[] btnArray = new GameObject[25];
    int redCardsCount = 9;
    int blueCardsCount = 8;
    

    // Start is called before the first frame update
    void Start()
    {
        ArrayList cards = new ArrayList();
        ArrayList redCards = new ArrayList();
        ArrayList blueCards = new ArrayList();
        for (int i = 0; i < 25; i++)
        {
            cards.Add(i);
        }
        for (int i = 0; i < redCardsCount; i++)
        {
            int idx = Random.Range(0, cards.Count);
            redCards.Add(cards[idx]);
            cards.Remove(cards[idx]);
        }
        for (int i = 0; i < blueCardsCount; i++)
        {
            int idx = Random.Range(0, cards.Count);
            blueCards.Add(cards[idx]);
            cards.Remove(cards[idx]);
        }

        int black = (int) cards[Random.Range(0, cards.Count)];
        cards.Remove(black);

        RectTransform btnRectTransform = card.GetComponent<RectTransform>();
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                btnArray[(i*5)+j] = Instantiate(card, new Vector3(0, 0, 0), Quaternion.identity, this.transform.parent);
                btnArray[(i * 5) + j].GetComponent<RectTransform>().anchoredPosition = new Vector3(i*btnRectTransform.rect.width - 356, j * btnRectTransform.rect.height - 175, 0);
                if(redCards.Contains((i * 5) + j))
                {
                    btnArray[(i * 5) + j].GetComponent<CardScript>().teamID = 1;
                }
                if (blueCards.Contains((i * 5) + j))
                {
                    btnArray[(i * 5) + j].GetComponent<CardScript>().teamID = 2;
                }
                if(black == (i * 5) + j)
                {
                    btnArray[(i * 5) + j].GetComponent<CardScript>().teamID = 3;
                }
            }
        }

        Debug.Log("Inst");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void registerPoint(int teamID)
    {
        Debug.Log(teamID);
    }
}
