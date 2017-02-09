using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CreateComments : MonoBehaviour {
    public Canvas canvasPrefab;
    public InputField commentInputPrefab;
    public Button commentCreateButtonPrefab;

    // Use this for initialization
    void Start () {
        // set up the canvas and the button (for commenting in non-VR mode)
        Canvas canvas = Instantiate(canvasPrefab);
        Button commentButton = Instantiate(commentCreateButtonPrefab) as Button;
        commentButton.onClick.AddListener(startCommentCreation);
        commentButton.transform.parent = canvas.transform;
    }


    public void startCommentCreation()
    {
        Debug.Log("creating");
        // create the input field - set up a link back to this class 
        Canvas canvas = Instantiate(canvasPrefab);
        InputField input = Instantiate(commentInputPrefab) as InputField;
        input.GetComponent<CreateInput>().CreateComment = gameObject.GetComponent<RenderComments>().CreateCommentPublic;
        input.transform.SetParent(canvas.transform, false);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            startCommentCreation();
        }
    }
}
