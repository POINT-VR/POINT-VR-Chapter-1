using System.Collections;
using UnityEngine;

/// <summary>
/// An object for the 4D spacetime introduction that spawns a 3D axis and with public member functions to show/hide and fluidly extend each axis individually.
/// </summary>
public class DynamicAxis : MonoBehaviour
{

    [SerializeField]
    private float axisWidth;

    private GameObject xAxis;
    private GameObject yAxis;
    private GameObject zAxis;

    private MeshRenderer xAxisRenderer;
    private MeshRenderer yAxisRenderer;
    private MeshRenderer zAxisRenderer;

    // Start is called before the first frame update
    void Start()
    {
        //Creates the axes
        xAxis = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        yAxis = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        zAxis = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        xAxis.transform.SetParent(this.transform);
        yAxis.transform.SetParent(this.transform);
        zAxis.transform.SetParent(this.transform);

        xAxis.transform.localScale = new Vector3(axisWidth, 1, axisWidth);
        yAxis.transform.localScale = new Vector3(axisWidth, 1, axisWidth);
        zAxis.transform.localScale = new Vector3(axisWidth, 1, axisWidth);

        //y axis is in proper orientation by default
        xAxis.transform.localEulerAngles = new Vector3(90, 0, 0); 
        zAxis.transform.localEulerAngles = new Vector3(0, 180, 90);

        //Saves refrence to renderers (cleans up code)
        xAxisRenderer = xAxis.GetComponent<MeshRenderer>();
        yAxisRenderer = yAxis.GetComponent<MeshRenderer>();
        zAxisRenderer = zAxis.GetComponent<MeshRenderer>();

        //Hides axis by default
        HideAxes();

        //Colors each of the axes properly
        xAxisRenderer.material.color = Color.red;
        yAxisRenderer.material.color = Color.green;
        zAxisRenderer.material.color = Color.blue;
    }

    /// <summary>
    /// ExtendAxes Coroutine will Lerp between two axis lengths at a specified speed. 
    /// </summary>
    /// <param name="doubleSided"> Determines whether the axes extend in both directions or only one. </param>
    /// <param name="axisNumber"> Specifies which axis is acted on. By default all three are enabled. </param>
    /// <returns></returns>
    public IEnumerator ExtendAxes(float startLength, float endLength, float speed, bool doubleSided, int axisNumber = -1)
    {
        float timeElapsed = 0; 
        float duration = (endLength - startLength) / speed;
        while (timeElapsed < duration) //textbook lerp
        {
            yield return null;
            float t = timeElapsed / duration;
            float lerpPoint = Mathf.Lerp(startLength, endLength, t);
            SetAxesLength(lerpPoint, doubleSided, axisNumber);
            timeElapsed += Time.deltaTime;
        }
        SetAxesLength(endLength, doubleSided, axisNumber); //snaps to final position after last loop
        yield break;
    }

    public void ShowAxes(int axisNumber = -1)
    { 
        if (axisNumber == -1)
        {
            xAxisRenderer.enabled = true;
            yAxisRenderer.enabled = true;
            zAxisRenderer.enabled = true;
        } 
        else if (axisNumber == 0) 
        {
            xAxisRenderer.enabled = true;
        }
        else if (axisNumber == 1)
        {
            yAxisRenderer.enabled = true;
        }
        else if (axisNumber == 2)
        {
            zAxisRenderer.enabled = true;
        }
    }
    public void HideAxes(int axisNumber = -1)
    {
        if (axisNumber == -1)
        {
            xAxisRenderer.enabled = false;
            yAxisRenderer.enabled = false;
            zAxisRenderer.enabled = false;
        }
        else if (axisNumber == 0)
        {
            xAxisRenderer.enabled = false;
        }
        else if (axisNumber == 1) 
        {
            yAxisRenderer.enabled = false;
        }
        else if (axisNumber == 2) 
        {
            zAxisRenderer.enabled = false;
        }
    }
    private void SetAxesLength(float length, bool doubleSided, int axisNumber = -1) //set to all axes by default
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
            }
        }
        else if (axisNumber == 0) //x axis
        {
            xAxis.transform.localScale = new Vector3(xAxis.transform.localScale.x, length, xAxis.transform.localScale.z);
            if (!doubleSided)
            {
                xAxis.transform.localPosition = length * xAxis.transform.up;
            }
        }
        else if (axisNumber == 1) //y axis
        {
            yAxis.transform.localScale = new Vector3(yAxis.transform.localScale.x, length, yAxis.transform.localScale.z);
            if (!doubleSided)
            {
                yAxis.transform.localPosition = length * yAxis.transform.up;
            }
        }
        else if (axisNumber == 2) //z axis
        {
            zAxis.transform.localScale = new Vector3(zAxis.transform.localScale.x, length, zAxis.transform.localScale.z);
            if (!doubleSided)
            {
                zAxis.transform.localPosition = length * zAxis.transform.up;
            }
        }
    }
}

