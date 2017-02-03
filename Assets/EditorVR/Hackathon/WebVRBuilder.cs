using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;

#if ENABLE_VR_EDITOR
public class WebVRBuilder {

    [MenuItem("Oculus/Build WebVR")]
    public static void MenuItem()
    {
        WebVRBuilder builder = new WebVRBuilder();
        builder.Build();
    }

    [MenuItem("Oculus/Test")]
    public static void Test()
    {
    }

    public void Build()
    {
        string path = EditorUtility.SaveFolderPanel("Choose Location of WebVR Build", "", "");
        string[] levels = new string[] {"Assets/Scenes/PrototypingTemplate.unity"};

        var options = BuildOptions.Development;
        BuildPipeline.BuildPlayer(levels, path, BuildTarget.WebGL, options);
    }
}
#endif
