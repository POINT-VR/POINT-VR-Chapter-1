using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DensityShellController : MonoBehaviour
{
    // This script is not complete. Purpose: to provide an easy wrapper for changing the angle of the shell.
    // Ideally, you would call SetAngle(angle) and have the shell animate opening/closing to that angle.
    // However, getting them to play sequentially and wait for previous animations to finish has been a challenge.

    Animator anim;
    float current_angle;
    Queue<float> anim_q;
    bool isAnimating;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        current_angle = 0;
        anim_q = new Queue<float>();
        anim_q.Clear();
        isAnimating = false;

        // debug
        // SetAngle(45);
        // SetAngle(180);
        // SetAngle(90);
        // SetAngle(135);
    }

    public void SetAngle(float angle) {
        Debug.Log("Set angle to " + angle);

        // Add the angle to the queue
        anim_q.Enqueue(angle);

        if (!isAnimating)
        {
            isAnimating = true;
            angle = anim_q.Peek();
            // Debug.Log("angle: " + angle + "\ncurrent angle: " + current_angle);
            if (angle < current_angle)
            {
                current_angle = angle;
                StartCoroutine(PlayFullAnim(1 - (angle / 180), "Close-Sphere"));
            }
            else if (angle > current_angle)
            {
                current_angle = angle;
                StartCoroutine(PlayFullAnim(angle / 180, "Open-Sphere"));
            }
            // Debug.Log("current angle: " + current_angle);
        }
        else
        {
            print("Animation in progress");
        }
    }

    IEnumerator PlayFullAnim(float time, string name) {
        Debug.Log("Starting animation " + name);
        print("Anim time: " + time);

        // Play the animation
        anim.SetFloat("TimeToPlay", time);
        anim.Play(name);

        // Wait for the full length of the animation to complete
        Debug.Log("Waiting for " + anim.GetCurrentAnimatorStateInfo(0).length + " seconds");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        Debug.Log("Done waiting for " + name);

        anim_q.Dequeue();

        // if there is anything else on the queue, pop the top and play it
        while (anim_q.Count != 0) {
            print("There are " + anim_q.Count + " angles on the queue");
            float angle = anim_q.Peek();
            Debug.Log("AFTER PLAY: angle: " + angle + "\ncurrent angle: " + current_angle);
            if (angle < current_angle)
            {
                current_angle = angle;
                StartCoroutine(PlayFullAnim(1 - (angle / 180), "Close-Sphere"));
                break;
            }
            else if (angle > current_angle)
            {
                current_angle = angle;
                StartCoroutine(PlayFullAnim(angle / 180, "Open-Sphere"));
                break;
            }
            else
            {
                anim_q.Dequeue();
            }
        }

        if (anim_q.Count == 0)
        {
            Debug.Log("Finished animating");
            isAnimating = false;
        }
    }
}
