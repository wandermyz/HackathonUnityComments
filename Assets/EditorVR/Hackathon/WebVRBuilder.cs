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
        instance.ShareToGit();
    }

    [MenuItem("Oculus/Test")]
    public static void Test()
    {
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
        if (path == null)
        {
            path = EditorUtility.OpenFolderPanel("Choose Location of WebVR Build", "", "");
        }
        path = path.Replace("/", "\\");
        path = Path.GetFullPath(path);

        string sharedPath = Share.SyncFolderToGit(path);
        EditorGUIUtility.systemCopyBuffer = sharedPath;
        Debug.Log(sharedPath + " copied to clipboard.");
    }
}
#endif
