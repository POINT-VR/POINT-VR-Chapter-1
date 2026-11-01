using UnityEngine;

public class EndpointGuideArrow : MonoBehaviour
{
    [SerializeField] private float arrowPosOffset = 0.3f;

    private Vector3 currentEndpointPos;
    public Vector3 CurrentEndpointPos
    {
        set
        {
            currentEndpointPos = value;
            shouldAdjustArrowTransform = true;
        }
    }

    private GameObject cameraContainer = null;
    private Vector3 lastCameraPos = Vector3.zero;
    private bool shouldAdjustArrowTransform = true;

    private void Update()
    {
        if (cameraContainer == null && Camera.current != null) // Player has spawned
        {
            cameraContainer = Camera.current.transform.parent.gameObject;
        }
        if (cameraContainer != null)
        {
            Vector3 cameraPos = cameraContainer.transform.position;
            if (cameraPos != lastCameraPos)
            {
                shouldAdjustArrowTransform = true;
            }

            if (shouldAdjustArrowTransform)
            {
                Vector3 back = new Vector3(currentEndpointPos.x - cameraPos.x, 0.0f, currentEndpointPos.z - cameraPos.z).normalized;
                Vector3 right = Vector3.Cross(Vector3.up, back).normalized;

                Vector3 refPos = currentEndpointPos + arrowPosOffset * (right + Vector3.up);
                Debug.Log(refPos);
                this.transform.position = refPos;
                this.transform.LookAt(cameraContainer.transform, Vector3.Cross(cameraPos - refPos, currentEndpointPos - refPos));

                shouldAdjustArrowTransform = false;
            }

            lastCameraPos = cameraPos;
        }
    }
}
