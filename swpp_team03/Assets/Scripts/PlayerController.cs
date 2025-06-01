using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public float moveForce = 20f;
    public float maxSpeed = 10f;
    public float rotationSpeed = 60f;
    public float uprightStability = 2f;
    public float groundCheckDistance = 1.0f;
    public float centerOfGravityY = -1.5f;
    public float gravityStrength = 1.0f;

    private Rigidbody rb;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.centerOfMass = new Vector3(0, centerOfGravityY, 0); // 무게 중심을 아래로
        Debug.Log(rb.centerOfMass);
        Vector3 com = rb.centerOfMass;
        com.y = centerOfGravityY;
        rb.centerOfMass = com;

        Physics.gravity = Physics.gravity * gravityStrength;
    }

    // Update is called once per frame
    void Update()
    {
        float turn = Input.GetAxis("Horizontal");
        if (turn != 0)
        {
            Quaternion deltaRotation = Quaternion.Euler(Vector3.up * turn * rotationSpeed * Time.deltaTime);
            rb.MoveRotation(rb.rotation * deltaRotation);   
        }
    }

    void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance + 0.1f);

        float move = Input.GetAxis("Vertical");

        if (isGrounded && move != 0)
        {
            Vector3 force = transform.forward * move * moveForce;
            if (rb.velocity.magnitude < maxSpeed)
                rb.AddForce(force);
        }

        UprightCorrection();

        // 수평 속도 보정: 측면 슬라이딩 방지
        Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
        localVelocity.x *= 0.95f; // 측면 슬라이딩 줄임
        localVelocity.z *= 0.95f; // 급가속 보정
        rb.velocity = transform.TransformDirection(localVelocity);
    }

    void UprightCorrection()
    {
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, Vector3.up) * rb.rotation;
        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, uprightStability * Time.fixedDeltaTime));
    }

    void OnCollisionEnter(Collision collision)
    {
        // TODO : Collision Logic
    }

    void OnTriggerEnter(Collider other)
    {
        // TODO : Item logic
    }
}
