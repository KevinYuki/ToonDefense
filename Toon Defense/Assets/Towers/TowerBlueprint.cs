using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TowerBlueprint
{
    public Tower towerPrefab;
    public string towerId;

    public TowerBlueprint()
    {
        
    }

    public TowerBlueprint(string id, Tower towerPrefab)
    {
        towerId = id;
        this.towerPrefab = towerPrefab;
    }
}
