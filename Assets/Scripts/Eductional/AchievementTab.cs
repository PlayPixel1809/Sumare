using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementTab : MonoBehaviour
{
    public EducationalModes.Modes mode;

    public int bronzeTarget = 1000;
    public int silverTarget = 5000;
    public int goldTarget = 10000;

    public Text winningsText;
    public Text bronzeTrophyCountText;
    public Text silverTrophyCountText;
    public Text goldTrophyCountText;

    [Space(30)]
    public int winnings;
    public string targetTrophy;

    public int bronzeTrophyCount;
    public int silverTrophyCount;
    public int goldTrophyCount;

    public int nextTrophyTarget;

    void Start()
    {
        CalculateTrophiesWon();
    }


    public void CalculateTrophiesWon()
    {
        if (!PlayerPrefs.HasKey(mode.ToString() + "ModeWinnings")) { return; }

        bronzeTrophyCount = 0;
        silverTrophyCount = 0;
        goldTrophyCount = 0;

        winnings = PlayerPrefs.GetInt(mode.ToString() + "ModeWinnings");
        winningsText.text = winnings.ToString();

        targetTrophy = "Bronze";
        while (true)
        {
            if (targetTrophy == "Bronze") 
            {
                winnings -= bronzeTarget;
                if (winnings >= 0) 
                { 
                    bronzeTrophyCount += 1;
                    targetTrophy = "Silver";
                    continue;
                }
                break;
            }

            if (targetTrophy == "Silver")
            {
                winnings -= silverTarget;
                if (winnings >= 0) 
                { 
                    silverTrophyCount += 1;
                    targetTrophy = "Gold";
                    continue;
                }
                break;
            }

            if (targetTrophy == "Gold")
            {
                winnings -= goldTarget;
                if (winnings >= 0) 
                { 
                    goldTrophyCount += 1;
                    targetTrophy = "Bronze";
                    continue;
                }
                break;
            }
        }

        bronzeTrophyCountText.text = "x" + bronzeTrophyCount.ToString();
        silverTrophyCountText.text = "x" + silverTrophyCount.ToString();
        goldTrophyCountText.text = "x" + goldTrophyCount.ToString();


        nextTrophyTarget = Mathf.Abs(winnings);
    }
}
