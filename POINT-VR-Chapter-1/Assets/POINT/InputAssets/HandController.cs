using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
/// <summary>
/// This simulates the player's hands in-game.
/// </summary>
public class HandController : MonoBehaviour
{
    /// <summary>
    /// The GameObject that is the parent to all hardware devices
    /// </summary>
    [SerializeField] Transform playerTransform;
    /// <summary>
    /// The GameObject that appears on the floor when in teleport mode
    /// </summary>
    [SerializeField] GameObject reticle;
    /// <summary>
    /// The laser pointer
    /// </summary>
    [SerializeField] MeshRenderer laser;
    /// <summary>
    /// The corresponding script representing the hardware
    /// </summary>
    [SerializeField] XRHardwareController hardwareController;
    /// <summary>
    /// The other instance
    /// </summary>
    [SerializeField] HandController otherHand;
    /// <summary>
    /// The audio that plays when this hand teleports
    /// </summary>
    [SerializeField] AudioClip teleportAudio;
    /// <summary>
    /// The mask used to detect the floor
    /// </summary>
    [Header("Raycast Masks")]
    [SerializeField] LayerMask floorMask;
    /// <summary>
    /// The mask used to detect objects which can be grabbed
    /// </summary>
    [SerializeField] LayerMask grabMask;
    /// <summary>
    /// The mask used to detect UI
    /// </summary>
    [SerializeField] LayerMask UIMask;
    /// <summary>
    /// The input reference used to interact with UI and teleport
    /// </summary>
    [Header("Input Action References")]
    [SerializeField] InputActionReference selectReference;
    /// <summary>
    /// The input reference used to interact with grabable objects
    /// </summary>
    [SerializeField] InputActionReference grabReference;
    /// <summary>
    /// The input reference used to enable/disable teleport mode
    /// </summary>
    [SerializeField] InputActionReference pushingReference;
    /// <summary>
    /// The input reference used to pull an object closer to the hand
    /// </summary>
    [SerializeField] InputActionReference pullingReference;
    /// <summary>
    /// The input reference used to scroll through the UI
    /// </summary>
    [SerializeField] InputActionReference scrollReference;
    /// <summary>
    /// The maximum distance a player can teleport 
    /// </summary>
    [Header("Constants")]
    [SerializeField] float teleportationDistance;
    /// <summary>
    /// The increment by which UI menu scroll changes on up/down joystick movement
    /// </summary>
    [SerializeField] float scrollIncrement;
    /// <summary>
    /// The maximum distance at which a player can grab objects
    /// </summary>
    [SerializeField] float grabDistance;
    /// <summary>
    /// The speed at which objects are pulled towards the player
    /// </summary>
    [SerializeField] float pullSpeed;
    /// <summary>
    /// The square of the minimum distance maintained between a pulled object and the hand
    /// </summary>
    [SerializeField] float squaredMinPullDistance;
    /// <summary>
    /// The square of the maximum distance maintained between a pushed object and the hand
    /// </summary>
    [SerializeField] float squaredMaxPushDistance;
    private bool pushing, holdingSlider, holdingScrollbar, pulling, gravEnabled, teleportMode;
    private Transform previousParentTransform, grabbingTransform, lastGrabHit;
    private Color laserColor;
    private Collider lastColliderHit;
    private Vector3 grabbingTransformVelocity, grabbingTransformPositionPrev, velocityPrev;
    private void OnEnable()
    {
        selectReference.action.Enable();
        grabReference.action.Enable();
        pushingReference.action.Enable();
        selectReference.action.started += Select;
        selectReference.action.canceled += Unselect;
        grabReference.action.started += Grab;
        grabReference.action.canceled += Released;
        pushingReference.action.started += StartPushing;
        pushingReference.action.canceled += StopPushing;
        pullingReference.action.Enable();
        pullingReference.action.started += StartPulling;
        pullingReference.action.canceled += StopPulling;
        scrollReference.action.Enable();
        scrollReference.action.performed += Scroll;
        pushing = false;
        previousParentTransform = null;
        laserColor = laser.material.color;
    }
    private void OnDisable()
    {
        selectReference.action.Disable();
        grabReference.action.Disable();
        pushingReference.action.Disable();
        selectReference.action.started -= Select;
        selectReference.action.canceled -= Unselect;
        grabReference.action.started -= Grab;
        grabReference.action.canceled -= Released; 
        pushingReference.action.started -= StartPushing;
        pushingReference.action.canceled -= StopPushing;
        pullingReference.action.Disable();
        pullingReference.action.started -= StartPulling;
        pullingReference.action.canceled -= StopPulling;
        scrollReference.action.Disable();
        scrollReference.action.performed -= Scroll;
    }
    private void Update()
    {
        if (pulling && grabbingTransform != null && (transform.position - grabbingTransform.position).sqrMagnitude > squaredMinPullDistance) // object being pulled: pull
        {
            grabbingTransform.position -= pullSpeed * transform.forward;
        }
        else if (pushing && grabbingTransform != null && (transform.position - grabbingTransform.position).sqrMagnitude < squaredMaxPushDistance) // object being pushed: push
        {
            grabbingTransform.position += pullSpeed * transform.forward;
        }
        if (grabbingTransform != null) // Holding an object
        {
            // Prevents the GrabbedTransform from being pushed by other objects
            grabbingTransform.GetComponent<Rigidbody>().velocity = Vector3.zero; 

            // Stores the velocity of the grabbing transform while its grabbed
            velocityPrev = grabbingTransformVelocity;
            grabbingTransformVelocity = grabbingTransform.position - grabbingTransformPositionPrev;
            grabbingTransformPositionPrev = grabbingTransform.position;
        }
        // Fires a raycast that places the reticle
        Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, teleportationDistance, floorMask);
        if (teleportMode && hit.point != Vector3.zero) //In teleport mode and raycast found the floor: place reticle
        { 
            reticle.SetActive(true);
            reticle.transform.position = hit.point;
            reticle.transform.LookAt(new Vector3(playerTransform.parent.position.x, 0f, playerTransform.parent.position.z));
            transform.GetComponent<Animator>().SetBool("isPointing", true);
        }
        else //Not in teleport mode or raycast was not able to find the floor: hides the reticle
        {
            reticle.SetActive(false);
            transform.GetComponent<Animator>().SetBool("isPointing", false);
        }
        //Searches for UI or grabbable
        if (Physics.Raycast(transform.position, transform.forward, out hit, 10f, UIMask)) //UI found: turn this green
        {
            if (hit.collider != lastColliderHit) //did not hit the same collider as in the previous frame: haptic feedback
            {
                hardwareController.VibrateHand();
            }
            if (hit.collider.GetComponent<ScrollRect>() == null)
            {
                // Check if detected item is within a ScrollRect; if so, make sure ScrollRect is also hit
                // This is a workaround for colliders not disappearing even when a UI element is hidden by a mask, as in a ScrollRect
                ScrollRect scrollRect = hit.collider.GetComponentInParent<ScrollRect>();
                if (scrollRect != null)
                {
                    RaycastHit[] raycastHits = Physics.RaycastAll(transform.position, transform.forward, 10f, UIMask);
                    bool containsScrollRect = false;
                    foreach (RaycastHit raycastHit in raycastHits)
                    {
                        if (raycastHit.collider.GetComponent<ScrollRect>() == scrollRect)
                        {
                            containsScrollRect = true;
                            break;
                        }
                    }
                    if (containsScrollRect)
                    {
                        laser.material.color = Color.green;
                        transform.GetComponent<Animator>().SetBool("isPointing", true);
                    }
                } else
                {
                    laser.material.color = Color.green;
                    transform.GetComponent<Animator>().SetBool("isPointing", true);
                }
            }
            else
            {
                laser.material.color = laserColor;
                transform.GetComponent<Animator>().SetBool("isPointing", false);
            }
            lastColliderHit = hit.collider;
            if (holdingSlider)
            {
                CheckSlider(hit);
            }
            if (holdingScrollbar)
            {
                CheckScrollbar(hit);
            }
        }
        else if (grabbingTransform != null || Physics.Raycast(transform.position, transform.forward, out hit, grabDistance, grabMask)) //grabbable found or is holding something: turn this cyan
        {
            if (hit.transform != lastGrabHit && grabbingTransform == null) //did not hit the same collider as in the previous frame: haptic feedback
            {
                hardwareController.VibrateHand();
            }
            laser.material.color = Color.magenta;
            if (grabbingTransform != null)
            {
                lastGrabHit = grabbingTransform;
            }
            else
            {
                lastGrabHit = hit.transform;
            }
        }
        else // UI or grabbable not found: return the laser to its normal color
        {
            laser.material.color = laserColor;
            lastColliderHit = null;
            lastGrabHit = null;
        }
    }
    /// <summary>
    /// This makes the hand "drop" whatever it was holding.
    /// </summary>
    public void Release()
    {
        if (grabbingTransform == null) //Nothing was grabbed: nothing happens
        {
            return;
        }

        bool snapped = false;
        if (grabbingTransform.GetComponent<SnapObject>())
        {
            SnapObject snapObject = grabbingTransform.GetComponent<SnapObject>();
            if (previousParentTransform && previousParentTransform.GetComponent<SnapAnchor>()) previousParentTransform.GetComponent<SnapAnchor>().heldObject = null;

            if (snapObject.currentAnchor)
            {
                snapped = true;
                grabbingTransform.SetParent(snapObject.currentAnchor.transform);
                grabbingTransform.localPosition = snapObject.currentAnchor.GetComponent<BoxCollider>().center;
                snapObject.currentAnchor.GetComponent<SnapAnchor>().heldObject = snapObject;
                //snapObject.currentAnchor = null;

            }
            else
            {
                grabbingTransform.SetParent(null);
            }
        }
        else
        {
            grabbingTransform.SetParent(previousParentTransform);
        }

        GravityScript grav = grabbingTransform.GetComponent<GravityScript>();
        if (grav != null)
        {
            grav.enabled = gravEnabled;
        }
        // Add velocity to grabbed object.
        if (!snapped) grabbingTransform.GetComponent<Rigidbody>().velocity = 2.5f*hardwareController.Velocity; //2.5f*(grabbingTransformVelocity + velocityPrev);
        grabbingTransform = null;
        transform.GetComponent<Animator>().SetBool("isGrabbing", false);
    }
    private void Released(InputAction.CallbackContext ctx)
    {
        Release();
    }
    private void Grab(InputAction.CallbackContext ctx)
    {
        Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, grabDistance, grabMask);
        if (hit.point == Vector3.zero) //Raycast did not find something to grab: nothing happens
        {
            return;
        }
        grabbingTransform = hit.transform;
        if (grabbingTransform == otherHand.grabbingTransform) //Object being grabbed is currently held by the other hand: the other hand releases it
        {
            otherHand.Release();
        }
        previousParentTransform = grabbingTransform.parent;
        if (grabbingTransform && grabbingTransform.GetComponent<SnapObject>()
                && previousParentTransform && previousParentTransform.GetComponent<SnapAnchor>())
        {
            previousParentTransform.GetComponent<SnapAnchor>().heldObject = null;
        }
        grabbingTransform.SetParent(transform);
        grabbingTransform.GetComponent<Rigidbody>().velocity = Vector3.zero; // Also set the grabbed object's velocity to zero
        velocityPrev = Vector3.zero;
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
    /// <summary>
    /// Runs when the user starts holding down the trigger.
    /// </summary>
    /// <param name="ctx"></param>
    private void Select(InputAction.CallbackContext ctx)
    { 
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 10f, UIMask)) //UI was detected: interact with it
        {
            // Check if detected item is within a ScrollRect; if so, make sure ScrollRect is also hit
            // This is a workaround for colliders not disappearing even when a UI element is hidden by a mask, as in a ScrollRect
            ScrollRect scrollRect = hit.collider.GetComponentInParent<ScrollRect>();
            if (scrollRect != null)
            {
                RaycastHit[] raycastHits = Physics.RaycastAll(transform.position, transform.forward, 10f, UIMask);
                bool containsScrollRect = false;
                foreach (RaycastHit raycastHit in raycastHits)
                {
                    if (raycastHit.collider.GetComponent<ScrollRect>() == scrollRect)
                    {
                        containsScrollRect = true;
                        break;
                    }
                }
                if (!containsScrollRect) return;
            }
            GetComponent<AudioSource>().PlayScheduled(0);
            UICollider activeUICollider = hit.collider.gameObject.GetComponent<UICollider>();
            if (activeUICollider != null) //Collider is a UICollider: invokes assigned event
            {
                activeUICollider.OnCast.Invoke();
            }
            CheckSlider(hit);
            CheckScrollbar(hit);
        }
        else
        {
            teleportMode = true;
        }
    }
    /// <summary>
    /// Runs when the user releases the trigger.
    /// </summary>
    /// <param name="ctx"></param>
    private void Unselect(InputAction.CallbackContext ctx)
    {
        holdingSlider = false;
        if (teleportMode && Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, teleportationDistance, floorMask)) //Raycast detected the floor: teleport
        {
            GetComponent<AudioSource>().PlayOneShot(teleportAudio);
            playerTransform.parent.position = hit.point;
        }
        teleportMode = false;
        transform.GetComponent<Animator>().SetBool("isPointing", false);
    }
    private void StopPushing(InputAction.CallbackContext ctx)
    {
        pushing = false;
        transform.GetComponent<Animator>().SetFloat("pushPull", 0.0f);
    }
    private void StartPushing(InputAction.CallbackContext ctx)
    {
        pushing = true;
        pulling = false;
        transform.GetComponent<Animator>().SetFloat("pushPull", 1.0f);
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
            holdingSlider = true;
        }
        else //Raycast did not find a slider: this hand is not holding a slider
        {
            holdingSlider = false;
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
            holdingScrollbar = true;
        }
        else //Raycast did not find a scrollbar: this hand is not holding a scrollbar
        {
            holdingScrollbar = false;
        }
    }
    private void StopPulling(InputAction.CallbackContext obj)
    {
        pulling = false;
        transform.GetComponent<Animator>().SetFloat("pushPull", 0.0f);
    }

    private void StartPulling(InputAction.CallbackContext obj)
    {
        pulling = true;
        pushing = false;
        transform.GetComponent<Animator>().SetFloat("pushPull", -1.0f);
    }

    private void Scroll(InputAction.CallbackContext obj)
    {
        if (obj.action.ReadValue<Vector2>().y != 0 && Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 10f, UIMask)) //UI was detected
        {
            ScrollRect scrollRect = hit.collider.gameObject.GetComponentInParent<ScrollRect>();
            if (scrollRect != null) // collider is scrollable
            {
                Scrollbar scrollbarVertical = scrollRect.verticalScrollbar;
                if (scrollbarVertical != null)
                {
                    float valueChange = obj.action.ReadValue<Vector2>().y > 0 ? scrollIncrement : -scrollIncrement;
                    scrollbarVertical.value = Mathf.Clamp(scrollbarVertical.value + valueChange, 0.0f, 1.0f);
                }
            }
        }
    }
}
