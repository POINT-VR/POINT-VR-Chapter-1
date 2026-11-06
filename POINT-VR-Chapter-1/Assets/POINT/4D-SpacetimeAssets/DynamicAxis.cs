using System.Collections;
using UnityEngine;

/// <summary>
/// An object that spawns a 3D axis and with public member functions to show/hide and fluidly extend each axis individually.
/// </summary>
public class DynamicAxis : MonoBehaviour
{
    [Tooltip("Radius of cylinders representing the axes")]
    [SerializeField] private float axisWidth;

    [Tooltip("Cylinder prefab for spawning axes")]
    [SerializeField] private GameObject axisObject;

    [Tooltip("Cone prefab for spawning arrow tips")]
    [SerializeField] private GameObject cone;

    [Tooltip("Cone prefab for spawning arrow tips")]
    [SerializeField] private Material axisMaterial;

    const int NUM_DIMENSIONS = 3;

    private readonly GameObject[] axes = new GameObject[NUM_DIMENSIONS];
    private readonly GameObject[] positiveArrows = new GameObject[NUM_DIMENSIONS];
    private readonly GameObject[] negativeArrows = new GameObject[NUM_DIMENSIONS];

    //Axis and arrow renderers, used to more easily hide objects 
    private readonly MeshRenderer[] axesAndArrowRenderers = new MeshRenderer[3 * NUM_DIMENSIONS];

    private readonly Color[] axesColors = { Color.red, Color.blue, Color.green };
    
    //Calls awake rather than start so that set up is performed instantly
    void Awake()
    {
        Vector3 axisScale = new Vector3(axisWidth, 1, axisWidth);

        // Iterate through the dimensions in order: x, y, z
        for (int i = 0; i < NUM_DIMENSIONS; i++)
        {
            // Creates axis
            axes[i] = Instantiate(axisObject);
            axes[i].transform.SetParent(this.transform);

            // Create arrows
            positiveArrows[i] = Instantiate(cone, this.transform);
            negativeArrows[i] = Instantiate(cone, this.transform);

            // Scale elements
            axes[i].transform.localScale = axisScale;
            positiveArrows[i].transform.localScale = axisWidth * Vector3.one;
            negativeArrows[i].transform.localScale = axisWidth * Vector3.one;

            // Position elements
            axes[i].transform.localPosition = Vector3.zero;
            Vector3 positiveArrowPosition = Vector3.zero;
            positiveArrowPosition[i] = 1.0f;
            positiveArrows[i].transform.localPosition = positiveArrowPosition;
            negativeArrows[i].transform.localPosition = -positiveArrowPosition;

            // Rotate elements
            switch (i)
            {
                case 0:
                    axes[i].transform.localEulerAngles = new Vector3(0, 180, 90);
                    positiveArrows[i].transform.localEulerAngles = new Vector3(0, 180, 90);
                    negativeArrows[i].transform.localEulerAngles = new Vector3(0, 0, 90);
                    break;
                case 1:
                    negativeArrows[i].transform.localEulerAngles = new Vector3(0, 0, 180);
                    break;
                case 2:
                    axes[i].transform.localEulerAngles = new Vector3(90, 0, 0);
                    positiveArrows[i].transform.localEulerAngles = new Vector3(90, 0, 0);
                    negativeArrows[i].transform.localEulerAngles = new Vector3(90, 180, 0);
                    break;
                default:
                    Debug.LogError("More than 3 spatial dimensions detected.");
                    break;
            }

            // Save references to renderers
            axesAndArrowRenderers[NUM_DIMENSIONS * i] = axes[i].GetComponent<MeshRenderer>();
            axesAndArrowRenderers[(NUM_DIMENSIONS * i) + 1] = positiveArrows[i].GetComponent<MeshRenderer>();
            axesAndArrowRenderers[(NUM_DIMENSIONS * i) + 2] = negativeArrows[i].GetComponent<MeshRenderer>();
        }

        SetAxisMaterial(axisMaterial);
        for (int i = 0; i < 3 * NUM_DIMENSIONS; i++)
        {
            axesAndArrowRenderers[i].material.renderQueue--;
            axesAndArrowRenderers[i].material.color = axesColors[i / NUM_DIMENSIONS];
        }

        //Shows dynamic axis by default
        ShowAxes();
    }

