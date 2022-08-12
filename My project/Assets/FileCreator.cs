using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
public class FileCreator : IPreprocessBuildWithReport
{

    int IOrderedCallback.callbackOrder { get { return 0; } }

    void IPreprocessBuildWithReport.OnPreprocessBuild(BuildReport report)
    {
#if TEST_BUILD
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows)
        {
            Debug.Log("<color=red>  The build was done on windows x86. The version Info File only is created in Windows x64. Change it on BuildSettings</color>");
        }
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.StandaloneWindows64)
        {
            GUICommitTest infoGetter = new GUICommitTest();
            string[] versionInfo = infoGetter.GetVersionInfo();
            SaveInfoInFile(date: versionInfo[0], commitInfo: versionInfo[1], branchInfo: versionInfo[2]);

        }
#endif
    }
#if TEST_BUILD
    private void SaveInfoInFile(string date, string commitInfo, string branchInfo)
    {
        string filePath = GetInfoFilePathInBuild();
        StreamWriter writer = File.CreateText(filePath);
        if (commitInfo.Length > 0)  // si esto se cumple es porque existe informacion de GIT.
            writer.WriteLine(date + "|" + commitInfo + "|" + branchInfo);
        else
            writer.WriteLine(date);

        writer.Close();
    }

    private string GetInfoFilePathInBuild()
    {
        string buildPath = EditorUserBuildSettings.GetBuildLocation(BuildTarget.StandaloneWindows64);

        string[] parts = buildPath.Split(new char[] { '\\' });
        string filePath = parts[0] + "/" + GUICommitTest.INFO_FILE_NAME;
        return filePath;
    }
#endif
	}
#endif