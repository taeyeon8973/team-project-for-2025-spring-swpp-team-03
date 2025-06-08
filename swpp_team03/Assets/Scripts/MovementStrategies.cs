using UnityEngine;

// 8자 이동 전략
public class FigureEightMovementStrategy : IMovementStrategy
{
    private float angle = 0f;
    private int rotatingLeft = 1;
    private Vector3 jointPosition;
    private Vector3 leftCenter;
    private Vector3 rightCenter;
    private Vector3 presentCenter;
    private float angularSpeed;
    private float figureEightRadius;

    public void Initialize(EnemyMoving enemy, Vector3 startPosition)
    {
        figureEightRadius = enemy.figureEightRadius;
        angularSpeed = enemy.speed / figureEightRadius;
        
        float y = Terrain.activeTerrain.SampleHeight(startPosition);
        jointPosition = new Vector3(startPosition.x, y, startPosition.z);

        Vector3 left = jointPosition + new Vector3(-figureEightRadius, 0, 0);
        Vector3 right = jointPosition + new Vector3(figureEightRadius, 0, 0);

        left.y = Terrain.activeTerrain.SampleHeight(left);
        right.y = Terrain.activeTerrain.SampleHeight(right);

        leftCenter = left;
        rightCenter = right;
        presentCenter = leftCenter;
    }

    public void Move(EnemyMoving enemy, Rigidbody rb)
    {
        angle += angularSpeed * Time.fixedDeltaTime * rotatingLeft;

        float x = presentCenter.x + Mathf.Cos(angle) * figureEightRadius;
        float z = presentCenter.z + Mathf.Sin(angle) * figureEightRadius;
        Vector3 targetPos = new Vector3(x, rb.position.y, z);
        Vector3 move = targetPos - rb.position;
        
        if (angle >= 2f * Mathf.PI)
        {
            angle = Mathf.PI;
            rotatingLeft = -1;
            presentCenter = rightCenter;
        }
        else if (angle <= -Mathf.PI)
        {
            angle = 0;
            rotatingLeft = 1;
            presentCenter = leftCenter;
        }
        
        rb.MovePosition(rb.position + enemy.SlideAlongObstacle(move));
    }

    public string GetStrategyName()
    {
        return "FigureEight";
    }
    
    public void UpdateCentersAfterChasing(Vector3 currentPos, float figureEightRadius)
    {
        Vector3 left = currentPos + new Vector3(-figureEightRadius, 0, 0);
        Vector3 right = currentPos + new Vector3(figureEightRadius, 0, 0);

        left.y = Terrain.activeTerrain.SampleHeight(left);
        right.y = Terrain.activeTerrain.SampleHeight(right);

        leftCenter = left;
        rightCenter = right;
        presentCenter = leftCenter;
        rotatingLeft = 1;
        angle = 0;
    }
}

// 원형 이동 전략
public class CircularMovementStrategy : IMovementStrategy
{
    private float angle = 0f;
    private Vector3 circleCenter;
    private float angularSpeed;
    private float radius;

    public void Initialize(EnemyMoving enemy, Vector3 startPosition)
    {
        radius = enemy.figureEightRadius;
        angularSpeed = enemy.speed / radius;
        
        float y = Terrain.activeTerrain.SampleHeight(startPosition);
        circleCenter = new Vector3(startPosition.x, y, startPosition.z);
    }

    public void Move(EnemyMoving enemy, Rigidbody rb)
    {
        angle += angularSpeed * Time.fixedDeltaTime;

        float x = circleCenter.x + Mathf.Cos(angle) * radius;
        float z = circleCenter.z + Mathf.Sin(angle) * radius;
        Vector3 targetPos = new Vector3(x, rb.position.y, z);
        Vector3 move = targetPos - rb.position;
        
        rb.MovePosition(rb.position + enemy.SlideAlongObstacle(move));
    }

    public string GetStrategyName()
    {
        return "Circular";
    }
    
    public void UpdateCenterAfterChasing(Vector3 currentPos, float currentAngle, float radius)
    {
        Vector3 newCenter = currentPos - new Vector3(Mathf.Cos(currentAngle), 0, Mathf.Sin(currentAngle)) * radius;
        newCenter.y = Terrain.activeTerrain.SampleHeight(newCenter);
        circleCenter = newCenter;
    }
    
    public float GetAngle()
    {
        return angle;
    }
}

// 추격 이동 전략
public class ChasingMovementStrategy : IMovementStrategy
{
    private Transform target;

    public void Initialize(EnemyMoving enemy, Vector3 startPosition)
    {
        target = GameObject.FindWithTag("Player")?.transform;
    }

    public void Move(EnemyMoving enemy, Rigidbody rb)
    {
        if (target != null)
        {
            Vector3 direction = (target.position - rb.position).normalized;
            Vector3 move = direction * enemy.speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + enemy.SlideAlongObstacle(move));
        }
    }

    public string GetStrategyName()
    {
        return "Chasing";
    }
} 