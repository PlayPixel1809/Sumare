using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System;

public class iosbuild : MonoBehaviour
{

    [PostProcessBuildAttribute(Int32.MaxValue)] //We want this code to run last!
    public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToBuildProject)
    {
#if UNITY_IOS
        if (buildTarget != BuildTarget.iOS) return; // Make sure its iOS build

        // Getting access to the xcode project file
        string projectPath = pathToBuildProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
        PBXProject pbxProject = new PBXProject();
        pbxProject.ReadFromFile(projectPath);

        // Getting the UnityFramework Target and changing build settings
        string target = pbxProject.GetUnityFrameworkTargetGuid();
        pbxProject.SetBuildProperty(target, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "NO");

        // After we're done editing the build settings we save it 
        pbxProject.WriteToFile(projectPath);
#endif
    }

}