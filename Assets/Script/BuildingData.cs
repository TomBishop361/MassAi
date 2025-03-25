using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData", menuName = "ScriptableObjects/BuildingData", order = 1)]
public class BuildingData : ScriptableObject
{
    public int Health;

    public string BuildingName;

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
        None, Food, Coin, Stone, Wood
    }

    public int productionSpeed;


}
