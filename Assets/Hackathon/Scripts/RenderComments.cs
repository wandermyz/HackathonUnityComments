using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public struct Comment
{
    public string id;
    public string author;
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

public class RenderComments : MonoBehaviour
{

    public Canvas inputPrefab;
    public GameObject commentPrefab;
    public Camera camera;

    float commentWidth = 1.0f;
    float commentHeight = 1.0f; // game units
    float commentDistance = 10.0f; // game units

    //	List<GameObject> children = new List<GameObject>();
    int commentCount = 0;

    private Vector3 commentPos;

    // Use this for initialization
    void Start()
    {
        transform.position = camera.transform.position;
        StartCoroutine(LoadComments());
    }

    private void Update()
    {
        if (Input.GetKeyDown("tab"))
        {
            Canvas input = Instantiate(inputPrefab) as Canvas;
            input.GetComponentInChildren<CreateInput>().commentContainer = gameObject;
            commentPos = camera.transform.position + (camera.transform.forward * 10);
            /*print("tab key was pressed");
            Vector3 pos = camera.transform.position + (camera.transform.forward * 10);
            StartCoroutine(
                CreateComment(
                    "3",
                    "Eugene",
                    "This",
                    pos
                )
            );*/
        }
    }

    IEnumerator LoadComments()
    {
        Debug.Log("Loading Comment");
        WWW www = new WWW("http://hackathon-unity-comments.herokuapp.com/comments");
        yield return www;
        // string json = @"{""comments"":[{""message"": ""hello"", ""rotationX"": ""0.1"", ""rotationY"": ""0.1""}]}";
        RenderPostsFromWWW(www.text);
    }

    public void CreateCommentPublic(string message)
    {
        StartCoroutine(CreateComment("3", "Eugene", message, commentPos));
    }

    IEnumerator CreateComment(string id, string author, string message, Vector3 position)
    {
        Debug.Log("Creating Comment");
        string url = "http://hackathon-unity-comments.herokuapp.com/comments/create"
                + "?message=" + message
                + "&author=" + author
                + "&id=" + id
                + "&positionX=" + position.x
                + "&positionY=" + position.y
                + "&positionZ=" + position.z;
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
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
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
        Debug.Log(comment.id + comment.message + ' ' + px + ' ' + py + ' ' + pz);
        Vector3 position = new Vector3(px, py, pz);

        GameObject postInstance = Instantiate(commentPrefab, position, Quaternion.identity) as GameObject;

        // postInstance.GetComponent<TextMesh>().text = comment.message;
        postInstance.GetComponent<RenderComment>().comment = comment; // set the post data
        postInstance.transform.LookAt(transform);
        postInstance.transform.Rotate(new Vector3(0, 180, 0));
        postInstance.transform.parent = transform;
        commentCount++;
    }
}