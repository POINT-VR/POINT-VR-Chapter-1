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
    /// The maximum distance a player can teleport 
    /// </summary>
    [Header("Constants")]
    [SerializeField] float teleportationDistance;
    /// <summary>
    /// The increment by which a player can move a UI slider
    /// </summary>
    [SerializeField] float sliderIncrement;
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
    private bool pushing, holdingSlider, pulling, gravEnabled, teleportMode;
    private Transform previousParentTransform, grabbingTransform;
    private Color laserColor;
    private Collider lastColliderHit;
    private Vector3 grabbingTransformVelocity, grabbingTransformPositionPrev;
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
    }
    private void Update()
    {
        if (pulling && grabbingTransform != null && (transform.position - grabbingTransform.position).sqrMagnitude > squaredMinPullDistance) // object being pulled: pull
        {
            grabbingTransform.position += pullSpeed * (transform.position - grabbingTransform.position).normalized;
        }
        else if (pushing && grabbingTransform != null && (transform.position - grabbingTransform.position).sqrMagnitude < squaredMaxPushDistance) // object being pushed: push
        {
            grabbingTransform.position -= pullSpeed * (transform.position - grabbingTransform.position).normalized;
        }
        if (grabbingTransform != null) // Holding an object
        {
            // Stores the velocity of the grabbing transform while its grabbed
            grabbingTransformVelocity = grabbingTransform.position - grabbingTransformPositionPrev;
            grabbingTransformPositionPrev = grabbingTransform.position;
        }
        // Fires a raycast that places the reticle
        Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, teleportationDistance, floorMask);
        if (teleportMode && hit.point != Vector3.zero) //In teleport mode and raycast found the floor: place reticle
        { 
            reticle.SetActive(true);
            reticle.transform.position = hit.point;
            reticle.transform.LookAt(new Vector3(playerTransform.position.x, 0f, playerTransform.position.z));
        }
        else //Not in teleport mode or raycast was not able to find the floor: hides the reticle
        {
            reticle.SetActive(false);
        }
        //Searches for UI
        if (Physics.Raycast(transform.position, transform.forward, out hit, 10f, UIMask)) //UI found: turn this green
        {
            if (hit.collider != lastColliderHit) //did not hit the same collider as in the previous frame: haptic feedback
            {
                hardwareController.VibrateHand();
            }
            laser.material.color = Color.green;
            lastColliderHit = hit.collider;
            if (holdingSlider)
            {
                CheckSlider(hit);
            }
        }
        else // UI not found: return the laser to its normal color
        {
            laser.material.color = laserColor;
            lastColliderHit = null;
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
        grabbingTransform.SetParent(previousParentTransform);
        GravityScript grav = grabbingTransform.GetComponent<GravityScript>();
        if (grav != null)
        {
            grav.enabled = gravEnabled;
        }
        // Add velocity to grabbed object.
        grabbingTransform.GetComponent<Rigidbody>().velocity = 5*grabbingTransformVelocity;

        grabbingTransform = null;

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
        grabbingTransform.SetParent(transform);
        grabbingTransform.GetComponent<Rigidbody>().velocity = Vector3.zero; // Also set the grabbed object's velocity to zero
        GravityScript grav = grabbingTransform.GetComponent<GravityScript>();
        if (grav != null)
        {
            gravEnabled = grav.enabled;
            grav.enabled = false;
        }
    }
    /// <summary>
    /// Runs when the user starts holding down the trigger.
    /// </summary>
    /// <param name="ctx"></param>
    private void Select(InputAction.CallbackContext ctx)
    { 
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 10f, UIMask)) //UI was detected: interact with it
        {
            GetComponent<AudioSource>().PlayScheduled(0);
            UICollider activeUICollider = hit.collider.gameObject.GetComponent<UICollider>();
            if (activeUICollider != null) //Collider is a UICollider: invokes assigned event
            {
                activeUICollider.OnCast.Invoke();
            }
            CheckSlider(hit);
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
            playerTransform.position = hit.point + 0.1f * Vector3.up;
        }
        teleportMode = false;
    }
    private void StopPushing(InputAction.CallbackContext ctx)
    {
        pushing = false;
    }
    private void StartPushing(InputAction.CallbackContext ctx)
    {
        pushing = true;
        pulling = false;
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
    private void StopPulling(InputAction.CallbackContext obj)
    {
        pulling = false;
    }

    private void StartPulling(InputAction.CallbackContext obj)
    {
        pulling = true;
        pushing = false;
    }
}
