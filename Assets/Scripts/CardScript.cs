using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    public int teamID = 0; //None = 0; Red = 1; Blue = 2; Death = 3;
    public int view = 0; //Spymaster or Operatives
    public bool revealed = false; //Card has been revealed
    Button btn = null;
    Image img = null;
    CardManager mngr;


    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        btn = GetComponent<Button>();
        btn.interactable = true;
        btn.onClick.AddListener(click);
        mngr = this.transform.parent.gameObject.GetComponentInChildren<CardManager>();
    }

    void displayColor()
    {
        switch (teamID)
        {
            case 0:
                img.color = Color.white;
                break;
            case 1:
                img.color = Color.red;
                break;
            case 2:
                img.color = Color.blue;
                break;
            case 3:
                img.color = Color.grey;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        view = (mngr.turns) % 2;
        if (view == 0)
        {
            btn.interactable = false;
            displayColor();
        }
        else
        {
            img.color = Color.white;
        }
        if (revealed == true)
        {
            displayColor();
            btn.interactable = false;
        }
        else
        {
            btn.interactable = true;
        }
    }
    public void click()
    {
        if (!revealed && mngr.turns%2==1)
        {
            revealed = true;
            mngr.registerPoint(teamID);
        }
    }
}
