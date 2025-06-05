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
    public bool isImmune = false;
    public int enemyDamage = 10;
    public float immuneTime = 3.0f;
    private GameObject gameManager;
    private StatusBar statusBarScript;
    private DashForward dashForwardScript;
    private HyunmuMode hyunmuModeScript;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.centerOfMass = new Vector3(0, centerOfGravityY, 0); // 무게 중심을 아래로
        Vector3 com = rb.centerOfMass;
        com.y = centerOfGravityY;
        rb.centerOfMass = com;

        Physics.gravity = Physics.gravity * gravityStrength;
        gameManager = GameObject.Find("GameManager");
        statusBarScript = gameManager.GetComponent<StatusBar>();
        dashForwardScript = GetComponent<DashForward>();
        hyunmuModeScript = GetComponent<HyunmuMode>();
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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            bool immune = isImmune || dashForwardScript.isDashing || hyunmuModeScript.isInvincible;
            if (!immune)
            {
                statusBarScript.TakeDamage(enemyDamage);
                StartCoroutine(ImmuneCoroutine());
            }
        }
    }

    private IEnumerator ImmuneCoroutine()
    {
        isImmune = true;
        yield return new WaitForSeconds(immuneTime);
        isImmune = false;
    }

    void OnTriggerEnter(Collider other)
    {
        // TODO : Item logic
    }


}
