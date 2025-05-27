using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoving : MonoBehaviour
{
    public float centerX;  // 중심 x 좌표
    public float centerZ;  // 중심 z 좌표

    public float angularSpeed = 1f;
    public float detectionRadius = 5f;
    public float pursuitSpeed = 3f;
    public float circleRecalcRadius = 2f;

    private Rigidbody rb;
    private float angle;
    private bool isChasing = false;
    private Transform targetPlayer;
    private Vector3 circleCenter;
    private float circleRadius;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        // Terrain 높이 계산
        float centerY = Terrain.activeTerrain.SampleHeight(new Vector3(centerX, 0, centerZ));

        // 중심 위치 설정
        circleCenter = new Vector3(centerX, centerY, centerZ);
        circleRadius = Vector3.Distance(transform.position, circleCenter);
    }

    void FixedUpdate()
    {
        UpdateTarget();

        if (isChasing && targetPlayer != null)
        {
            Vector3 direction = (targetPlayer.position - transform.position).normalized;
            Vector3 move = direction * pursuitSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + SlideAlongObstacle(move));
        }
        else
        {
            angle += angularSpeed * Time.fixedDeltaTime;

            float x = circleCenter.x + Mathf.Cos(angle) * circleRadius;
            float z = circleCenter.z + Mathf.Sin(angle) * circleRadius;
            Vector3 targetPos = new Vector3(x, rb.position.y, z);
            Vector3 move = targetPos - rb.position;

            rb.MovePosition(rb.position + SlideAlongObstacle(move));
        }
    }

    void UpdateTarget()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.transform.position);

        if (dist <= detectionRadius)
        {
            if (!isChasing)
            {
                isChasing = true;
                targetPlayer = player.transform;
            }
        }
        else
        {
            if (isChasing)
            {
                isChasing = false;
                targetPlayer = null;

                Vector3 forwardFlat = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
                Vector3 newCenterXZ = transform.position + forwardFlat * circleRecalcRadius;

                float newY = Terrain.activeTerrain.SampleHeight(newCenterXZ);
                circleCenter = new Vector3(newCenterXZ.x, newY, newCenterXZ.z);

                angle = 0f;
                circleRadius = circleRecalcRadius;
            }
        }
    }

    Vector3 SlideAlongObstacle(Vector3 move)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, move.normalized, out hit, move.magnitude + 0.1f))
        {
            Vector3 slideDirection = Vector3.ProjectOnPlane(move, hit.normal);
            return slideDirection;
        }
        return move;
    }
}
