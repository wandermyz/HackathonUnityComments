using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;

#if ENABLE_VR_EDITOR
public class WebVRBuilder
{
    [MenuItem("Oculus/Build WebVR")]
    public static void BuildMenuItem()
    {
        WebVRBuilder builder = new WebVRBuilder();
        builder.Build();
    }

    [MenuItem("Oculus/Share WebVR")]
    public static void ShareMenuItem()
    {
        string path = EditorUtility.OpenFolderPanel("Choose Location of WebVR Build", "", "");

        path = path.Replace("/", "\\");

        string sharedPath = Share.SyncFolderToGit(path);

        EditorGUIUtility.systemCopyBuffer = sharedPath;

        Debug.Log(sharedPath + " copied to clipboard.");
    }

    [MenuItem("Oculus/Test")]
    public static void Test()
    {
    }

    public void Build()
    {
        string path = EditorUtility.SaveFolderPanel("Choose Location of WebVR Build", "", "");

        string[] levels = new string[] {"Assets/Scenes/PrototypingTemplate.unity"};

        var options = BuildOptions.None;
        BuildPipeline.BuildPlayer(levels, path, BuildTarget.WebGL, options);
    }
}
#endif
