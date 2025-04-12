using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DensityShellController : MonoBehaviour
{
    Animator anim;
    float current_angle;
    [SerializeField] bool cycleAngleDebug = true;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        current_angle = 0;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (cycleAngleDebug && timer % 4 < 0.01) {
            if (current_angle == 0) {
                SetAngle(45);
            } else if (current_angle == 45) {
                SetAngle(90);
            } else if (current_angle == 90) {
                SetAngle(180);
            } else if (current_angle == 180) {
                SetAngle(135);
            } else if (current_angle == 135) {
                SetAngle(0);
            }
        }
    }

    void OpenShellToAngle(float angle) {
        anim.SetFloat("TimeToPlay", angle/180);
        anim.Play("Open-Sphere");
        current_angle = angle;
    }

    void CloseShellToAngle(float angle) {
        anim.SetFloat("TimeToPlay", 1-(angle/180));
        anim.Play("Close-Sphere");
        current_angle = angle;
    }

    public void SetAngle(float angle) {
        Debug.Log("Set angle to " + angle);
        if (angle < current_angle) {
            CloseShellToAngle(angle);
        } else if (angle > current_angle) {
            OpenShellToAngle(angle);
        }
        current_angle = angle;
    }
}
