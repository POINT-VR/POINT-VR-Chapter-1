using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    /// <summary>
    /// The GameObject that this will be oriented towards.
    /// </summary>
    [SerializeField] private Transform lookAt;
    /// <summary>
    /// The GameObject that this will be transformed towards.
    /// </summary>
    [SerializeField] private Transform transformToFollow;
    /// <summary>
    /// The speed that this follows at.
    /// </summary>
    [SerializeField] private float followSpeed;
    private Transform _thisTransform;
    void Start()
    {
        _thisTransform = transform;
    }

    /// <summary>
    /// Every update, this faces and transforms the UI.
    /// </summary>
    void Update()
    {
        _thisTransform.LookAt(lookAt, Vector3.up);
        // _thisTransform.Rotate(0f, 180f, 0f);
        var newPosition = _thisTransform.position;
        var followPosition = transformToFollow.position;
        newPosition.x = Mathf.Lerp(newPosition.x, followPosition.x, followSpeed * Time.deltaTime);
        newPosition.y = Mathf.Lerp(newPosition.y, followPosition.y, followSpeed * Time.deltaTime);
        newPosition.z = Mathf.Lerp(newPosition.z, followPosition.z, followSpeed * Time.deltaTime);
    }
}
