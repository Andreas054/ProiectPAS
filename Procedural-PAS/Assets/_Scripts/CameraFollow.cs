using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float FollowSpeed = 2f;
    public float yOffset = 1f;
    public Transform target;

    private void Awake()
    {
        transform.position = target.position;
    }

    //void Update()
    //{
    //    Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, -10f);
    //    transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    //}

    void LateUpdate()
    {
        Vector3 newPos = new Vector3(target.position.x, target.position.y + yOffset, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, FollowSpeed * Time.deltaTime);
    }
}
