using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [SerializeField]
    GameObject card;

    [SerializeField]
    GameObject redScoreText;
    [SerializeField]
    GameObject blueScoreText;

    [SerializeField]
    GameObject btnNext;

    [SerializeField]
    GameObject highlightBtn;

    GameObject[] btnArray = new GameObject[25];
    RectTransform highlightRectTransform;
    RectTransform btnRectTransform;
    int redCardsCount = 9;
    int blueCardsCount = 8;
    Vector2 highlightPos = new Vector2(0,0);

    public int turns = 0; //Spymaster red; team red; Spymaster blue; team blue



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

        int black = (int)cards[Random.Range(0, cards.Count)];
        cards.Remove(black);

        btnRectTransform = card.GetComponent<RectTransform>();
        highlightRectTransform = highlightBtn.GetComponent<RectTransform>();
        //highlightRectTransform.rect.Set(0,0,btnRectTransform.rect.width+10, btnRectTransform.rect.height+10);
        highlightRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, btnRectTransform.rect.height + 10);
        highlightRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, btnRectTransform.rect.width + 10);


        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                btnArray[(i * 5) + j] = Instantiate(card, new Vector3(0, 0, 0), Quaternion.identity, this.transform.parent);
                btnArray[(i * 5) + j].GetComponent<RectTransform>().anchoredPosition = new Vector3(i * btnRectTransform.rect.width - 356 + (i*5), j * btnRectTransform.rect.height - 175 + (j * 5), 0);
                if (redCards.Contains((i * 5) + j))
                {
                    btnArray[(i * 5) + j].GetComponent<CardScript>().teamID = 1;
                }
                if (blueCards.Contains((i * 5) + j))
                {
                    btnArray[(i * 5) + j].GetComponent<CardScript>().teamID = 2;
                }
                if (black == (i * 5) + j)
                {
                    btnArray[(i * 5) + j].GetComponent<CardScript>().teamID = 3;
                }
            }
        }
        btnNext.GetComponent<Button>().onClick.AddListener(nextTurn);
        Debug.Log("Inst");
    }

    // Update is called once per frame
    void Update()
    {
        redScoreText.GetComponent<Text>().text = "Red: " + redCardsCount;
        blueScoreText.GetComponent<Text>().text = "Blue: " + blueCardsCount;
        turns = turns % 4;
        if(redCardsCount==0)
        {
            Debug.Log("Red wins!");
            endGame();
        } else if(blueCardsCount==0)
        {
            Debug.Log("Blue wins!");
            endGame();
        }
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            highlightPos.y++;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            highlightPos.y--;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            highlightPos.x--;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            highlightPos.x++;
        }
        highlightPos.x = (highlightPos.x + 5) % 5;
        highlightPos.y = (highlightPos.y + 5) % 5;
        highlightRectTransform.GetComponent<RectTransform>().anchoredPosition = new Vector3(highlightPos.x * btnRectTransform.rect.width - 356 + (highlightPos.x * 5), highlightPos.y * btnRectTransform.rect.height - 175 + (highlightPos.y * 5));

        if(Input.GetKeyDown(KeyCode.Return))
        {
            btnArray[(int)((highlightPos.x * 5) + highlightPos.y)].GetComponent<CardScript>().click();
        }
        if(turns%2==0)
        {
            highlightBtn.active = false;
        } else
        {
            highlightBtn.active = true;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            nextTurn();
        }
    }
    void nextTurn()
    {
        turns++;
    }

    public void registerPoint(int teamID)
    {
        switch (teamID)
        {
            case 0:
                turns++;
                break;
            case 1:
                redCardsCount--;
                if (turns == 3)
                {
                    turns++;
                }
                break;
            case 2:
                blueCardsCount--;
                if (turns==1)
                {
                    turns++;
                }
                break;
            case 3:
                if(turns == 1)
                {
                    Debug.Log("Blue wins!");
                } else if(turns ==3)
                {
                    Debug.Log("Red wins!");
                } else
                {
                    Debug.Log("What?");
                }
                endGame();
                break;

        }
    }
    void endGame()
    {
        btnNext.GetComponent<Button>().interactable = false;
        turns = 0;
        for (int i = 0; i < btnArray.Length; i++)
        {
            btnArray[i].GetComponent<Button>().interactable = false;
        }
    }
}
