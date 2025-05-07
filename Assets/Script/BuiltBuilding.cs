using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BuiltBuilding : MonoBehaviour
{
    FlowFieldManager manager;
    ResourceManager managerResource;
    public Health health;
    BuildingData buildingData;
    public List<GameObject> validTargets = new List<GameObject>();    
    

    public void construct(FlowFieldManager _manager, BuildingData _buildingData, List<GameObject> _validTargets,Building buildingScript)
    {
        manager = _manager;
        health._maxHealth = _buildingData.Health;
        buildingData = _buildingData;
        foreach (GameObject target in _validTargets) { 
            validTargets.Add(target);
        }
        health.healthDepleted += buildingDestroyed;
        Destroy(buildingScript);
        managerResource = ResourceManager.Instance;
        if (buildingData.Produce != BuildingData.Produces.None && validTargets.Count> 0)
        {            
            StartCoroutine(ProduceResource());            
        }
    }

    IEnumerator ProduceResource()
    {
        while (true)
        {
            yield return new WaitForSeconds(buildingData.productionSpeed);
            managerResource.AdjustResource(validTargets.Count, $"{buildingData.Produce}");
        }
    }


    void buildingDestroyed()
    {
        Destroy(gameObject);
    }
    private void OnDisable()
    {
        health.healthDepleted -= buildingDestroyed;
        manager.buildingDestroyed(transform, buildingData.influence);
        
    }
    
}
