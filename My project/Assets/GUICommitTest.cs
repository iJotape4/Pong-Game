using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

public class GUICommitTest : MonoBehaviour
{
    public const string INFO_FILE_NAME = "Version Info.txt";

    private string _date;
    private bool _existGITInfo;
    private string _branchInfo;
    private string _commitInfo;
    private bool _gitInfoOpen;
    private const float height = 20;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        if (Application.platform == RuntimePlatform.WindowsEditor)
            ReadInfoInEditor();
        else
            ReadInfoFromFile();
    }
    #region ShowInfo
    private void ReadInfoInEditor()
    {
        string[] versionInfo = GetVersionInfo();
        _date = versionInfo[0];
        _commitInfo = versionInfo[1];
        _branchInfo = versionInfo[2];
        _existGITInfo = versionInfo[1].Length > 0;
    }

    private void ReadInfoFromFile()
    {
        string filePath = GetInfoFilePathInBuild();

        if (!File.Exists(filePath))
            return;

        string infoContent = File.ReadAllText(filePath).Trim();
        if (infoContent.Contains("|")) // if true, it has GIT Info
        {
            string[] parts = infoContent.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            Debug.Assert(parts.Length == 3, "VersionInfoLabel.LoadInfoInFile: La informacion almacenada en el archivo " + INFO_FILE_NAME + " no es valida.");
            _date = parts[0];
            _commitInfo = parts[1];
            _branchInfo = parts[2];
            _existGITInfo = true;
        }
        else
        {
            _date = infoContent;
            _existGITInfo = false;
        }
    }

    private string GetInfoFilePathInBuild()
    {
        string dataPath = Application.dataPath;
        string[] parts = dataPath.Split(new char[] { '/' });
        string filePath = "";
        for (int i = 0; i < parts.Length - 1; i++)
        {
            filePath += parts[i] + "/";
        }
        filePath += INFO_FILE_NAME;

        return filePath;
    }

#if TEST_BUILD

    void OnGUI()
    {
        float width = 200;
        int boxSize = 3;
        GUI.Box(new Rect(Screen.width - width - 3, 0, width, boxSize * height), "");
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            GUI.Label(new Rect(Screen.width - width, 0, width, height), "Play Date: " + _date);
        }
        else
        {
            GUI.Label(new Rect(Screen.width - width, 0, width, height), "Build Date: " + _date);
        }

        GUI.Label(new Rect(Screen.width - width, 1 * height, width, height), "Commit: " + _commitInfo);
        GUI.Label(new Rect(Screen.width - width, 2 * height, width, height), "Branch: " + _branchInfo);


    }

#endif

    #endregion

    #region Get Info
    public string[] GetVersionInfo()
    {
        string date = GetDateInfo();

        string gitFolderPath = GetGitFolderPath();
        bool existGITInfo = gitFolderPath != null;
        string commitInfo = "";
        string branchInfo = "";
        if (existGITInfo) // if != , the folder exists
        {
            commitInfo = GetCommitInfo(gitFolderPath);
            branchInfo = GetBranchInfo(gitFolderPath);
        }

        return new string[] { date, commitInfo, branchInfo };
    }

    private string GetDateInfo()
    {
        DateTime localDate = DateTime.Now;
        var culture = new CultureInfo("en-GB");
        return localDate.ToString(culture);
    }

    private string GetGitFolderPath()
    {
        string appDataPath = Application.dataPath;
        string[] splits = appDataPath.Split(new char[] { '/' });
        int numSplits = splits.Length - 2;

        if (numSplits <= 0)
            return null;

        string gitFolderPath = "";
        for (int i = 0; i < numSplits; i++)
        {
            gitFolderPath += splits[i] + "/";
        }
        gitFolderPath += ".git";

        bool existGITInfo = Directory.Exists(gitFolderPath);

        if (!existGITInfo)
            return null;
        else
            return gitFolderPath;
    }

    private string GetCommitInfo(string gitFolderPath)
    {
        string headBranchPath = GetHeadBranchPath(gitFolderPath);
        string commitInfoPath = gitFolderPath + "/" + headBranchPath;
        Debug.Log("commitInfoPath: " + commitInfoPath);

        if (!File.Exists(commitInfoPath))
        {
            var result = TryFindInPackedRefs(headBranchPath, gitFolderPath);
            return result;
        }

        string commitInfo = File.ReadAllText(commitInfoPath).Trim();
        return commitInfo.Substring(0, 9);
    }

    private string GetHeadBranchPath(string gitFolderPath)
    {
        var headFilePath = gitFolderPath + "/HEAD";
        var headBranchPath = File.ReadAllText(headFilePath).Substring(5).Trim();
        return headBranchPath;
    }
    private string TryFindInPackedRefs(string headBranchPath, string gitFolderPath)
    {
        var result = "Not Found!";

        var packedRef = gitFolderPath + "/packed-refs";
        var txtlines = File.ReadLines(packedRef);
        var line = txtlines.FirstOrDefault(x => x.Contains(headBranchPath));
        if (line != null)
        {
            result = line.Substring(0, 41);
            if (result.Length > 10) //If the text is very long, it isn't shown by the label. Because of this, text is being cutted. 
                result = result.Substring(0, 9) + "...";
        }

        return result;

    }
    private string GetBranchInfo(string gitFolderPath)
    {
        string headInfoPath = gitFolderPath + "/HEAD";
        string branchInfo = File.ReadAllText(headInfoPath).Trim();
        branchInfo = branchInfo.Substring(16);
        if (branchInfo.StartsWith("feature"))
        {
            branchInfo = branchInfo.Substring(8);
        }
        if (branchInfo.Length > 20) // If the text is very long, it isn't shown by the label. Because of this, text is being cutted. 
            branchInfo = branchInfo.Substring(0, 20) + "...";

        return branchInfo;
    }
    #endregion

}