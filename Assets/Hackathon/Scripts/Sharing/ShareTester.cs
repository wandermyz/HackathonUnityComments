using UnityEngine;
using System.Collections;

public class ShareTester : MonoBehaviour {
    private const string TestSourcePath = "C:\\Users\\Rift\\test";

    // Use this for initialization
    void Start () {
        Share.SyncFolderToGit(TestSourcePath, (string path) => { UnityEngine.Debug.Log("CALLBACK!"); UnityEngine.Debug.Log(path);  });
	}

    private 
	
	// Update is called once per frame
	void Update () {
	
	}
}

// StringFilePath
