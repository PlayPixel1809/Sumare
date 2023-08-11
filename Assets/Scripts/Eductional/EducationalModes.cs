using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EducationalModes : MonoBehaviour
{
    public static EducationalModes ins;
    void Awake() { ins = this; }

    public enum Modes { Default, EvenNumbers , OddNumbers , NegativeNumbers}

    public Deck deck;

    public Sprite selected;
    public Sprite unselected;

    public Transform modesBtns;
    

    void Start()
    {
        if (!PlayerPrefs.HasKey("educationalMode")) { PlayerPrefs.SetString("educationalMode", "Default"); }

        for (int i = 0; i < modesBtns.childCount; i++)
        {
            EducatiopnalModeBtn educatiopnalModeBtn = modesBtns.GetChild(i).GetComponent<EducatiopnalModeBtn>();
            if (educatiopnalModeBtn.mode.ToString() == PlayerPrefs.GetString("educationalMode")) 
            { 
                educatiopnalModeBtn.Select();
                break;
            }
        }
    }

    public void DeselectAll()
    {
        for (int i = 0; i < modesBtns.childCount; i++)
        {
            modesBtns.GetChild(i).GetComponent<EducatiopnalModeBtn>().Deselect();
        }
    }

    
}
