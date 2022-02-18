using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    private TowerBlueprint towerToBuild;

    [SerializeField]
    List<TowerBlueprint> allTowers;

    public bool CanBuild { get { return towerToBuild != null; } }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one BuildManager in scene!");
            return;
        }

        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
            
    }

    public Tower GetTowerToBuild()
    {
        return towerToBuild.towerPrefab;
    }

    public void SetTowerToBuild(string id)
    {
        towerToBuild = allTowers.Find(x => x.towerId == id);
    }
}
