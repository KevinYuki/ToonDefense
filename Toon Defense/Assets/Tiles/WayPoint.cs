using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    [SerializeField]
    bool isPlaceble;

    private void OnMouseDown()
    {
        if (isPlaceble)
        {
            BuildTower();
            isPlaceble = false;
        }
    }

    void BuildTower()
    {
        var tower = BuildManager.instance.GetTowerToBuild();
        Instantiate(tower, this.transform.position, Quaternion.identity, this.transform);
    }
}
