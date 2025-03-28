using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData", menuName = "ScriptableObjects/BuildingData", order = 1)]
public class BuildingData : ScriptableObject
{
    public int Health;

    public string BuildingName;

    public RecourseCost Cost;

    public SecondRecourceCost SecondaryCost;

    public Produces Produce;

    public float ResourceRange;
    
    public enum RecourseCost
    {
        Wood, Coin, Stone, Food
    }

    
    public enum SecondRecourceCost
    {
        None, Wood, Coin, Stone, Food
    }

    
    public enum Produces
    {
        None, Food, Coin, Stone, Wood, Population
    }

    [Tooltip("set to 0 for flat one time increase")]
    public int productionSpeed;


}
