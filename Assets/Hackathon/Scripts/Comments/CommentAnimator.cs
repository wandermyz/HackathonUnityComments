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
	    initScale = transform.localScale;
	    startTime = -1;
	}
	
	void Update ()
	{
	    if (!Application.isEditor)
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

        if (ratio > 1)
        {
            return;
        }

	    float scale = Mathf.Lerp(0, 1, (float)ratio);
	    transform.localScale = initScale * scale;
    }
}
