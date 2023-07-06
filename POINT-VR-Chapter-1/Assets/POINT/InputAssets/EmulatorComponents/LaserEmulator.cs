﻿using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;
/// <summary>
/// This script controls the actions of the laser on objects. As of 7/3/23 UI code is still incomplete - George Bayliss
/// </summary>
public class LaserEmulator : MonoBehaviour
{
    /// <summary>
    /// The laser pointer
    /// </summary>
    [SerializeField] MeshRenderer laser;
    /// <summary>
    /// The mask used to detect the floor
    /// </summary>
    [Header("Raycast Masks")]
    [SerializeField] LayerMask grabMask;
    /// <summary>
    /// The mask used to detect UI
    /// </summary>
    [SerializeField] LayerMask UIMask;
    /// <summary>
    /// The input reference used to interact with grabable objects and UI
    /// </summary>
    [Header("Input Action References")]
    [SerializeField] InputActionReference clickReference;
    /// <summary>
    /// The input reference used to scroll through the UI and to push/pull objects
    /// </summary>
    [SerializeField] InputActionReference scrollReference;
    /// <summary>
    /// Scrollwheel sensitivity; affects UI scrolling and push/pull rate
    /// </summary>
    [Header("Parameters")]
    [SerializeField] float scrollSensitivity;
    /// <summary>
    /// The square of the minimum distance maintained between a pulled object and the hand
    /// </summary>
    [SerializeField] float squaredMinPullDistance;
    private enum State
    {
        none,
        grabbing,
        pushing,
        pulling,
        UI,
        slider,
        scrollbar
    }
    /// <summary>
    /// The action currently being performed
    /// </summary>
    State state = State.none;
    /// <summary>
    /// Stores the grabbable object currently being held
    /// </summary>
    private Transform grabbingTransform;
    /// <summary>
    /// The gravity state of the transformed object; necessary in order to re-enable gravity to released objects
    /// </summary>
    private bool gravEnabled;
    /// <summary>
    /// The initial parent object of a grabbable object; necessary in order to reset released objects to their initial parent
    /// </summary>
    private Transform previousParentTransform;
    /// <summary>
    /// The velocity of the grabbable object as it is moved by the laser; necessary to give the object velocity after it is released
    /// </summary>
    private Vector3 grabbingTransformVelocity;
    /// <summary>
    /// The position of the grabbable object in the last frame; used to calculate the velocity
    /// </summary>
    private Vector3 grabbingTransformPositionPrev;
    /// <summary>
    /// The speed of pulling/pushing action
    /// </summary>
    private float pullSpeed;
    /// <summary>
    /// The default color of the laser; set in the inspector window
    /// </summary>
    private Color laserColor;
    /// <summary>
    /// The default length of the laser; set in the inspector window
    /// </summary>
    private float laserLength;
    /// <summary>
    /// Gives the position of the near of the laser relative the script transform 
    /// </summary>
    private Vector3 laserPosition;
    void Start()
    {
        laserPosition = laser.transform.localPosition;
        laserColor = laser.material.color;
        laserLength = laser.transform.localScale.y;
    }
    private void OnEnable()
    {
        clickReference.action.Enable();
        clickReference.action.started += Clicked;
        clickReference.action.canceled += Released;
        scrollReference.action.Enable();
        scrollReference.action.performed += Scroll;
        previousParentTransform = null;
    }
    private void OnDisable()
    {
        clickReference.action.Disable();
        clickReference.action.started -= Clicked;
        clickReference.action.canceled -= Released; 
        scrollReference.action.Disable();
        scrollReference.action.performed -= Scroll;
    }
    private void Update()
    {

        if (state == State.grabbing || state == State.pushing || state == State.pulling) // object being held
        {
            if (grabbingTransform == null) //grabbing transform lost -> makes sure to reset state
            {
                transform.GetComponent<Animator>().SetBool("isGrabbing", false);
                state = State.none;
            }
            // Prevents the GrabbedTransform from being pushed by other objects
            grabbingTransform.GetComponent<Rigidbody>().velocity = Vector3.zero;

            // Stores the velocity of the grabbing transform while its grabbed
            grabbingTransformVelocity = grabbingTransform.position - grabbingTransformPositionPrev;
            grabbingTransformPositionPrev = grabbingTransform.position;

            laser.material.color = Color.red;

            //fail safe to prevent object from getting where it shouldn't -> immediately drops the object
            if ((grabbingTransform.position - transform.position).sqrMagnitude < squaredMinPullDistance || (grabbingTransform.position - transform.position).sqrMagnitude > laserLength*laserLength)
            {
                grabbingTransform.SetParent(previousParentTransform);
                GravityScript grav = grabbingTransform.GetComponent<GravityScript>();
                if (grav != null)
                {
                    grav.enabled = gravEnabled;
                }
                // Add velocity to grabbed object.
                grabbingTransform.GetComponent<Rigidbody>().velocity = 2.5f * (grabbingTransformVelocity);
                grabbingTransform = null;
                transform.GetComponent<Animator>().SetBool("isGrabbing", false);
                state = State.none;
            }
        } 
        else if (state == State.UI || state == State.slider || state == State.scrollbar) // interacting with UI
        {
            laser.material.color = laserColor;
            Physics.Raycast(transform.position, transform.forward, out RaycastHit UIHit, 10f, UIMask);
            if (state == State.slider)
            {
                CheckSlider(UIHit);
            }
            if (state == State.scrollbar)
            {
                CheckScrollbar(UIHit);
            }
        } 
        else if (state == State.none)
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, laserLength, grabMask))
            {
                laser.material.color = Color.magenta;
                float distance = (hit.point - transform.position).magnitude;
                laser.transform.localScale = new Vector3(laser.transform.localScale.x, distance, laser.transform.localScale.z);
                laser.transform.localPosition = laserPosition + new Vector3(0, 0, 1) * (distance - laserLength);
            } 
            else if (Physics.Raycast(transform.position, transform.forward, out hit, laserLength, UIMask)) 
            {
                laser.material.color = Color.green;
                float distance = (hit.point - transform.position).magnitude;
                laser.transform.localScale = new Vector3(laser.transform.localScale.x, distance, laser.transform.localScale.z);
                laser.transform.localPosition = laserPosition + new Vector3(0, 0, 1) * (distance - laserLength);
            } else
            {
                laser.material.color = laserColor;
                laser.transform.localScale = new Vector3(laser.transform.localScale.x, laserLength, laser.transform.localScale.z);
                laser.transform.localPosition = laserPosition;
            }
        }

        if (state == State.pulling) // object in pull state -> pull
        {
            Vector3 newPos = grabbingTransform.position + pullSpeed * (transform.position - grabbingTransform.position).normalized;
            if ((newPos - transform.position).sqrMagnitude > squaredMinPullDistance)
            {
                grabbingTransform.position = newPos;
                float distance = (grabbingTransform.position - transform.position).magnitude;
                laser.transform.localScale = new Vector3(laser.transform.localScale.x, distance, laser.transform.localScale.z);
                laser.transform.localPosition = laserPosition + new Vector3(0, 0, 1) * (distance - laserLength);
            }
        }
        else if (state == State.pushing) // object in push state -> push
        {
            Vector3 newPos = grabbingTransform.position + pullSpeed * (transform.position - grabbingTransform.position).normalized;
            if ((newPos - transform.position).sqrMagnitude < laserLength * laserLength)
            {
                grabbingTransform.position = newPos;
                float distance = (grabbingTransform.position - transform.position).magnitude;
                laser.transform.localScale = new Vector3(laser.transform.localScale.x, distance, laser.transform.localScale.z);
                laser.transform.localPosition = laserPosition + new Vector3(0, 0, 1) * (distance - laserLength);
            }
        }
    }
    /// <summary>
    /// This makes the hand "drop" whatever it was holding.
    /// </summary>
    public void Release()
    {
        if (state == State.UI || state == State.slider || state == State.scrollbar)
        {
            state = State.none;
        }
    }
    private void Released(InputAction.CallbackContext ctx)
    {
        Release();
    }
    /// <summary>
    /// When hovering a grabbable click toggles the grabbed and default states. When hovering a UI object, click interacts with it.
    /// </summary>
    private void Clicked(InputAction.CallbackContext ctx)
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, laserLength, grabMask)) //Grabbable object detected
        {
            if (state == State.none) //Object is not being held -> grabs object
            {
                state = State.grabbing;
                grabbingTransform = hit.transform;

                //centers object on the beam of the laser
                float distance = (grabbingTransform.position - transform.position).magnitude;
                grabbingTransform.position = transform.position + transform.forward * distance;

                previousParentTransform = grabbingTransform.parent;
                grabbingTransform.SetParent(transform);
                grabbingTransform.GetComponent<Rigidbody>().velocity = Vector3.zero; // Also set the grabbed object's velocity to zero
                grabbingTransformVelocity = Vector3.zero;
                grabbingTransformPositionPrev = hit.transform.position;
                GravityScript grav = grabbingTransform.GetComponent<GravityScript>();
                if (grav != null)
                {
                    gravEnabled = grav.enabled;
                    grav.enabled = false;
                }
                transform.GetComponent<Animator>().SetBool("isGrabbing", true);
            }
            else if (state == State.grabbing || state == State.pushing || state == State.pulling) //Object is being held -> drops it
            {
                state = State.none;
                grabbingTransform.SetParent(previousParentTransform);
                GravityScript grav = grabbingTransform.GetComponent<GravityScript>();
                if (grav != null)
                {
                    grav.enabled = gravEnabled;
                }
                // Add velocity to grabbed object.
                grabbingTransform.GetComponent<Rigidbody>().velocity = 2.5f * (grabbingTransformVelocity); 
                grabbingTransform = null;
                transform.GetComponent<Animator>().SetBool("isGrabbing", false);
            }
        } else if (Physics.Raycast(transform.position, transform.forward, out hit, laserLength, UIMask)) //UI detected -> interacts with it
        {
            GetComponent<AudioSource>().PlayScheduled(0);
            UICollider activeUICollider = hit.collider.gameObject.GetComponent<UICollider>();
            if (activeUICollider != null) //Collider is a UICollider: invokes assigned event
            {
                activeUICollider.OnCast.Invoke();
            }
            CheckSlider(hit);
            CheckScrollbar(hit);
        }
    }
    /// <summary>
    /// Checks if the raycast hit was against a slider and acts on the slider if so
    /// </summary>
    /// <param name="hit"></param>
    private void CheckSlider(RaycastHit hit)
    {
        Slider activeSlider = hit.collider.gameObject.GetComponent<Slider>();
        if (activeSlider != null) //Collider is a slider: proceeds to interact with slider. 
        {
            RectTransform sliderRect = activeSlider.transform as RectTransform;
            if (sliderRect != null)
            {
                float sliderWidth = sliderRect.rect.width;
                // If the width is more than 0, set slider value to be the ratio of the raycast x-coordinate over the width of the slider (clamped to between 0.0f and 1.0f)
                // Otherwise, set value to 0.0f (to avoid division by 0)
                activeSlider.value = (sliderWidth > 0) ? (Mathf.Clamp(((activeSlider.transform.InverseTransformPoint(hit.point).x + sliderRect.anchoredPosition.x) / sliderWidth), 0.0f, 1.0f)) : 0.0f;
            }
            state = State.slider;
        }
        else //Raycast did not find a slider: hand is not holding a slider
        {
            state = State.UI;
        }
    }
    /// <summary>
    /// Checks if the raycast hit was against a scrollbar and acts on the scrollbar if so
    /// </summary>
    /// <param name="hit"></param>
    private void CheckScrollbar(RaycastHit hit)
    {
        Scrollbar activeScrollbar = hit.collider.gameObject.GetComponent<Scrollbar>();
        if (activeScrollbar != null) //Collider is a scrollbar: proceeds to interact with scrollbar. 
        {
            RectTransform scrollbarRect = activeScrollbar.transform as RectTransform;
            if (scrollbarRect != null)
            {
                float scrollbarHeight = scrollbarRect.rect.height;
                // Similar calculation to slider, but with y-coordinates instead of x
                activeScrollbar.value = (scrollbarHeight > 0) ? (Mathf.Clamp(((activeScrollbar.transform.InverseTransformPoint(hit.point).y + scrollbarRect.anchoredPosition.y) / scrollbarHeight), 0.0f, 1.0f)) : 0.0f;
            }
            state = State.scrollbar;
        }
        else //Raycast did not find a scrollbar: this hand is not holding a scrollbar
        {
            state = State.UI;
        }
    }

    private void Pull()
    {
        state = State.pulling;
        transform.GetComponent<Animator>().SetFloat("pushPull", -1.0f);
    }
    private void Push()
    {
        state = State.pushing;
        transform.GetComponent<Animator>().SetFloat("pushPull", 1.0f);
    }
    private void StopPushPull()
    {
        transform.GetComponent<Animator>().SetFloat("pushPull", 0.0f);
    }
    /// <summary>
    /// Scroll pushes/pulls grabbable and interacts with UI objects.
    /// </summary>
    private void Scroll(InputAction.CallbackContext obj)
    {
        float scrollValue = obj.action.ReadValue<Vector2>().y * scrollSensitivity;
        if (scrollValue != 0)
        {
            if (state == State.UI || state == State.slider || state == State.scrollbar) //UI detected -> scroll through menu
            {
                Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 10f, UIMask);
                ScrollRect scrollRect = hit.collider.gameObject.GetComponentInParent<ScrollRect>();
                Debug.Log(hit.collider.gameObject);
                if (scrollRect != null) // collider is scrollable
                {
                    Scrollbar scrollbarVertical = scrollRect.verticalScrollbar;
                    if (scrollbarVertical != null)
                    {
                        scrollbarVertical.value = Mathf.Clamp(scrollbarVertical.value + scrollValue, 0.0f, 1.0f);
                    }
                }
            } else if (state == State.grabbing || state == State.pushing || state == State.pulling) //Holding an object -> scroll to push/pull
            {
                pullSpeed = scrollValue;
                if (scrollValue*scrollValue >= 4*squaredMinPullDistance) //avoids error where object is being pulled fast enough to get behind the player
                {
                    scrollValue = 0;
                    return;
                }
                if (scrollValue < 0)
                {
                    Push();
                } 
                else
                {
                    Pull();
                }
            }
        } else //Scroll value 0 -> stops push/pull
        {
            StopPushPull(); 
        }   
    }
}
