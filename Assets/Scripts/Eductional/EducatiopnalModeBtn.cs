using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EducatiopnalModeBtn : MonoBehaviour
{
    public float price;
    public string productId;

    public EducationalModes.Modes mode;

    public Text modeText;
    public Text rangeText;
    public Image btnBg;

    public Transform divisibleByBtns;
    public InputField lowerRange, upperRange;
    public int rangeGap = 100;

    [Space(20)]
    public bool locked;
    private string modeNameRaw;
    private int lowerVal;
    private int upperVal;

    public void Start()
    {
        

        modeNameRaw = modeText.text;
        if (price != 0 && !PlayerPrefs.HasKey("educationalMode" + mode.ToString())) 
        {
            locked = true;
            modeText.text += "   ( $" + price + " )"; 
        }

        if (divisibleByBtns.gameObject.activeSelf)
        {
            if (!PlayerPrefs.HasKey(mode.ToString() + "ModeSelectedDivisibleBy")) { PlayerPrefs.SetInt(mode.ToString() + "ModeSelectedDivisibleBy", 3); }
            for (int i = 0; i < divisibleByBtns.childCount; i++)
            {
                if (divisibleByBtns.GetChild(i).GetChild(1).GetComponent<Text>().text == PlayerPrefs.GetInt(mode.ToString() + "ModeSelectedDivisibleBy").ToString())
                { SelectDivisibleByBtn(divisibleByBtns.GetChild(i)); }
            }
        }

        if (lowerRange.gameObject.activeSelf && upperRange.gameObject.activeSelf)
        {
            if (!PlayerPrefs.HasKey(mode.ToString() + "ModeLowerRangeVal")) { PlayerPrefs.SetInt(mode.ToString() + "ModeLowerRangeVal", -1000); }
            if (!PlayerPrefs.HasKey(mode.ToString() + "ModeUpperRangeVal")) { PlayerPrefs.SetInt(mode.ToString() + "ModeUpperRangeVal",  1000); }

            lowerVal = PlayerPrefs.GetInt(mode.ToString() + "ModeLowerRangeVal");
            upperVal = PlayerPrefs.GetInt(mode.ToString() + "ModeUpperRangeVal");

            lowerRange.text = lowerVal.ToString();
            upperRange.text = upperVal.ToString();

            rangeText.text = "( " + lowerVal + " to " + upperVal + " )";
        }
    }


    public void Select()
    {
        /*if (price != 0 && !PlayerPrefs.HasKey("educationalMode" + mode.ToString()))
        {
            BuyProduct(productId, "Are you sure you want to unlock '" + modeNameRaw + "' mode for " + price + " $ ?");
            return;
        }*/

        if (!PlayerPrefs.HasKey(mode.ToString() + "ModeWinnings")) { PlayerPrefs.SetInt(mode.ToString() + "ModeWinnings", 0); }
        EducationalModes.ins.modeWinnings.text = PlayerPrefs.GetInt(mode.ToString() + "ModeWinnings").ToString();

        EducationalModes.ins.DeselectAll();
        PlayerPrefs.SetString("selectedEducationalMode", mode.ToString());
        btnBg.sprite = EducationalModes.ins.selected;


        if (mode == EducationalModes.Modes.BasicNumbers)    {EducationalModes.ins.GenerateCards(new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, 4, "Whole Numbers", "1 - 10");}
        if (mode == EducationalModes.Modes.EvenNumbers)     {EducationalModes.ins.GenerateCards(new List<int>() { 2, 4, 6, 8, 10, 12, 14, 16, 18, 20 }, 4, "Even Numbers", "1 - 20");}
        if (mode == EducationalModes.Modes.OddNumbers)      {EducationalModes.ins.GenerateCards(new List<int>() { 1, 3, 5, 7, 9, 11, 13, 15, 17, 19 }, 4, "Odd Numbers", "1 - 20");}
        if (mode == EducationalModes.Modes.NegativeNumbers) {EducationalModes.ins.GenerateCardsAccordingToRange(-100, -1, 1, 50, 1, "Negative Numbers", "-1 to -100");}

        if (mode == EducationalModes.Modes.DivisibleBy)
        {
            int no = PlayerPrefs.GetInt(mode.ToString() + "ModeSelectedDivisibleBy");
            EducationalModes.ins.GenerateCardsAccordingToRange(no, no*10, no, 10, 4, "Divisible By", "( " + no + " to " + no * 10 + " ) / " + no);
        }

        if (mode == EducationalModes.Modes.Range)           {EducationalModes.ins.GenerateCardsAccordingToRange( lowerVal, upperVal, 1, 50, 1, "Range", lowerVal + " to " + upperVal);}

        if (mode == EducationalModes.Modes.RangeAndDivisibleBy)
        {
            int no = PlayerPrefs.GetInt(mode.ToString() + "ModeSelectedDivisibleBy");
            EducationalModes.ins.GenerateCardsAccordingToRange(lowerVal, upperVal, no, 50, 1, "Range & Divisible By", "( " + lowerVal + " to " + upperVal + " ) / " + no);
        }
    }

    public void Deselect()
    {
        btnBg.sprite = EducationalModes.ins.unselected;
    }

    
    void BuyProduct(string productId, string purchaseMsg)
    {
        AudioSource.PlayClipAtPoint(GameUtils.ins.btnSoundDefault, Camera.main.transform.position, .5f);
        NoticeUtils.ins.ShowTwoBtnAlert(purchaseMsg, (i) =>
        {
            if (i == 0)
            {
                PaymentGateway.ins.BuyProduct(productId, (id) =>
                {
                    locked = false;
                    PlayerPrefs.SetInt("educationalMode" + mode.ToString(), 1);
                    modeText.text = modeNameRaw;
                    Select();
                });
            }
        });
    }



    public void DivisibleByOnClick(Transform btn)
    {
        PlayerPrefs.SetInt(mode.ToString() + "ModeSelectedDivisibleBy", int.Parse(btn.GetChild(1).GetComponent<Text>().text));
        SelectDivisibleByBtn(btn);
        Select();
    }

    public void SelectDivisibleByBtn(Transform btn)
    {
        for (int i = 0; i < divisibleByBtns.childCount; i++) { divisibleByBtns.GetChild(i).GetChild(0).GetComponent<Image>().color = new Color32(36, 134, 233, 255); }
        btn.GetChild(0).GetComponent<Image>().color = new Color32(52, 190, 0, 255);
    }


    public void LowerRangeOnEndEdit()
    {
        Debug.Log(lowerRange.text);
        if (string.IsNullOrEmpty(lowerRange.text) || lowerRange.text == "-") 
        { 
            lowerRange.text = "-1000";
            NoticeUtils.ins.ShowOneBtnAlert("Value can not be empty");
        }
        
        lowerVal = int.Parse(lowerRange.text);

        if (lowerVal < -1000) 
        { 
            lowerRange.text = "-1000"; 
            NoticeUtils.ins.ShowOneBtnAlert("Value can not be less than -1000"); 
        }

        if (lowerVal > (upperVal - rangeGap)) 
        { 
            lowerRange.text = (upperVal - rangeGap).ToString(); 
            NoticeUtils.ins.ShowOneBtnAlert("Must maintain a gap of " + rangeGap + " between values"); 
        }

        lowerVal = int.Parse(lowerRange.text);
        rangeText.text = "( " + lowerVal + " to " + upperVal + " )";
        PlayerPrefs.SetInt(mode.ToString() + "ModeLowerRangeVal", lowerVal);
        Select();
    }

    public void UpperRangeOnEndEdit()
    {
        //Debug.Log(upperRange.text);
        if (string.IsNullOrEmpty(upperRange.text) || upperRange.text == "-")
        {
            upperRange.text = "1000";
            NoticeUtils.ins.ShowOneBtnAlert("Value can not be empty");
        }

        upperVal = int.Parse(upperRange.text);

        if (upperVal > 1000)
        {
            upperRange.text = "1000";
            NoticeUtils.ins.ShowOneBtnAlert("Value can not be more than 1000");
        }

        if (upperVal < (lowerVal + rangeGap))
        {
            upperRange.text = (lowerVal + rangeGap).ToString();
            NoticeUtils.ins.ShowOneBtnAlert("Must maintain a gap of " + rangeGap + " between values");
        }

        upperVal = int.Parse(upperRange.text);
        rangeText.text = "( " + lowerVal + " to " + upperVal + " )";
        PlayerPrefs.SetInt(mode.ToString() + "ModeUpperRangeVal", upperVal);
        Select();
    }
}
