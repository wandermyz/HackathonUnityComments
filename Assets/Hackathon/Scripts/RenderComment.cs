using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class RenderComment : MonoBehaviour {

    public Comment comment;

    public Material agatha;
    public Material eugene;
    public Material kevin;

	// Use this for initialization
    void Start()
    {
        Dictionary<string, Material> materials = new Dictionary<string, Material>()
        {
            {"Agatha", agatha},
            {"Eugene", eugene},
            {"Kevin", kevin}
        };

        GameObject author = transform.FindChild("Author").gameObject;
        author.GetComponent<TextMesh>().text = comment.author;

        GameObject commentObj = transform.FindChild("Comment").gameObject;
        string formattedMessage = ResolveTextSize(comment.message);
        commentObj.GetComponent<TextMesh>().text = formattedMessage;
        int numLines = getNumLines(formattedMessage);

        GameObject card = transform.FindChild("Card").gameObject;
        card.transform.localScale = new Vector3(1, 1 + numLines * 0.25f, 1);

        GameObject image = transform.FindChild("Image").gameObject;

        /*
        image.GetComponent<Renderer>().material.mainTexture =
            (Texture2D) AssetDatabase.LoadAssetAtPath("Assets/Hackathon/Textures/" + comment.author + ".jpg", typeof(Texture2D));
            */

        if (materials.ContainsKey(comment.author))
        {
            image.GetComponent<Renderer>().material = materials[comment.author];
        }
    }

    // Wrap text by line height
    private string ResolveTextSize(string input)
    {
        int lineLength = 20;

        // Split string by char " "         
        string[] words = input.Split(" "[0]);

        // Prepare result
        string result = "";

        // Temp line string
        string line = "";

        // for each all words        
        foreach (string s in words)
        {
            // Append current word into line
            string temp = line + " " + s;

            // If line length is bigger than lineLength
            if (temp.Length > lineLength)
            {

                // Append current line into result
                result += line + "\n";
                // Remain word append into new line
                line = s;
            }
            // Append current word into current line
            else
            {
                line = temp;
            }
        }

        // Append last line into result        
        result += line;

        // Remove first " " char
        return result.Substring(1, result.Length - 1);
    }


    private int getNumLines(string input)
    {
        return input.Split('\n').Length;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
