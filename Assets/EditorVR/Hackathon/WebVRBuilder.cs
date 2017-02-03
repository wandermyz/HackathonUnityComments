using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;

#if ENABLE_VR_EDITOR
public class WebVRBuilder
{
    private static WebVRBuilder instance = null;

    public static WebVRBuilder Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new WebVRBuilder();
            }

            return instance;
        }
    }

    [MenuItem("Oculus/Build WebVR")]
    public static void BuildMenuItem()
    {
        Instance.Build();
    }

    [MenuItem("Oculus/Share WebVR")]
    public static void ShareMenuItem()
    {
        Instance.ShareToGit();
    }

    [MenuItem("Oculus/Test")]
    public static void Test()
    {
    }

    private string sharedUrl;

    public void OnEditorUpdate(double time)
    {
        if (sharedUrl != null)
        {
            Debug.Log("Share sync done on main thread");
            Application.OpenURL(sharedUrl);
            sharedUrl = null;
        }
    }

    public void Build(string path = null)
    {
        if (path == null)
        {
            path = EditorUtility.SaveFolderPanel("Choose Location of WebVR Build", "", "");
        }

        path = Path.GetFullPath(path);

        string[] levels = new string[] {"Assets/Scenes/PrototypingTemplate.unity"};

        var options = BuildOptions.None;
        BuildPipeline.BuildPlayer(levels, path, BuildTarget.WebGL, options);
    }

    public void ShareToGit(string path = null)
    {
        sharedUrl = null;

        if (path == null)
        {
            path = EditorUtility.OpenFolderPanel("Choose Location of WebVR Build", "", "");
        }
        path = path.Replace("/", "\\");
        path = Path.GetFullPath(path);

        string dest = Share.SyncFolderToGit(path, (url) =>
        {
            Debug.Log("Sync done");

            sharedUrl = url;
        });

        EditorGUIUtility.systemCopyBuffer = dest;
    }
}
#endif
