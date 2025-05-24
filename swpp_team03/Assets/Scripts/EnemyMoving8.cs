using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoving8 : MonoBehaviour
{
    public float centerAX;
    public float centerAZ;
    public float centerBX;
    public float centerBZ;

    public float angularSpeed = 1f;
    public float detectionRadius = 5f;
    public float pursuitSpeed = 3f;
    public float circleRecalcRadius = 2f;

    private Rigidbody rb;
    private float angle = 0f;
    private bool usingFirstCircle = true;
    private bool isChasing = false;
    private Transform targetPlayer;
    private Vector3 currentCenterA;
    private Vector3 currentCenterB;
    private float radius;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        // Terrain 높이 보정 포함한 중심 위치 설정
        float yA = Terrain.activeTerrain.SampleHeight(new Vector3(centerAX, 0, centerAZ));
        float yB = Terrain.activeTerrain.SampleHeight(new Vector3(centerBX, 0, centerBZ));
        currentCenterA = new Vector3(centerAX, yA, centerAZ);
        currentCenterB = new Vector3(centerBX, yB, centerBZ);

        // 반지름 계산
        radius = Vector3.Distance(currentCenterA, currentCenterB) / 2f;
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

            if (angle >= Mathf.PI * 2f)
            {
                angle = 0f;
                usingFirstCircle = !usingFirstCircle;
            }

            Vector3 center = usingFirstCircle ? currentCenterA : currentCenterB;

            float x = center.x + Mathf.Cos(angle) * radius;
            float z = center.z + Mathf.Sin(angle) * radius;
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

                Vector3 newA = transform.position + forwardFlat * circleRecalcRadius;
                Vector3 newB = transform.position - forwardFlat * circleRecalcRadius;

                float newAY = Terrain.activeTerrain.SampleHeight(newA);
                float newBY = Terrain.activeTerrain.SampleHeight(newB);

                currentCenterA = new Vector3(newA.x, newAY, newA.z);
                currentCenterB = new Vector3(newB.x, newBY, newB.z);

                radius = circleRecalcRadius;
                angle = 0f;
                usingFirstCircle = true;
            }
        }
    }

    Vector3 SlideAlongObstacle(Vector3 move)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, move.normalized, out hit, move.magnitude + 0.1f))
        {
            Vector3 slide = Vector3.ProjectOnPlane(move, hit.normal);
            return slide;
        }
        return move;
    }
}
