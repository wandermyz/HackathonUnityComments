using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

[System.Serializable]
public struct Comment
{
    public string id;
    public string author;
    public string message;
    public string positionX;
    public string positionY;
    public string positionZ;
    public string orientX;
    public string orientY;
    public string orientZ;
    public string orientW;
}

[System.Serializable]
public class CommentsJSON
{
    public List<Comment> comments;
}

[ExecuteInEditMode]
public class RenderComments : MonoBehaviour
{
    public GameObject commentPrefab;

    public Camera camera;

    public float commentWidth = 1.0f;
    public float commentHeight = 1.0f; // game units
    public float commentDistance = 10.0f; // game units
    public float fetchInterval = 5.0f;

    //	List<GameObject> children = new List<GameObject>();
    int commentCount = 0;

    private double lastRefetchTime = 0;
    private WWW workingWWW = null;

    private readonly Dictionary<string, GameObject> commentsInScene = new Dictionary<string, GameObject>();

    // Use this for initialization
    void Start()
    {
        // the comments container starts where the camera does
        transform.position = camera.transform.position;
    }

    private void Update()
    {
        if (!Application.isPlaying)
        {
            return;
        }

        OnUpdate(Time.time);
    }

    IEnumerator LoadComments()
    {
        Debug.Log("Loading Comment");
        WWW www = new WWW("http://hackathon-unity-comments.herokuapp.com/comments");
        yield return www;
        // string json = @"{""comments"":[{""message"": ""hello"", ""rotationX"": ""0.1"", ""rotationY"": ""0.1""}]}";
        RenderPostsFromWWW(www.text);
    }

    void OnEnable()
    {
        while (transform.childCount > 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
        commentsInScene.Clear();

#if UNITY_EDITOR
        Debug.Log("Comment container OnEnable");
        EditorApplication.update = OnEditorUpdate;
#endif
    }

#if UNITY_EDITOR
    private void OnEditorUpdate()
    {
        if (!Application.isPlaying)
        {
            OnUpdate(EditorApplication.timeSinceStartup);
        }

#if ENABLE_VR_EDITOR
        WebVRBuilder.Instance.OnEditorUpdate(EditorApplication.timeSinceStartup);
#endif
    }
#endif

    private void OnUpdate(double time)
    {
        if (!enabled || !gameObject.activeSelf)
        {
            return;
        }

        foreach (Transform t in transform)
        {
            var commentAnimator = t.GetComponent<CommentAnimator>();
            if (commentAnimator != null)
            {
                commentAnimator.OnUpdate(time);
            }
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

        if (time - lastRefetchTime < fetchInterval)
        {
            return;
        }

        Debug.Log("Refetching comments...");
        workingWWW =  new WWW("https://hackathon-unity-comments.herokuapp.com/comments");
        lastRefetchTime = time;
        // string json = @"{""comments"":[{""message"": ""hello"", ""rotationX"": ""0.1"", ""rotationY"": ""0.1""}]}";
    }

    public void CreateCommentPublic(string message)
    {
        Vector3 position = camera.transform.position + camera.transform.forward * commentDistance;
        Quaternion rotation = camera.transform.rotation;
        StartCoroutine(CreateComment(GetRandomString(5), "Yang", message, position, rotation));
    }
    public static string GetRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
          .Select(s => s[(int)(Random.Range(0, s.Length))]).ToArray());
    }

    IEnumerator CreateComment(string id, string author, string message, Vector3 position, Quaternion rotation)
    {
        Debug.Log("Creating Comment");
        string url = "https://hackathon-unity-comments.herokuapp.com/comments/create" + "?message=" + message
                     + "&author=" + author
                     + "&id=" + id
                     + "&positionX=" + position.x
                     + "&positionY=" + position.y
                     + "&positionZ=" + position.z
                     + "&orientX=" + rotation.x
                     + "&orientY=" + rotation.y
                     + "&orientZ=" + rotation.z
                     + "&orientW=" + rotation.w;
        Debug.Log(url);
        WWW www = new WWW(System.Uri.EscapeUriString(url));
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
        HashSet<string> commentsToDelete = new HashSet<string>(commentsInScene.Keys);

        foreach (Comment comment in comments)
        {
            string id = comment.id;

            if (commentsToDelete.Contains(id))
            {
                commentsToDelete.Remove(id);
            }
            RenderComment(comment);
        }

        foreach (string key in commentsToDelete)
        {
            GameObject c = commentsInScene[key];
            commentsInScene.Remove(key);
            DestroyImmediate(c);
        }
    }

    private void RenderComment(Comment comment)
    {
        float scaleHeight = commentWidth;
        float scaleWidth = commentHeight;

        float px = float.Parse(comment.positionX);
        float py = float.Parse(comment.positionY);
        float pz = float.Parse(comment.positionZ);
        float rx = float.Parse(comment.orientX);
        float ry = float.Parse(comment.orientY);
        float rz = float.Parse(comment.orientZ);
        float rw = float.Parse(comment.orientW);
        Debug.Log(comment.id + comment.message + ' ' + px + ' ' + py + ' ' + pz);
        Vector3 position = new Vector3(px, py, pz);
        Quaternion rotation = new Quaternion(rx, ry, rz, rw);

        GameObject postInstance;
        if (commentsInScene.ContainsKey(comment.id))
        {
            postInstance = commentsInScene[comment.id];
        }
        else
        {
            postInstance = Instantiate(commentPrefab);
            postInstance.name = "comment_" + comment.id;
            commentsInScene.Add(comment.id, postInstance);
        }

        postInstance.GetComponent<RenderComment>().comment = comment; // set the post data
        postInstance.transform.position = position;
        postInstance.transform.rotation = rotation;
        // postInstance.transform.LookAt(transform);
        // postInstance.transform.Rotate(new Vector3(0, 180, 0));
        postInstance.transform.parent = transform;
        commentCount++;
    }
}