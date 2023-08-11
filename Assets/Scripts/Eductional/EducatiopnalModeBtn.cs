using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EducatiopnalModeBtn : MonoBehaviour
{
    public float price;
    public string productId;

    public EducationalModes.Modes mode;
    
    public Image btnBg;

    public GameObject unlockPanel;

    void Start()
    {
        if (price != 0)
        {
            if (PlayerPrefs.HasKey("EducationalMode" + mode)) { Unlock(); } else { Lock(); }
        }
        else { Unlock(); }
    }


    public void Select()
    {
        if (unlockPanel.activeInHierarchy)
        {
            BuyProduct(productId, "Are you sure you want to unlock this mode for " + price + " $ ?");
            return;
        }
        
        
        EducationalModes.ins.DeselectAll();
        
        PlayerPrefs.SetString("educationalMode", mode.ToString());
        btnBg.sprite = EducationalModes.ins.selected;

        if (mode == EducationalModes.Modes.Default)
        {
            EducationalModes.ins.deck.values = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        }

        if (mode == EducationalModes.Modes.EvenNumbers)
        {
            EducationalModes.ins.deck.values = new List<int>() { 2, 4, 6, 8, 10, 12, 14, 16, 18, 20 };
        }

        if (mode == EducationalModes.Modes.OddNumbers)
        {
            EducationalModes.ins.deck.values = new List<int>() { 1, 3, 5, 7, 9, 11, 13, 15, 17, 19 };
        }

        EducationalModes.ins.deck.CreateCards();
    }

    public void Deselect()
    {
        btnBg.sprite = EducationalModes.ins.unselected;
    }

    public void Lock()
    {
        unlockPanel.SetActive(true);
    }

    public void Unlock()
    {
        PlayerPrefs.SetInt("EducationalMode" + mode, 1);
        unlockPanel.SetActive(false);
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
                    Unlock();
                    Select();
                });
            }
        });
    }

}
