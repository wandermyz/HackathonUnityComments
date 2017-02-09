using UnityEngine;
using System.Collections;
using System;

public class StereoCameraTouch : MonoBehaviour
{
    GameObject camera;
    Camera cameraMain, cameraL, cameraR;
    GameObject handL, handR;
    float eyeLFOVUpTan, eyeLFOVDownTan, eyeLFOVLeftTan, eyeLFOVRightTan;
    float eyeRFOVUpTan, eyeRFOVDownTan, eyeRFOVLeftTan, eyeRFOVRightTan;
    const float DEG2RAD = (float)(Math.PI / 180);
    Transform myTransform, cameraLTransform, cameraRTransform;
    Vector3 webVREuler = new Vector3();
    Vector3 webVRPosition = new Vector3();

    Vector3 webVRHandLEuler = new Vector3();
    Vector3 webVRHandLPosition = new Vector3();
    Vector3 webVRHandREuler = new Vector3();
    Vector3 webVRHandRPosition = new Vector3();
    Vector3 handLStartPosition;
    Vector3 handRStartPosition;

    float rotationHoriz = 0.0f;
    float rotationVert = 0.0f;

    Boolean vrControls = false;

    Vector3 myStartPosition;

    // Use this for initialization
    void Start()
    {
        camera = GameObject.Find("Camera");
        cameraMain = GameObject.Find("CameraMain").GetComponent<Camera>();
        cameraL = GameObject.Find("CameraL").GetComponent<Camera>();
        cameraR = GameObject.Find("CameraR").GetComponent<Camera>();

        handL = GameObject.Find("HandL");
        handR = GameObject.Find("HandR");

        myTransform = camera.transform;
        myStartPosition = myTransform.localPosition;
        cameraLTransform = cameraL.transform;
        cameraRTransform = cameraR.transform;

        handLStartPosition = handL.transform.localPosition;
        handRStartPosition = handR.transform.localPosition;

        //eyeL_fovUpDegrees(53.09438705444336f);
        //eyeL_fovDownDegrees(53.09438705444336f);
        //eyeL_fovLeftDegrees(47.52769470214844f);
        //eyeL_fovRightDegrees(46.63209533691406f);

        //eyeR_fovUpDegrees(53.09438705444336f);
        //eyeR_fovDownDegrees(53.09438705444336f);
        //eyeR_fovLeftDegrees(47.52769470214844f);
        //eyeR_fovRightDegrees(46.63209533691406f);

        changeMode("normal");

        Application.ExternalCall("vrInit");
    }

    // Update is called once per frame
    void Update()
    {
        if (vrControls)
        {
            var unityEuler = ConvertWebVREulerToUnity(webVREuler);
            unityEuler.x = -unityEuler.x;
            unityEuler.z = -unityEuler.z;
            myTransform.rotation = Quaternion.Euler(unityEuler);
            var pos = webVRPosition;
            pos.z *= -1;
            myTransform.localPosition = myStartPosition + pos;

            unityEuler = ConvertWebVREulerToUnity(webVRHandLEuler);
            unityEuler.x = -unityEuler.x;
            unityEuler.z = -unityEuler.z;
            handL.transform.rotation = Quaternion.Euler(unityEuler);
            pos = webVRHandLPosition;
            pos.z *= -1;
            handL.transform.localPosition = handLStartPosition + pos;

            Debug.Log("left " + unityEuler.x + " " + webVRHandLPosition.x);

            unityEuler = ConvertWebVREulerToUnity(webVRHandREuler);
            unityEuler.x = -unityEuler.x;
            unityEuler.z = -unityEuler.z;
            handR.transform.rotation = Quaternion.Euler(unityEuler);
            pos = webVRHandRPosition;
            pos.z *= -1;
            handR.transform.localPosition = handRStartPosition + pos;

            Debug.Log("left " + unityEuler.x + " " + webVRHandLPosition.x);

            StartCoroutine(WaitEndOfFrame());

        } else
        {
            float sensitivityY = 20f;
            float sensitivityX = 20f;
            float clampAngle = 80.0f;
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            rotationHoriz += mouseX * sensitivityY * Time.deltaTime;
            rotationVert -= mouseY * sensitivityX * Time.deltaTime;

            Vector3 gyroRotation = Input.gyro.rotationRateUnbiased;
            rotationVert += -gyroRotation.x;
            rotationHoriz += -gyroRotation.y;

            rotationVert = Mathf.Clamp(rotationVert, -clampAngle, clampAngle);

            myTransform.localRotation = Quaternion.Euler(rotationVert, rotationHoriz, 0.0f);
        }
    }

