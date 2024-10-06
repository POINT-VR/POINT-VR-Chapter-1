using UnityEngine;

public class DirectionalArrow : MonoBehaviour
{
    private const float DELTA_ANGLE = 90.0f;

    [Tooltip("The angle (in degrees) of this arrow in the intended solution")]
    [SerializeField] private int correctAngle;

    [Tooltip("The color for the arrow excluding outlines when it is at the correct angle")]
    [SerializeField] private Color correctColor;

    [Tooltip("The color for the arrow excluding outlines when it is not at the correct angle")]
    [SerializeField] private Color incorrectColor;

    private bool isCorrect = false;
    public bool IsCorrect
    {
        get
        {
            return isCorrect;
        }
    }

    private void Start()
    {
        if (this.TryGetComponent(out MeshRenderer renderer)) {
            if (this.transform.eulerAngles.z == correctAngle)
            {
                renderer.materials[0].color = correctColor;
                isCorrect = true;
            } else {
                renderer.materials[0].color = incorrectColor;
                isCorrect = false;
            }
        }
    }

    public void Rotate()
    {
        this.transform.eulerAngles += Vector3.forward * DELTA_ANGLE;

        if (this.TryGetComponent(out MeshRenderer renderer))
        {
            if ((int)this.transform.eulerAngles.z % 360 == correctAngle)
            {
                renderer.materials[0].color = correctColor;
                isCorrect = true;
            }
            else
            {
                renderer.materials[0].color = incorrectColor;
                isCorrect = false;
            }
        }


    }
}
