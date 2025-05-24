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

    private Rigidbody rb;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -1.0f, 0); // 무게 중심을 아래로
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
