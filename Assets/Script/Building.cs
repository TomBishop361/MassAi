using UnityEngine;
using UnityEngine.Rendering;

public class Building : MonoBehaviour
{
    public FlowFieldManager manager;
    public int influence = 5;
    public BuildingData _BuildingData;
    int currentHealth = 0;
    public BoxCollider boxCollider;
    public MeshRenderer _Renderer;
    public bool preplacedBuilding;
    public bool IsValidLocation = true;
    bool isBuilt = false;
    
    public HealthBar healthBar;

    private void Awake()
    {
        currentHealth = _BuildingData.Health;
        
    }
    private void Start()
    {
        
        if (preplacedBuilding) buildComplete();

    }

    void placeOnFlowField()
    {
        manager = FlowFieldManager.Instance;
        manager.AddSecondaryTarget(this.transform, influence);
    }


    public void AdjustHealh(int healthChange)
    {        
        currentHealth = Mathf.Clamp(currentHealth + healthChange, 0, _BuildingData.Health) ;          
        healthBar.setHealth((float)currentHealth / (float)_BuildingData.Health);
        if (currentHealth < _BuildingData.Health) healthBar.displayHealthBar();
        if (currentHealth == _BuildingData.Health) healthBar.hideHealthBar();        
    }

    private void Update()
    {
        if (!isBuilt)
        {
            int count = 0;
            Collider[] hit = Physics.OverlapBox(transform.position, boxCollider.size * 0.5f,transform.rotation);
            if (hit.Length > 0)
            {
                foreach (Collider c in hit)
                {
                    if (c.gameObject.CompareTag("Building"))
                    {
                        if (c != boxCollider && IsValidLocation)
                        {

                            IsValidLocation = false;
                            _Renderer.materials[1].SetColor("_OutlineColour", new Color(1, 0, 0));
                        }
                        count++;
                    }                  
                    
                }
                if (count == 1 && !IsValidLocation)
                {
                    _Renderer.materials[1].SetColor("_OutlineColour", new Color(0, 1, 0));
                    IsValidLocation = true;
                }
            }
            
        }
    }


    public void buildComplete()
    {
        _Renderer.materials[1].SetFloat("_IsBuilt", 1);
        isBuilt = true;
        healthBar.hideHealthBar();
        placeOnFlowField();
    }

    private void OnDestroy()
    {
       // manager.buildingDestroyed(transform, influence);
    }
}
