using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 5, -10);
    public float followSpeed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        Quaternion targetRotation = Quaternion.Euler(0, player.eulerAngles.y, 0);

        Vector3 desiredPosition = player.position + targetRotation * offset;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        Quaternion lookRotation = Quaternion.LookRotation((player.position + Vector3.up * 1.5f) - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, followSpeed * Time.deltaTime);
    }

}
