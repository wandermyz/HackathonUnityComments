using UnityEngine;
using System.Collections;
using System.Diagnostics;

public static class Share
{
    private const string DestinationPath = "C:\\Users\\Rift\\Projects\\HackathonHost";
    private const string HostURL = "https://wandermyz.github.io/HackathonHost/";

    public static string SyncFolderToGit(string sourcePath)
    {
        UnityEngine.Debug.Log("Syncing source folder " + sourcePath + " to git...");

        Process.Start("C:\\Windows\\System32\\cmd.exe", string.Format("/C xcopy /S {0}\\* {1} /e /Y && cd {2} && git add -A && git commit -m \"Sync\" && git push -u origin master", sourcePath, DestinationPath, DestinationPath));

        UnityEngine.Debug.Log("Sync to git complete!");
        
        return HostURL;
    }
}
