﻿using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;

public class CreateInput : MonoBehaviour {

    public InputField mainInputField;
    public GameObject commentContainer;

    void LockInput(InputField input)
    {
        if (input.text.Length > 0)
        {
            commentContainer.GetComponent<RenderComments>().CreateCommentPublic(input.text);
            Debug.Log("Text has been entered");
            Object.Destroy(transform.parent.gameObject);
        }
        else if (input.text.Length == 0)
        { }
    }

    // Use this for initialization
    void Start () {
        Debug.Log(EventSystem.current);
        EventSystem.current.SetSelectedGameObject(mainInputField.gameObject, null);
        mainInputField.OnPointerClick(new PointerEventData(EventSystem.current));
        mainInputField.onEndEdit.AddListener(delegate { LockInput(mainInputField); });
    }
}
