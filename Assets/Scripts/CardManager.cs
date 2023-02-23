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
    GameObject turnsText;

    [SerializeField]
    GameObject btnNext;

    [SerializeField]
    GameObject highlightBtn;

    GameObject[] btnArray = new GameObject[25];
    RectTransform highlightRectTransform;
    RectTransform btnRectTransform;
    int redCardsCount = 9;
    int blueCardsCount = 8;
    Vector2 highlightPos = new Vector2(0, 0);

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
        highlightBtn.SetActive(false);

        for (int j = 0; j < 5; j++)
        {
            for (int i = 0; i < 5; i++)
            {
                btnArray[(i * 5) + j] = Instantiate(card, new Vector3(0, 0, 0), Quaternion.identity, this.transform.parent);
                btnArray[(i * 5) + j].GetComponent<RectTransform>().anchoredPosition = new Vector3(i * btnRectTransform.rect.width - 356 + (i * 5), j * btnRectTransform.rect.height - 175 + (j * 5), 0);
                btnArray[(i * 5) + j].name = "(" + i + "," + j + ")";
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
        if (redCardsCount == 0)
        {
            Debug.Log("Red wins!");
            endGame();
        }
        else if (blueCardsCount == 0)
        {
            Debug.Log("Blue wins!");
            endGame();
        }

        // key check   
        if (turns % 2 == 1)
        {
            keyCheck();
        }

    }

    void keyCheck()
    {
        
        if (Input.GetKeyDown(KeyCode.UpArrow))
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
        btnArray[(int)((highlightPos.x * 5) + highlightPos.y)].GetComponent<Button>().Select();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            nextTurn();
        }
    }
    void nextTurn()
    {
        turns++;
        turns %= 4;
        switch (turns)
        {
            case 0:
                turnsText.GetComponent<Text>().text = "Red Spymaster";
                break; 
            case 1:
                turnsText.GetComponent<Text>().text = "Red Team";
                break; 
            case 2:
                turnsText.GetComponent<Text>().text = "Blue Spymaster";
                break; 
            case 3:
                turnsText.GetComponent<Text>().text = "Blue Team";
                break;
        }
        if (turns % 2 == 0)
        {
            highlightBtn.SetActive(false);
        }
        else
        {
            highlightBtn.SetActive(true);
            highlightBtn.GetComponent<Button>().Select();
        }
    }


    public void registerPoint(int teamID)
    {
        Debug.Log("teamID of pressed button: " + teamID);
        switch (teamID)
        {
            case 0:
                nextTurn();
                break;
            case 1:
                redCardsCount--;
                if (turns == 3)
                {
                    nextTurn();
                }
                break;
            case 2:
                blueCardsCount--;
                if (turns == 1)
                {
                    nextTurn();
                }
                break;
            case 3:
                if (turns == 1)
                {
                    Debug.Log("Blue wins!");
                }
                else if (turns == 3)
                {
                    Debug.Log("Red wins!");
                }
                else
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
