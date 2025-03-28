using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuiltBuilding : MonoBehaviour
{
    FlowFieldManager manager;
    ResourceManager managerResource;
    HealthBar healthBar;
    BuildingData buildingData;
    public List<GameObject> validTargets = new List<GameObject>();    
    int currentHealth = 0;

    public void construct(FlowFieldManager _manager, HealthBar _healthBar, BuildingData _buildingData, List<GameObject> _validTargets,Building buildingScript)
    {
        manager = _manager;
        healthBar = _healthBar; 
        buildingData = _buildingData;
        foreach (GameObject target in _validTargets) { 
            validTargets.Add(target);
        }
        currentHealth = buildingData.Health;
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

    public void AdjustHealh(int healthChange)
    {
        currentHealth = Mathf.Clamp(currentHealth + healthChange, 0, buildingData.Health);
        healthBar.setHealth((float)currentHealth / (float)buildingData.Health);
        if (currentHealth < buildingData.Health) healthBar.displayHealthBar();
        if (currentHealth == buildingData.Health) healthBar.hideHealthBar();
    }

    private void OnDestroy()
    {
        // manager.buildingDestroyed(transform, influence);
    }
}
