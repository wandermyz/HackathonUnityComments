using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.Experimental.EditorVR;
using UnityEngine.Experimental.EditorVR.Menus;

#if ENABLE_VR_EDITOR
[MainMenuItem("Build WebVR", "Oculus", "Build WebVR Projects")]
public class OculusWebVRBuildTools : IOculusTools {
    public void OnClick()
    {
        Debug.Log("Build tool clicked");
        EditorWindow.GetWindow<VRView>().Close();
        WebVRBuilder.Instance.Build("Build");       
    }
}

[MainMenuItem("Share WebVR", "Oculus", "Share WebVR Projects")]
public class OculusWebVRShareTools : IOculusTools
{
    public void OnClick()
    {
        Debug.Log("Share tool clicked");
        WebVRBuilder.Instance.ShareToGit("Build");
    }
}
#endif
