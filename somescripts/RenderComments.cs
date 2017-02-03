using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommentsJSON
{
    public List<Comment> comments;
}

public class RenderComments : MonoBehaviour
{

    public GameObject commentPrefab;

    float commentWidth = 1.0f;
    float commentHeight = 1.0f; // game units
    float commentDistance = 10.0f; // game units

    //	List<GameObject> children = new List<GameObject>();
    int commentCount = 0;
    GameObject commentContainer;

    // Use this for initialization
    void Start()
    {
        commentContainer = gameObject;
        StartCoroutine(LoadComments());
    }

    IEnumerator LoadComments()
    {
        WWW www = new WWW("http://hackathon-unity-comments.herokuapp.com/comments");
        yield return www;

        Debug.Log(www.text);

        string json = "{comments:" + www.text + "}";
        CommentsJSON comments = JsonUtility.FromJson<CommentsJSON>(json);
        RenderPosts(comments.comments);
    }

    IEnumerator CreateComment(string message, float rotationX, float rotationY)
    {
        WWW www = new WWW("http://hackathon-unity-comments.herokuapp.com/comment?Message=" + message + "&RotationX=" + rotationX + "&RotationY=" + rotationY);
        yield return www;
    }

    private void RenderPosts(List<Comment> comments)
    {
        foreach (Comment comment in comments)
        {
            CreateComment(comment);
        }
    }

    private void CreateComment(Comment comment)
    {
        float scaleHeight = commentWidth;
        float scaleWidth = commentHeight;

        Debug.Log(comment.Message);
        Quaternion rotation = Quaternion.Euler(comment.RotationX, comment.RotationY, 0);
        Vector3 position = new Vector3(
            Mathf.Sin(30 * Mathf.Deg2Rad),
            0,
            Mathf.Cos(30 * Mathf.Deg2Rad)
        ) * commentDistance;

        GameObject postInstance = Instantiate(commentPrefab, position, rotation) as GameObject;
        postInstance.GetComponent<ShowComment>().comment = comment; // set the post data
        postInstance.transform.parent = commentContainer.transform;
        commentCount++;
    }
}