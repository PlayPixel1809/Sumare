using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievements : MonoBehaviour
{
    public Transform achievementTabs;
    public Transform collectTrophiesPanelTrophies;

    public Text bronzeTrophyCountText;
    public Text silverTrophyCountText;
    public Text goldTrophyCountText;

    public Text nextTrophyTarget;

    [Space(30)]
    public AchievementTab activeAchievement;

    void Start()
    {
        SetGameplayAchievementsPanel(PlayerPrefs.GetString("selectedEducationalMode"));

        CasinoSumare.ins.onBetWon += (amountWon) =>
        {
            int target = int.Parse(nextTrophyTarget.text);
            nextTrophyTarget.DOCounter(target, Mathf.Clamp(target - amountWon, 0, target), 1, false).OnComplete(delegate 
            {
                if (int.Parse(nextTrophyTarget.text) == 0) 
                {
                    CasinoSumare.ins.deck.gameObject.SetActive(false);
                    CasinoSumare.ins.collectTrophyPanel.SetActive(true);
                    for (int i = 0; i < collectTrophiesPanelTrophies.childCount; i++){collectTrophiesPanelTrophies.GetChild(i).gameObject.SetActive(false);}
                   
                    if (activeAchievement.targetTrophy == "Bronze") { collectTrophiesPanelTrophies.GetChild(0).gameObject.SetActive(true); }
                    if (activeAchievement.targetTrophy == "Silver") { collectTrophiesPanelTrophies.GetChild(1).gameObject.SetActive(true); }
                    if (activeAchievement.targetTrophy == "Gold")   { collectTrophiesPanelTrophies.GetChild(2).gameObject.SetActive(true); }
                }
            });
            
        };

        EducationalModes.ins.onEducationalModeSelect += (educationalMode) =>
        {
            SetGameplayAchievementsPanel(educationalMode.mode.ToString());
        };
    }

    void SetGameplayAchievementsPanel(string educationalMode)
    {
        for (int i = 0; i < achievementTabs.childCount; i++)
        {
            activeAchievement = achievementTabs.GetChild(i).GetComponent<AchievementTab>();
            if (activeAchievement.mode.ToString() == educationalMode)
            {
                activeAchievement.CalculateTrophiesWon();

                bronzeTrophyCountText.text = "x" + activeAchievement.bronzeTrophyCount.ToString();
                silverTrophyCountText.text = "x" + activeAchievement.silverTrophyCount.ToString();
                goldTrophyCountText.text = "x" + activeAchievement.goldTrophyCount.ToString();

                bronzeTrophyCountText.color = Color.white;
                silverTrophyCountText.color = Color.white;
                silverTrophyCountText.color = Color.white;

                if (activeAchievement.targetTrophy == "Bronze") { bronzeTrophyCountText.color = Color.green; }
                if (activeAchievement.targetTrophy == "Silver") { silverTrophyCountText.color = Color.green; }
                if (activeAchievement.targetTrophy == "Gold")   { goldTrophyCountText.color = Color.green; }

                nextTrophyTarget.text = activeAchievement.nextTrophyTarget.ToString();

                break;
            }
        }
    }

    public void CollectTrophy()
    {
        CasinoSumare.ins.deck.gameObject.SetActive(true);
        CasinoSumare.ins.collectTrophyPanel.SetActive(false);
        SetGameplayAchievementsPanel(PlayerPrefs.GetString("selectedEducationalMode"));
    }
}
