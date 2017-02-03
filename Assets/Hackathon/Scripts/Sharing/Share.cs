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

    public static void SyncFolderToGit(string sourcePath, Action<string> onComplete)
    {
        UnityEngine.Debug.Log("Syncing source folder to git...");

        string destinationPath = string.Format("{0}\\{1}\\", DestinationPathRoot, GetRandomString(12));

        Process process = new Process();
        ProcessStartInfo startInfo = new ProcessStartInfo();
        
        startInfo.FileName = "C:\\Windows\\System32\\cmd.exe";
        startInfo.Arguments = string.Format("/C xcopy /S {0}\\* {1} /e /Y && cd {2} && git add -A && git commit -m \"Sync\" && git push", sourcePath, destinationPath, DestinationPathRoot);
        startInfo.WindowStyle = ProcessWindowStyle.Hidden;
        startInfo.UseShellExecute = false;
        startInfo.RedirectStandardOutput = true;

        process.EnableRaisingEvents = true;
        process.StartInfo = startInfo;
        process.Exited += (sender, e) =>
        {
            onComplete(destinationPath);
            UnityEngine.Debug.Log("Sync to git complete!");
        };
        process.Start();
    }

    
    public static string GetRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
