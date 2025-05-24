using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoving8 : MonoBehaviour
{
    public Transform centerA;
    public Transform centerB;
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

        // 초기 중심과 반지름 계산
        currentCenterA = centerA.position;
        currentCenterB = centerB.position;
        radius = Vector3.Distance(currentCenterA, currentCenterB) / 2f;
    }

    void FixedUpdate()
    {
        UpdateTarget();

        if (isChasing && targetPlayer != null)
        {
            // 추적
            Vector3 direction = (targetPlayer.position - transform.position).normalized;
            Vector3 move = direction * pursuitSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + SlideAlongObstacle(move));
        }
        else
        {
            // 8자 모양 회전
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

                // 현재 위치와 전방 방향을 기준으로 새 중심 2개 생성
                Vector3 forwardFlat = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;

                currentCenterA = transform.position + forwardFlat * circleRecalcRadius;
                currentCenterB = transform.position - forwardFlat * circleRecalcRadius;

                radius = circleRecalcRadius;
                angle = 0f;
                usingFirstCircle = true;
            }
        }
    }

    // 벽을 따라 미끄러지듯 이동
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
