using UnityEngine;

public class Building : MonoBehaviour
{
   public FlowFieldManager manager;
    public int influence = 5;
    private void Start()
    {
        manager = FlowFieldManager.Instance;
        manager.AddSecondaryTarget(this.transform, influence);
    }

    private void OnDestroy()
    {
        manager.buildingDestroyed();
    }
}
