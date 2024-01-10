﻿using System.Collections;
using UnityEngine;

/// <summary>
/// An object that spawns a 3D axis and with public member functions to show/hide and fluidly extend each axis individually.
/// </summary>
public class DynamicAxis : MonoBehaviour
{

    [SerializeField]
    private float axisWidth;

    [SerializeField] 
    private GameObject cone;

    private GameObject xAxis;
    private GameObject yAxis;
    private GameObject zAxis;

    private GameObject xArrow;
    private GameObject yArrow;
    private GameObject zArrow;

    private GameObject xArrowTwo;
    private GameObject yArrowTwo;
    private GameObject zArrowTwo;

    private MeshRenderer xAxisRenderer;
    private MeshRenderer yAxisRenderer;
    private MeshRenderer zAxisRenderer;

    private MeshRenderer xArrowRenderer;
    private MeshRenderer yArrowRenderer;
    private MeshRenderer zArrowRenderer;

    private MeshRenderer xArrowTwoRenderer;
    private MeshRenderer yArrowTwoRenderer;
    private MeshRenderer zArrowTwoRenderer;

    /// <summary>
    /// Bool determines whether or not the axis lines emanate from the origin in one or two directions
    /// </summary>
    private bool doubleSided = true; 
    void Awake()
    {
        //Creates the axes
        xAxis = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        yAxis = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        zAxis = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        xArrow = Instantiate(cone, this.transform);
        xArrow.transform.localScale = axisWidth*Vector3.one;

        yArrow = Instantiate(cone, this.transform);
        yArrow.transform.localScale = axisWidth * Vector3.one;

        zArrow = Instantiate(cone, this.transform);
        zArrow.transform.localScale = axisWidth * Vector3.one;

        xArrowTwo = Instantiate(cone, this.transform);
        xArrowTwo.transform.localScale = axisWidth * Vector3.one;

        yArrowTwo = Instantiate(cone, this.transform);
        yArrowTwo.transform.localScale = axisWidth * Vector3.one;

        zArrowTwo = Instantiate(cone, this.transform);
        zArrowTwo.transform.localScale = axisWidth * Vector3.one;

        xAxis.transform.SetParent(this.transform);
        yAxis.transform.SetParent(this.transform);
        zAxis.transform.SetParent(this.transform);

        xAxis.transform.localScale = new Vector3(axisWidth, 1, axisWidth);
        yAxis.transform.localScale = new Vector3(axisWidth, 1, axisWidth);
        zAxis.transform.localScale = new Vector3(axisWidth, 1, axisWidth);

        xAxis.transform.localPosition = Vector3.zero;
        yAxis.transform.localPosition = Vector3.zero;
        zAxis.transform.localPosition = Vector3.zero;

        xArrow.transform.localPosition = new Vector3(1, 0, 0);
        yArrow.transform.localPosition = new Vector3(0, 1, 0);
        zArrow.transform.localPosition = new Vector3(0, 0, 1);

        xArrowTwo.transform.localPosition = new Vector3(-1, 0, 0);
        yArrowTwo.transform.localPosition = new Vector3(0, -1, 0);
        zArrowTwo.transform.localPosition = new Vector3(0, 0, -1);

        xArrow.transform.localEulerAngles = new Vector3(0, 180, 90);
        zArrow.transform.localEulerAngles = new Vector3(90, 0, 0);

        xArrowTwo.transform.localEulerAngles = new Vector3(0, 0, 90);
        yArrowTwo.transform.localEulerAngles = new Vector3(0, 0, 180);
        zArrowTwo.transform.localEulerAngles = new Vector3(90, 180, 0);

        //y axis is in proper orientation by default
        xAxis.transform.localEulerAngles = new Vector3(0, 180, 90);
        zAxis.transform.localEulerAngles = new Vector3(90, 0, 0);

        //Saves refrence to renderers (cleans up code)
        xAxisRenderer = xAxis.GetComponent<MeshRenderer>();
        yAxisRenderer = yAxis.GetComponent<MeshRenderer>();
        zAxisRenderer = zAxis.GetComponent<MeshRenderer>();

        xArrowRenderer = xArrow.GetComponent<MeshRenderer>();
        yArrowRenderer = yArrow.GetComponent<MeshRenderer>();
        zArrowRenderer = zArrow.GetComponent<MeshRenderer>();

        xArrowTwoRenderer = xArrowTwo.GetComponent<MeshRenderer>();
        yArrowTwoRenderer = yArrowTwo.GetComponent<MeshRenderer>();
        zArrowTwoRenderer = zArrowTwo.GetComponent<MeshRenderer>();

        //Colors each of the axes properly
        xAxisRenderer.material.color = Color.red;
        yAxisRenderer.material.color = Color.green;
        zAxisRenderer.material.color = Color.blue;

        xArrowRenderer.material.color = Color.red;
        yArrowRenderer.material.color = Color.green;
        zArrowRenderer.material.color = Color.blue;

        xArrowTwoRenderer.material.color = Color.red;
        yArrowTwoRenderer.material.color = Color.green;
        zArrowTwoRenderer.material.color = Color.blue;

        //Show axis by default
        ShowAxes();
    }

