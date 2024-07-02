using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Enemy enemy;
    private Vector2 Velocity;
    private Vector2 SmoothDeltaPosition;

    void Awake(){
		enemy.animator.applyRootMotion = true;
        if(enemy.navMeshAgent!=null){
            enemy.navMeshAgent.updatePosition = false;
            enemy.navMeshAgent.updateRotation = true;
        }
    }

    void FixedUpdate(){
        if(enemy.health<=0) return;
        SynchronizeAnimatorAndAgent();
    }
    
    private void SynchronizeAnimatorAndAgent()
    {
        Vector3 worldDeltaPosition = enemy.navMeshAgent.nextPosition - transform.position;
        worldDeltaPosition.y = 0;

        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        float smooth = Mathf.Min(1, Time.fixedDeltaTime / 0.1f);
        SmoothDeltaPosition = Vector2.Lerp(SmoothDeltaPosition, deltaPosition, smooth);

        Velocity = SmoothDeltaPosition / Time.fixedDeltaTime;
        if (enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance)
        {
            Velocity = Vector2.Lerp(
                Vector2.zero, 
                Velocity, 
                enemy.navMeshAgent.remainingDistance / enemy.navMeshAgent.stoppingDistance
            );
        }

        bool shouldMove = Velocity.magnitude > 0.5f
            && enemy.navMeshAgent.remainingDistance > enemy.navMeshAgent.stoppingDistance;

        //enemy.animator.SetBool("move", shouldMove);
        //enemy.animator.SetFloat("locomotion", Velocity.magnitude);

        float deltaMagnitude = worldDeltaPosition.magnitude;
        if (deltaMagnitude > enemy.navMeshAgent.radius / 2f)
        {
            transform.position = Vector3.Lerp(
                enemy.animator.rootPosition,
                enemy.navMeshAgent.nextPosition,
                smooth
            );
        }
    }

    private void OnAnimatorMove(){
        Vector3 rootPosition = enemy.animator.rootPosition;
        rootPosition.y = enemy.navMeshAgent.nextPosition.y;
        transform.position = rootPosition;
        enemy.navMeshAgent.nextPosition = rootPosition;
    }
}
