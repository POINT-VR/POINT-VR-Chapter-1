using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    [SerializeField] private Transform lookAt;
    [SerializeField] private Transform transformToFollow;
    [SerializeField] private float followSpeed;
    private Transform _thisTransform;
    void Start()
    {
        _thisTransform = transform;
    }


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