    // Public member functions
    private void SetAxesLength(float length, int axisNumber = -1) //set to all axes by default
    {
        if (axisNumber == -1) //all axes
        {
            for (int i = 0; i < NUM_DIMENSIONS; i++)
            {
                axes[i].transform.localScale = new Vector3(axes[i].transform.localScale.x, length, axes[i].transform.localScale.z);
                axes[i].transform.localPosition = Vector3.zero;
                positiveArrows[i].transform.localPosition = length * axes[i].transform.up;
                negativeArrows[i].transform.localPosition = -length * axes[i].transform.up;
            }
        }
        else
        {
            axes[axisNumber].transform.localScale = new Vector3(axes[axisNumber].transform.localScale.x, length, axes[axisNumber].transform.localScale.z);
            axes[axisNumber].transform.localPosition = Vector3.zero;
            positiveArrows[axisNumber].transform.localPosition = length * axes[axisNumber].transform.up;
            negativeArrows[axisNumber].transform.localPosition = -length * axes[axisNumber].transform.up;
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

    /// <summary>
    /// Lerps material between two colors
    /// </summary>
    /// <param name="endColor"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public IEnumerator TransitionAxisColor(Color endColor, float duration)
    {
        float timeElapsed = 0;
        Color[] currentAxesColors = new Color[NUM_DIMENSIONS];
        for (int i = 0; i < NUM_DIMENSIONS; i++)
        {
            currentAxesColors[i] = axesAndArrowRenderers[NUM_DIMENSIONS * i].material.color;
        }

        while (timeElapsed < duration)
        {
            yield return null;
            float t = timeElapsed / duration;

            for (int i = 0; i < 3 * NUM_DIMENSIONS; i++)
            {
                axesAndArrowRenderers[i].material.color = Color.Lerp(currentAxesColors[i / NUM_DIMENSIONS], endColor, t);
            }

            timeElapsed += Time.deltaTime;
        }

        // Set to final color after last loop
        foreach (MeshRenderer meshRenderer in axesAndArrowRenderers)
        {
            meshRenderer.material.color = endColor;
        }

        yield break;
    }

    /// <summary>
    /// Lerps thickness of the axes
    /// </summary>
    /// <returns></returns>
    public IEnumerator TransitionAxisThickness(float endThickness, float duration)
    {
        float timeElapsed = 0;
        float startThickness = axes[0].transform.localScale.x;
        while (timeElapsed < duration) //textbook lerp
        {
            yield return null;
            float t = timeElapsed / duration;
            float lerpPoint = Mathf.Lerp(startThickness, endThickness, t);

            for (int i = 0; i < NUM_DIMENSIONS; i++)
            {
                axes[i].transform.localScale = new Vector3(lerpPoint, axes[i].transform.localScale.y, lerpPoint);
            }

            timeElapsed += Time.deltaTime;
        }

        for (int i = 0; i < NUM_DIMENSIONS; i++)
        {
            axes[i].transform.localScale = new Vector3(endThickness, axes[i].transform.localScale.y, endThickness);
        }

        yield break;
    }

    /// <summary>
    /// Shows or hides the entire dynamic axis (Axes and arrows)
    /// </summary>
    /// <param name="axisNumber"></param>
    /// <param name="shouldShow"></param>
    public void ShowAxes(int axisNumber = -1, bool shouldShow = true)
    { 
        if (axisNumber == -1)
        {
            foreach (MeshRenderer meshRenderer in axesAndArrowRenderers)
            {
                meshRenderer.enabled = shouldShow;
            }
        }
        else
        {
            axesAndArrowRenderers[axisNumber * NUM_DIMENSIONS].enabled = shouldShow;
            axesAndArrowRenderers[(axisNumber * NUM_DIMENSIONS) + 1].enabled = shouldShow;
            axesAndArrowRenderers[(axisNumber * NUM_DIMENSIONS) + 2].enabled = shouldShow;
        }
    }

    public void SetAxisMaterial(Material material)
    {
        foreach (MeshRenderer meshRenderer in axesAndArrowRenderers)
        {
            meshRenderer.material = material;
        }
    }

    public void AxisLength(float length)
    {
        SetAxesLength(length);
    }
}