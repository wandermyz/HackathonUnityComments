using UnityEngine;
using System.Collections;
using UnityEditor;

[ExecuteInEditMode]
public class CommentAnimator : MonoBehaviour
{
    public float mountDuration = 1;
    private Vector3 initScale;
    private double startTime = -1;

	void Start ()
	{
	    initScale = Vector3.one; 
        transform.localScale = Vector3.zero;
	    startTime = -1;
	}
	
	void Update ()
	{
	    if (Application.isPlaying)
	    {
	        OnUpdate(Time.time);
	    }
	}

    void OnEnable()
    {
        startTime = -1;
    }

    public void OnUpdate(double time)
    {
        if (startTime < 0)
        {
            startTime = time;
        }

	    double ratio = (time - startTime) / mountDuration;
	    float scale = Mathf.Lerp(0, 1, (float)ratio);

	    transform.localScale = initScale * scale;
    }
}
