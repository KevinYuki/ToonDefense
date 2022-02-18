using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GunTower : Tower
{
    [SerializeField]
    float turnSpeed = 1;


    [SerializeField]
    Transform weapon;

    GameObject target;
    bool hasTarget { get => target != null; }

    List<string> targetStyles = new List<string> { "First", "Last", "Strongest", "Weakest" };
    string targetStyleSelected;


    // Start is called before the first frame update
    void Start()
    {
        targetStyleSelected = targetStyles.First();
        InvokeRepeating("UpdateTarget", 0, .5f);
    }

    // Update is called once per frame
    void Update()
    {
        LockOnTarget();
    }

    void UpdateTarget()
    {
        switch (targetStyleSelected)
        {
            case ("First"):
                FindFirstTarget();
                break;
            case ("Last"):
                FindLastTarget();
                break;
            case ("Strongest"):
                FindStrongestTarget();
                break;
            case ("Weakest"):
                FindWeakestTarget();
                break;
        }
    }

    private void FindFirstTarget()
    {
        foreach(GameObject enemy in WaveSpawner.EnemiesAlive)
        {
            float distToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if(distToEnemy <= range)
            {
                if(!hasTarget || enemy.GetComponent<EnemyMover>().GetDistanceToEndPoint() < target.GetComponent<EnemyMover>().GetDistanceToEndPoint())
                {
                    target = enemy;
                    Debug.Log(target.name);
                }
            }
        }
    }

    private void FindLastTarget()
    {
        throw new NotImplementedException();
    }

    private void FindStrongestTarget()
    {
        throw new NotImplementedException();
    }

    private void FindWeakestTarget()
    {
        throw new NotImplementedException();
    }

    protected virtual void LockOnTarget()
    {
        if (hasTarget)
        {
            // Target lock on for nearest target
            Vector3 dir = target.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(weapon.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            weapon.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
    }

}
