using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DensityAnimation : MonoBehaviour
{
    private Animator _animator;

    void Start()
    {
        // Get the Animator component attached to this GameObject
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Command: Play animation when the Space bar is pressed
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            _animator.SetTrigger("OpenTrigger");
        }

        if (Keyboard.current.jKey.wasPressedThisFrame)
        {
            _animator.SetTrigger("CloseTrigger");
        }
    }
}
