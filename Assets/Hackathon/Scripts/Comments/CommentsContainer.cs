using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[System.Serializable]
public struct Comment
{
    public string id;
    public string message;
    public string positionX;
    public string positionY;
    public string positionZ;
}

[System.Serializable]
public class CommentsJSON
{
    public List<Comment> comments;
}

[ExecuteInEditMode]
public class CommentsContainer : MonoBehaviour
{
    public GameObject commentPrefab;
    public Camera camera;

    public float commentWidth = 1.0f;
    public float commentHeight = 1.0f; // game units
    public float commentDistance = 10.0f; // game units
    public float fetchInterval = 10000.0f;

    //	List<GameObject> children = new List<GameObject>();
    int commentCount = 0;

    private double lastRefetchTime = 0;
    private WWW workingWWW = null;

    // Use this for initialization
    void Start()
    {
        transform.position = camera.transform.position;
    }

    private void Update()
    {
        /*
        if (Input.GetKeyDown("space"))
        {
            Vector3 pos = camera.transform.position + (camera.transform.forward * 10);
            StartCoroutine(
                CreateComment(
                    "3",
                    "This",
                    pos
                )
            );
        }
        */
    }

    IEnumerator LoadComments()
    {
        while (true)
        {
            Debug.Log("Refetching comments...");
            WWW www = new WWW("http://hackathon-unity-comments.herokuapp.com/comments");
            yield return www;
            // string json = @"{""comments"":[{""message"": ""hello"", ""rotationX"": ""0.1"", ""rotationY"": ""0.1""}]}";
            RenderPostsFromWWW(www.text);

            yield return new WaitForSeconds(fetchInterval);
        }
    }

    void OnEnable()
    {
        Debug.Log("Comment container OnEnable");
        EditorApplication.update = OnEditorUpdate;
    }

    private void OnEditorUpdate()
    {
        if (!enabled || !gameObject.activeSelf)
        {
            return;
        }

        if (workingWWW != null)
        {
            if (!workingWWW.isDone)
            {
                return;
            }

            RenderPostsFromWWW(workingWWW.text);
            workingWWW = null;
        }

        if (EditorApplication.timeSinceStartup - lastRefetchTime < fetchInterval)
        {
            return;
        }

        Debug.Log("Refetching comments...");
        workingWWW =  new WWW("http://hackathon-unity-comments.herokuapp.com/comments");
        lastRefetchTime = EditorApplication.timeSinceStartup;
        // string json = @"{""comments"":[{""message"": ""hello"", ""rotationX"": ""0.1"", ""rotationY"": ""0.1""}]}";
    }

    IEnumerator CreateComment(string id, string message, Vector3 position)
    {
        string url = "http://hackathon-unity-comments.herokuapp.com/comments/create"
                + "?message=" + message
                + "&id=" + id
                + "&positionX=" + position.x
                + "&positionY=" + position.y
                + "&positionZ=" + position.z;
        Debug.Log(url);
        WWW www = new WWW(url);
        yield return www;
        RenderPostsFromWWW(www.text);
    }

    private void RenderPostsFromWWW(string payload)
    {
        string json = @"{""comments"":" + payload + "}";
        Debug.Log(json);
        CommentsJSON comments = JsonUtility.FromJson<CommentsJSON>(json);
        RenderPosts(comments.comments);
    }

    private void RenderPosts(List<Comment> comments)
    {
        foreach (Transform child in transform)
        {
            GameObject.DestroyImmediate(child.gameObject);
        }

        foreach (Comment comment in comments)
        {
            RenderComment(comment);
        }
    }

    private void RenderComment(Comment comment)
    {
        float scaleHeight = commentWidth;
        float scaleWidth = commentHeight;

        float px = float.Parse(comment.positionX);
        float py = float.Parse(comment.positionY);
        float pz = float.Parse(comment.positionZ);
        Debug.Log(comment.id + ' ' + comment.message + ' ' + px + ' ' + py + ' ' + pz);
        Vector3 position = new Vector3(px, py, pz);

        GameObject postInstance = Instantiate(commentPrefab, position, Quaternion.identity) as GameObject;

        postInstance.GetComponent<TextMesh>().text = comment.message;
        //postInstance.GetComponent<ShowComment>().comment = comment; // set the post data
        postInstance.transform.LookAt(transform);
        postInstance.transform.Rotate(new Vector3(0, 180, 0));
        postInstance.transform.parent = transform;
        commentCount++;
    }
}