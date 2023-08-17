
using UnityEditor;
using UnityEngine;

public class PlayerSettingsChanger
{
    [MenuItem("PlayerSettings/CasinoSumare")]
    static void CasinoSumare()
    {
        Debug.Log("Invoke");

        PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, new Texture2D[] { Resources.Load<Texture2D>("Casino/Icon") });
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.i8already.casinosumare");
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, "com.i8already.casinosumare");
        PlayerSettings.productName = "Casino Sumare";
    }



    [MenuItem("PlayerSettings/EducationalSumare")]
    static void EducationalSumare()
    {
        Debug.Log("Invoke");

        PlayerSettings.SetIconsForTargetGroup( BuildTargetGroup.Unknown, new Texture2D []{ Resources.Load<Texture2D>("Educational/Icon") });
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.i8already.educationalsumare");
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, "com.i8already.educationalsumare");
        PlayerSettings.productName = "Educational Sumare";
    }
}
