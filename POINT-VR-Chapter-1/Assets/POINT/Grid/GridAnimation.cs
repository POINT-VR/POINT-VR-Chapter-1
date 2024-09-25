using System.Collections;
using UnityEngine;

public class GridAnimation : MonoBehaviour
{
    [Header("Object references")]
    [Tooltip("Growable axis object in the current scene")]
    [SerializeField] private DynamicAxis dynamicAxis = null;

    [Tooltip("A large static grid that cannot deform")]
    [SerializeField] private GameObject staticGrid = null;

    [Tooltip("The grid that deforms around a mass sphere")]
    [SerializeField] private GridScript deformationGrid = null;

    [Tooltip("The mass spheres that deform the grid")]
    [SerializeField] private Rigidbody[] massSpheres = null;

    [Header("Properties")]
    [Tooltip("Material for the static grid")]
    [SerializeField] private Material gridMaterial = null;

    [Tooltip("The intended mass of the mass sphere")]
    [SerializeField] private float mass = 0.25f;

    [Tooltip("The position where the mass starts to spawn in, ending in the center of the deformation grid")]
    [SerializeField] private Vector3 massSpawnOffset = Vector3.zero;

    /// <summary>
    /// A faraway point for the grid cube to spawn
    /// </summary>
    private Vector3 gridCubeSpawnPoint = 10000.0f * Vector3.one;

    /// <summary>
    /// The main camera to serve as a reference to player point of vision
    /// </summary>
    private Camera playerCamera = null;

    private void Start()
    {
        if (dynamicAxis != null && gridMaterial != null && staticGrid != null && deformationGrid != null
            && massSpheres != null && massSpheres.Length > 0)
        {
            StartCoroutine(AxisToGridTransition());
        }
    }

    private void Update()
    {
        if (playerCamera != null)
        {
            Shader.SetGlobalVector("Grid_Player_Position", playerCamera.transform.position);
        }
        else
        {
            playerCamera = Camera.current;
        }
    }

    private IEnumerator AxisToGridTransition()
    {
        Vector3 originPosition = (deformationGrid.gridSize - Vector3.one) / 2.0f;
        Shader.SetGlobalVector("Grid_OriginPosition", new Vector4(originPosition.x, originPosition.y, originPosition.z, 0.0f));
        dynamicAxis.transform.position = originPosition;

        Shader.SetGlobalFloat("Grid_RevealRadius", 0.0f);
        Shader.SetGlobalFloat("Grid_OpaqueRadius", 2.0f);
        Shader.SetGlobalFloat("Grid_ExponentialConstant", 0.2f);
        Bounds staticGridBounds = staticGrid.GetComponent<MeshRenderer>().bounds;
        staticGrid.transform.position = originPosition + (new Vector3(0.5f, -0.5f, 0.5f) * (int)staticGridBounds.size.x);
        staticGrid.SetActive(true);

        Shader.SetGlobalFloat("Grid_Player_HideRadius", 0.5f);
        Shader.SetGlobalFloat("Grid_Player_FadeRadius", 1.0f);

        for (int i = 0; i < massSpheres.Length; ++i)
        {
            Rigidbody massSphere = massSpheres[i];
            massSphere.gameObject.SetActive(true);
            deformationGrid.transform.position = gridCubeSpawnPoint;
            deformationGrid.GetComponent<MeshRenderer>().material = gridMaterial;
            deformationGrid.gameObject.SetActive(true);
            Color originalMassColor = massSphere.GetComponent<MeshRenderer>().material.color;
            massSphere.GetComponent<MeshRenderer>().material.color = new Color(originalMassColor.r, originalMassColor.g, originalMassColor.b, 0.0f);
            massSphere.transform.position = originPosition + (massSpawnOffset * Mathf.Pow(-1, i));
            massSphere.gameObject.SetActive(false);
        }
        
        yield return dynamicAxis.ExtendAxes(1.0f, 16.0f, 2.0f);

        StartCoroutine(dynamicAxis.TransitionAxisColor(Color.white, 3.0f));
        yield return dynamicAxis.TransitionAxisThickness(0.02f, 3.0f);
        
        yield return RevealGrid(16.0f, 8.0f);
        dynamicAxis.SetAxisMaterial(gridMaterial);
        yield return ShrinkGrid(1.8f, 3.0f, 2.0f);
        dynamicAxis.gameObject.SetActive(false);
        staticGrid.SetActive(false);
        foreach (Rigidbody massSphere in massSpheres)
        {
            massSphere.gameObject.SetActive(true);
        }
        deformationGrid.transform.position = Vector3.zero;

        this.transform.position = originPosition;
        this.transform.LookAt(massSpheres[0].transform.position);

        yield return new WaitForSeconds(1.0f);

        foreach (Rigidbody massSphere in massSpheres)
        {
            // Reveal mass and start orbit
            Color massColor = massSphere.GetComponent<MeshRenderer>().material.color;
            massSphere.mass = mass;
            massSphere.GetComponent<MeshRenderer>().material.color = new Color(massColor.r, massColor.g, massColor.b, 1.0f);
            StartCoroutine(AnimateSphereCycle(massSphere, originPosition, 20.0f));
        }
    }

    private IEnumerator RevealGrid(float maxRadius, float duration)
    {
        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            yield return null;
            float t = timeElapsed / duration;
            float lerpPoint = Mathf.Lerp(0.0f, maxRadius, t);
            Shader.SetGlobalFloat("Grid_RevealRadius", lerpPoint);
            timeElapsed += Time.deltaTime;
        }
        Shader.SetGlobalFloat("Grid_RevealRadius", maxRadius); //snaps to final value after last loop
        yield break;
    }

    private IEnumerator ShrinkGrid(float endOpaqueRadius, float endExponentialConstant, float duration)
    {
        float timeElapsed = 0;

        float startOpaqueRadius = Shader.GetGlobalFloat("Grid_OpaqueRadius");
        float startExponentialConstant = Shader.GetGlobalFloat("Grid_ExponentialConstant");

        while (timeElapsed < duration)
        {
            yield return null;
            float t = timeElapsed / duration;

            float opaqueRadiusLerp = Mathf.Lerp(startOpaqueRadius, endOpaqueRadius, t);
            Shader.SetGlobalFloat("Grid_OpaqueRadius", opaqueRadiusLerp);

            float exponentialConstantRadius = Mathf.Lerp(startExponentialConstant, endExponentialConstant, t);
            Shader.SetGlobalFloat("Grid_ExponentialConstant", exponentialConstantRadius);

            timeElapsed += Time.deltaTime;
        }

        Shader.SetGlobalFloat("Grid_OpaqueRadius", endOpaqueRadius);
        Shader.SetGlobalFloat("Grid_ExponentialConstant", endExponentialConstant);

        yield break;
    }

    private IEnumerator AnimateSphereCycle(Rigidbody massSphere, Vector3 originPosition, float speed)
    {
        while (true)
        {
            massSphere.transform.RotateAround(originPosition, this.transform.up, speed * Time.deltaTime);
            yield return null;
        }
    }
}
