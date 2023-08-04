using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AxisScriptTwo : MonoBehaviour
{
    public MeshRenderer xAxis;
    public MeshRenderer yAxis;
    public MeshRenderer zAxis;
    public float startLength;
    public float endLength;
    public float speed;

    float timeElapsed = 0;
    float duration;
    // Start is called before the first frame update
    void Start()
    {
        duration = (endLength - startLength) / speed;
        SetAxisLength(startLength);
        xAxis.material.color = Color.red;
        yAxis.material.color = Color.green;
        zAxis.material.color = Color.blue;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeElapsed < duration)
        {
            float t = timeElapsed / duration;
            float lerpPoint = Mathf.Lerp(startLength, endLength, t);
            timeElapsed += Time.deltaTime;
            SetAxisLength(lerpPoint);
        }
        else
        {
            SetAxisLength(endLength);
        }
    }
    void SetAxisLength(float length)
    {
        xAxis.transform.localScale = new Vector3(xAxis.transform.localScale.x, length, xAxis.transform.localScale.z);
        xAxis.transform.localPosition = length * xAxis.transform.up;

        yAxis.transform.localScale = new Vector3(yAxis.transform.localScale.x, length, yAxis.transform.localScale.z);
        yAxis.transform.localPosition = length * yAxis.transform.up;

        zAxis.transform.localScale = new Vector3(zAxis.transform.localScale.x, length, zAxis.transform.localScale.z);
        zAxis.transform.localPosition = length * zAxis.transform.up;
    }
}

