using UnityEngine;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

public static class Share
{
    private const string DestinationPathRoot = "C:\\Users\\Rift\\Projects\\HackathonHost";
    private const string HostURL = "https://wandermyz.github.io/HackathonHost/";
    private static System.Random random = new System.Random();

    public static string SyncFolderToGit(string sourcePath, Action<string> onComplete)
    {
        UnityEngine.Debug.Log("Syncing source folder to git...");

        string subfolder = GetRandomString(12);
        string destinationPath = string.Format("{0}\\{1}\\", DestinationPathRoot, subfolder);
        string url = HostURL + subfolder + "/index.html";

        Process process = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo();
        
        startInfo.FileName = "C:\\Windows\\System32\\cmd.exe";
        startInfo.Arguments = string.Format("/C xcopy /S {0}\\* {1} /e /Y && cd {2} && git add -A && git commit -m \"Share {3}\" && git pull --rebase && git push", sourcePath, destinationPath, DestinationPathRoot, subfolder);
        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
        startInfo.UseShellExecute = false;
        startInfo.RedirectStandardOutput = true;

        process.EnableRaisingEvents = true;
        process.StartInfo = startInfo;
        process.Exited += (sender, e) =>
        {
            onComplete(url);
            UnityEngine.Debug.Log("Sync to git complete! " + url);
        };
        process.Start();

        return url;
    }

    
    public static string GetRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
