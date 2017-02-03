using UnityEngine;
using System.Collections;

public class ObjectEventTester : MonoBehaviour
{
    private Vector3 initialScale;

	void Start ()
	{
	    initialScale = transform.localScale;
	}
	
	void Update () {
	
	}

    public void JumpUp()
    {
        StartCoroutine(jumpUpCoroutine());
    }

    private IEnumerator jumpUpCoroutine()
    {
        transform.localScale = initialScale * 1.2f;
        yield return new WaitForSeconds(0.3f);
        transform.localScale = initialScale;
    }
}