    // Send post render update so we can submitFrame to vrDisplay.
    IEnumerator WaitEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        Application.ExternalCall("postRender");
    }

    #region receive values form JavaScript

    void eyeL_translation_x(float val)
    {
        cameraLTransform.position.Set(val, 0, 0);
    }

    void eyeR_translation_x(float val)
    {
        cameraRTransform.position.Set(val, 0, 0);
    }

    void eyeL_fovUpDegrees(float val)
    {
        eyeLFOVUpTan = (float)Math.Tan(val * DEG2RAD) * cameraMain.nearClipPlane;
    }

    void eyeL_fovDownDegrees(float val)
    {
        eyeLFOVDownTan = -(float)Math.Tan(val * DEG2RAD) * cameraMain.nearClipPlane;
    }

    void eyeL_fovLeftDegrees(float val)
    {
        eyeLFOVLeftTan = -(float)Math.Tan(val * DEG2RAD) * cameraMain.nearClipPlane;
    }

    void eyeL_fovRightDegrees(float val)
    {
        eyeLFOVRightTan = (float)Math.Tan(val * DEG2RAD) * cameraMain.nearClipPlane;
        cameraL.projectionMatrix = PerspectiveOffCenter(
            eyeLFOVLeftTan,
            eyeLFOVRightTan,
            eyeLFOVDownTan,
            eyeLFOVUpTan,
            cameraMain.nearClipPlane,
            cameraMain.farClipPlane);
    }

    void eyeR_fovUpDegrees(float val)
    {
        eyeRFOVUpTan = (float)Math.Tan(val * DEG2RAD) * cameraMain.nearClipPlane;
    }

    void eyeR_fovDownDegrees(float val)
    {
        eyeRFOVDownTan = -(float)Math.Tan(val * DEG2RAD) * cameraMain.nearClipPlane;
    }

    void eyeR_fovLeftDegrees(float val)
    {
        eyeRFOVLeftTan = -(float)Math.Tan(val * DEG2RAD) * cameraMain.nearClipPlane;
    }

    void eyeR_fovRightDegrees(float val)
    {
        try
        {
            eyeRFOVRightTan = (float)Math.Tan(val * DEG2RAD) * cameraMain.nearClipPlane;
            var m = PerspectiveOffCenter(
                eyeRFOVLeftTan,
                eyeRFOVRightTan,
                eyeRFOVDownTan,
                eyeRFOVUpTan,
                cameraMain.nearClipPlane,
                cameraMain.farClipPlane);
            cameraR.projectionMatrix = m;
        }
        catch (Exception ex)
        {
            Application.ExternalEval("console.log('" + ex.StackTrace + "')");
        }
    }

    void euler_x(float val)
    {
        // start up vrcontrols when we start receiving positions
        vrControls = true;
        webVREuler.x = val;
    }

    void euler_y(float val)
    {
        webVREuler.y = val;
    }

    void euler_z(float val)
    {
        webVREuler.z = val;
    }

    void position_x(float val)
    {
        webVRPosition.x = val;
    }

    void position_y(float val)
    {
        webVRPosition.y = val;
    }

    void position_z(float val)
    {
        webVRPosition.z = val;
    }

    void handL_euler_X(float val) { webVRHandLEuler.x = val; }
    void handL_euler_Y(float val) { webVRHandLEuler.y = val; }
    void handL_euler_Z(float val) { webVRHandLEuler.z = val; }
    void handL_position_X(float val) { webVRHandLPosition.x = val; }
    void handL_position_Y(float val) { webVRHandLPosition.y = val; }
    void handL_position_Z(float val) { webVRHandLPosition.z = val; }

    void handR_euler_X(float val) { webVRHandREuler.x = val; }
    void handR_euler_Y(float val) { webVRHandREuler.y = val; }
    void handR_euler_Z(float val) { webVRHandREuler.z = val; }
    void handR_position_X(float val) { webVRHandRPosition.x = val; }
    void handR_position_Y(float val) { webVRHandRPosition.y = val; }
    void handR_position_Z(float val) { webVRHandRPosition.z = val; }

    void changeMode(string mode)
    {
        switch (mode)
        {
            case "normal":
                cameraMain.GetComponent<Camera>().enabled = true;
                cameraL.GetComponent<Camera>().enabled = false;
                cameraR.GetComponent<Camera>().enabled = false;
                break;
            case "vr":
                cameraMain.GetComponent<Camera>().enabled = false;
                cameraL.GetComponent<Camera>().enabled = true;
                cameraR.GetComponent<Camera>().enabled = true;
                break;
        }
    }

    #endregion

    static Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far)
    {
        float x = 2.0F * near / (right - left);
        float y = 2.0F * near / (top - bottom);
        float a = (right + left) / (right - left);
        float b = (top + bottom) / (top - bottom);
        float c = -(far + near) / (far - near);
        float d = -(2.0F * far * near) / (far - near);
        float e = -1.0F;
        Matrix4x4 m = new Matrix4x4();
        m[0, 0] = x;
        m[0, 1] = 0;
        m[0, 2] = a;
        m[0, 3] = 0;
        m[1, 0] = 0;
        m[1, 1] = y;
        m[1, 2] = b;
        m[1, 3] = 0;
        m[2, 0] = 0;
        m[2, 1] = 0;
        m[2, 2] = c;
        m[2, 3] = d;
        m[3, 0] = 0;
        m[3, 1] = 0;
        m[3, 2] = e;
        m[3, 3] = 0;
        return m;
    }



    /// <summary>
    /// Converts the given XYZ euler rotation taken from Threejs to a Unity Euler rotation
    /// </summary>
    Vector3 ConvertWebVREulerToUnity(Vector3 eulerThreejs)
    {
        eulerThreejs.x *= -1;
        eulerThreejs.z *= -1;
        Matrix4x4 threejsMatrix = CreateRotationalMatrixThreejs(ref eulerThreejs);

        Matrix4x4 unityMatrix = threejsMatrix;
        unityMatrix.m02 *= -1;
        unityMatrix.m12 *= -1;
        unityMatrix.m20 *= -1;
        unityMatrix.m21 *= -1;

        Quaternion rotation = ExtractRotationFromMatrix(ref unityMatrix);
        return rotation.eulerAngles;
    }

    /// <summary>
    /// Creates a rotation matrix for the given threejs euler rotation
    /// </summary>
    Matrix4x4 CreateRotationalMatrixThreejs(ref Vector3 eulerThreejs)
    {
        float c1 = Mathf.Cos(eulerThreejs.x);
        float c2 = Mathf.Cos(eulerThreejs.y);
        float c3 = Mathf.Cos(eulerThreejs.z);
        float s1 = Mathf.Sin(eulerThreejs.x);
        float s2 = Mathf.Sin(eulerThreejs.y);
        float s3 = Mathf.Sin(eulerThreejs.z);
        Matrix4x4 threejsMatrix = new Matrix4x4();
        threejsMatrix.m00 = c2 * c3;
        threejsMatrix.m01 = -c2 * s3;
        threejsMatrix.m02 = s2;
        threejsMatrix.m10 = c1 * s3 + c3 * s1 * s2;
        threejsMatrix.m11 = c1 * c3 - s1 * s2 * s3;
        threejsMatrix.m12 = -c2 * s1;
        threejsMatrix.m20 = s1 * s3 - c1 * c3 * s2;
        threejsMatrix.m21 = c3 * s1 + c1 * s2 * s3;
        threejsMatrix.m22 = c1 * c2;
        threejsMatrix.m33 = 1;
        return threejsMatrix;
    }

    /// <summary>
    /// Extract rotation quaternion from transform matrix.
    /// </summary>
    /// <param name="matrix">Transform matrix. This parameter is passed by reference
    /// to improve performance; no changes will be made to it.</param>
    /// <returns>
    /// Quaternion representation of rotation transform.
    /// </returns>
    Quaternion ExtractRotationFromMatrix(ref Matrix4x4 matrix)
    {
        Vector3 forward;
        forward.x = matrix.m02;
        forward.y = matrix.m12;
        forward.z = matrix.m22;

        Vector3 upwards;
        upwards.x = matrix.m01;
        upwards.y = matrix.m11;
        upwards.z = matrix.m21;

        return Quaternion.LookRotation(forward, upwards);
    }

}