    //Public member functions
    private void SetAxesLength(float length, int axisNumber = -1) //set to all axes by default
    {
        if (axisNumber == -1) //all axes
        {
            xAxis.transform.localScale = new Vector3(xAxis.transform.localScale.x, length, xAxis.transform.localScale.z);
            yAxis.transform.localScale = new Vector3(yAxis.transform.localScale.x, length, yAxis.transform.localScale.z);
            zAxis.transform.localScale = new Vector3(zAxis.transform.localScale.x, length, zAxis.transform.localScale.z);
            if (!doubleSided)
            {
                xAxis.transform.localPosition = length * xAxis.transform.up;
                yAxis.transform.localPosition = length * yAxis.transform.up;
                zAxis.transform.localPosition = length * zAxis.transform.up;

                xArrow.transform.localPosition = 2*length * xAxis.transform.up;
                yArrow.transform.localPosition = 2*length * yAxis.transform.up;
                zArrow.transform.localPosition = 2*length * zAxis.transform.up;
            } else
            {
                xAxis.transform.localPosition = new Vector3(0, 0, 0);
                yAxis.transform.localPosition = new Vector3(0, 0, 0);
                zAxis.transform.localPosition = new Vector3(0, 0, 0);

                xArrow.transform.localPosition = (length) * xAxis.transform.up;
                yArrow.transform.localPosition = (length) * yAxis.transform.up;
                zArrow.transform.localPosition = (length) * zAxis.transform.up;

                xArrowTwo.transform.localPosition = -(length) * xAxis.transform.up;
                yArrowTwo.transform.localPosition = -(length) * yAxis.transform.up;
                zArrowTwo.transform.localPosition = -(length) * zAxis.transform.up;
            }
        }
        else if (axisNumber == 0) //x axis
        {
            xAxis.transform.localScale = new Vector3(xAxis.transform.localScale.x, length, xAxis.transform.localScale.z);
            if (!doubleSided)
            {
                xAxis.transform.localPosition = length * xAxis.transform.up;
            } else
            {
                xAxis.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
        else if (axisNumber == 1) //y axis
        {
            yAxis.transform.localScale = new Vector3(yAxis.transform.localScale.x, length, yAxis.transform.localScale.z);
            if (!doubleSided)
            {
                yAxis.transform.localPosition = length * yAxis.transform.up;
            } else
            {
                yAxis.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
        else if (axisNumber == 2) //z axis
        {
            zAxis.transform.localScale = new Vector3(zAxis.transform.localScale.x, length, zAxis.transform.localScale.z);
            if (!doubleSided)
            {
                zAxis.transform.localPosition = length * zAxis.transform.up;
            } else
            {
                zAxis.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
    }

    /// <summary>
    /// ExtendAxes Coroutine will Lerp between two axis lengths at a specified speed. 
    /// </summary>
    /// <param name="axisNumber"> Specifies which axis is acted on. By default all three are enabled. </param>
    /// <returns></returns>
    public IEnumerator ExtendAxes(float startLength, float endLength, float speed, int axisNumber = -1)
    {
        float timeElapsed = 0; 
        float duration = (endLength - startLength) / speed;
        while (timeElapsed < duration) //textbook lerp
        {
            yield return null;
            float t = timeElapsed / duration;
            float lerpPoint = Mathf.Lerp(startLength, endLength, t);
            SetAxesLength(lerpPoint, axisNumber);
            timeElapsed += Time.deltaTime;
        }
        SetAxesLength(endLength, axisNumber); //snaps to final position after last loop
        yield break;
    }

    public void ShowAxes(int axisNumber = -1)
    { 
        if (axisNumber == -1)
        {
            xAxisRenderer.enabled = true;
            xArrowRenderer.enabled = true;
            xArrowTwoRenderer.enabled = true;
            yAxisRenderer.enabled = true;
            yArrowRenderer.enabled = true;
            yArrowTwoRenderer.enabled = true;
            zAxisRenderer.enabled = true;
            zArrowRenderer.enabled = true;
            zArrowTwoRenderer.enabled = true;
        } 
        else if (axisNumber == 0) 
        {
            xAxisRenderer.enabled = true;
            xArrowRenderer.enabled = true;
            xArrowTwoRenderer.enabled = true;
        }
        else if (axisNumber == 1)
        {
            yAxisRenderer.enabled = true;
            yArrowRenderer.enabled = true;
            yArrowTwoRenderer.enabled = true;
        }
        else if (axisNumber == 2)
        {
            zAxisRenderer.enabled = true;
            zArrowRenderer.enabled = true;
            zArrowTwoRenderer.enabled = true;
        }
    }
    public void HideAxes(int axisNumber = -1)
    {
        if (axisNumber == -1)
        {
            xAxisRenderer.enabled = false;
            xArrowRenderer.enabled = false;
            xArrowTwoRenderer.enabled = false;
            yAxisRenderer.enabled = false;
            yArrowRenderer.enabled = false;
            yArrowTwoRenderer.enabled = false;
            zAxisRenderer.enabled = false;
            zArrowRenderer.enabled = false;
            zArrowTwoRenderer.enabled = false;

        }
        else if (axisNumber == 0)
        {
            xAxisRenderer.enabled = false;
            xArrowRenderer.enabled = false;
            xArrowTwoRenderer.enabled = false;
        }
        else if (axisNumber == 1) 
        {
            yAxisRenderer.enabled = false;
            yArrowRenderer.enabled = false;
            yArrowTwoRenderer.enabled = false;
        }
        else if (axisNumber == 2) 
        {
            zAxisRenderer.enabled = false;
            zArrowRenderer.enabled = false;
            zArrowTwoRenderer.enabled = false;
        }
    }

    /// <summary>
    /// Sets the axis to be double sided (true) or single sided (false)
    /// </summary>
    public void SetAxisMode(bool mode)
    {
        doubleSided = mode;
        SetAxesLength(xAxis.transform.localScale.y, 0);
        SetAxesLength(yAxis.transform.localScale.y, 1);
        SetAxesLength(zAxis.transform.localScale.y, 2);
        if (doubleSided)
        {
           if (xAxisRenderer.enabled)
            { 
               xArrowTwoRenderer.enabled = true;
           }
           if (yAxisRenderer.enabled)
           {
               yArrowTwoRenderer.enabled = true;
           }
           if (zAxisRenderer.enabled)
           {
               zArrowTwoRenderer.enabled = true;
           }

        } else
        {
            xArrowTwoRenderer.enabled = false;
            yArrowTwoRenderer.enabled = false;
            zArrowTwoRenderer.enabled = false;
        }
    }

    public void AxisLength(float length)
    {
        SetAxesLength(length);
    }
}

