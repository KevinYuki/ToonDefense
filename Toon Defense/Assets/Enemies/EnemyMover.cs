using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    List<WayPoint> path = new List<WayPoint>();
    List<WayPoint> pathLeft;

    WayPoint nextWaypoint;

    [SerializeField][Range(0, 5)]
    float speed = 1f;

    protected Animator animator;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        //TODO: arrumar isso
        path = WaveSpawner.instance.path;
        pathLeft = new List<WayPoint>(path);

        StartCoroutine(FollowPath());
    }

    IEnumerator FollowPath()
    {
        foreach(WayPoint waypoint in path)
        {
            nextWaypoint = waypoint;

            Vector3 startPos = transform.position;
            Vector3 endPos = waypoint.transform.position;
            float travelPercent = 0f;

            transform.LookAt(endPos);

            while (travelPercent < 1f)
            {
                if (speed == 0)
                    IdleAnimation();
                else if (speed < 1)
                    WalkAnimation();
                else
                    RunAnimation();

                travelPercent += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(startPos, endPos, travelPercent);
                yield return new WaitForEndOfFrame();
            }
            pathLeft.Remove(nextWaypoint);
        }
        IdleAnimation();
    }

    public float GetDistanceToEndPoint()
    {
        float distance = 0;

        distance = Vector3.Distance(transform.position, nextWaypoint.transform.position);
        foreach(WayPoint wp in pathLeft)
        {
            if(pathLeft.Last() != wp)
                distance += Vector3.Distance(wp.transform.position, pathLeft[pathLeft.IndexOf(wp)+1].transform.position);
        }

        return distance;
    }

    void RunAnimation()
    {
        animator.SetBool("Walk Forward", false);
        animator.SetBool("Run Forward", true);
    }
    void WalkAnimation()
    {
        animator.SetBool("Walk Forward", true);
        animator.SetBool("Run Forward", false);
    }
    void IdleAnimation()
    {
        animator.SetBool("Run Forward", false);
        animator.SetBool("Walk Forward", false);
    }
}
