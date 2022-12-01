using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    [SerializeField] InputActionReference teleportModeReference;
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
    private bool teleportMode, holdingSlider, pulling, gravEnabled;
    private Transform previousParentTransform, grabbingTransform;
    private Color laserColor;
    private Collider lastColliderHit;
    private void OnEnable()
    {
        selectReference.action.Enable();
        grabReference.action.Enable();
        teleportModeReference.action.Enable();
        selectReference.action.started += Select;
        selectReference.action.canceled += Unselect;
        grabReference.action.started += Grab;
        grabReference.action.canceled += Released;
        teleportModeReference.action.started += TeleportModeActivate;
        teleportModeReference.action.canceled += TeleportModeCancel;
        pullingReference.action.Enable();
        pullingReference.action.started += StartPulling;
        pullingReference.action.canceled += StopPulling;
        teleportMode = false;
        previousParentTransform = null;
        laserColor = laser.material.color;
    }
    private void OnDisable()
    {
        selectReference.action.Disable();
        grabReference.action.Disable();
        teleportModeReference.action.Disable();
        selectReference.action.started -= Select;
        selectReference.action.canceled -= Unselect;
        grabReference.action.started -= Grab;
        grabReference.action.canceled -= Released; 
        teleportModeReference.action.started -= TeleportModeActivate;
        teleportModeReference.action.canceled -= TeleportModeCancel;
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
        RaycastHit hit;
        if (teleportMode) // Teleport mode: fires a raycast that places the reticle
        {
            Physics.Raycast(transform.position, transform.forward, out hit, teleportationDistance, floorMask);
            if (hit.point != Vector3.zero) //Raycast found the floor: place reticle
            {
                reticle.SetActive(true);
                reticle.transform.position = hit.point;
                reticle.transform.LookAt(new Vector3(playerTransform.position.x, 0f, playerTransform.position.z));
            }
            else //Raycast was not able to find the floor: hides the reticle
            {
                reticle.SetActive(false);
            }
            return;
        }
        //Not in teleport mode: hides the reticle, searches for UI
        reticle.SetActive(false);
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
        GravityScript grav = grabbingTransform.GetComponent<GravityScript>();
        if (grav != null)
        {
            gravEnabled = grav.enabled;
            grav.enabled = false;
            grabbingTransform.GetComponent<Rigidbody>().velocity = Vector3.zero; // Also set the grabbed objects velocity to zero
        }
    }
    private void Select(InputAction.CallbackContext ctx)
    { 
        if (teleportMode) // Teleport mode and user presses select: raycast checks for a floor and tries to teleport the user to it
        {
            Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, teleportationDistance, floorMask);
            if (hit.point != Vector3.zero) //Raycast detected the floor: teleport
            {
                GetComponent<AudioSource>().PlayOneShot(teleportAudio);
                playerTransform.position = hit.point + 0.1f * Vector3.up;
            }
        }
        else //Not in teleport mode and user presses select: raycast checks for UI (UICollider or Slider) and interacts with it
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
        }
    }
    private void Unselect(InputAction.CallbackContext ctx)
    {
        holdingSlider = false;
    }
    private void TeleportModeCancel(InputAction.CallbackContext ctx)
    {
        teleportMode = false;
    }
    private void TeleportModeActivate(InputAction.CallbackContext ctx)
    {
        teleportMode = true;
    }
    private void CheckSlider(RaycastHit hit)
    {
        Slider activeSlider = hit.collider.gameObject.GetComponent<Slider>();
        if (activeSlider != null) //Collider is a slider: proceeds to interact with slider. 
        {
            if (activeSlider.transform.InverseTransformPoint(hit.point).x > activeSlider.transform.InverseTransformPoint(activeSlider.handleRect.position).x) //Raycast lands to the right of handle: handle moves right
            {
                activeSlider.value += sliderIncrement;
            }
            else //Raycast lands to the left of handle: handle moves left
            {
                activeSlider.value -= sliderIncrement;
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
    }
}
