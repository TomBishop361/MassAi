using UnityEngine;


[CreateAssetMenu(fileName = "UnitData", menuName = "ScriptableObjects/UnitData", order = 1)]
public class UnitData : ScriptableObject 
{
    public int Health;
    
    public float moveSpeed;
    
    public float attackSpeed;
    public float damage;
    public unitType UnitType;
    public team Team;
    
    public enum unitType
    {
        None, Ranged, Melee, Brute, WarMachine
    }

   public enum team
    {
        Ally, Enemy
    }

  
}
