using UnityEngine;
using System.Collections;

public class Comment {
    public string Message { get; set; }
    public float RotationX { get; set; }
    public float RotationY { get; set; }

    public Comment(
        string message,
        float rotationX,
        float rotationY
    )
    {
        Message = message;
        RotationX = rotationX;
        RotationY = rotationY;
    }
}
