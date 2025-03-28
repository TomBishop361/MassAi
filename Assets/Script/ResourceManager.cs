using TMPro;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    int Food;
    public TextMeshProUGUI FoodText;
    int Gold;
    public TextMeshProUGUI GoldText;
    int Metal;
    public TextMeshProUGUI MetalText;
    public int Wood;
    public TextMeshProUGUI WoodText;
    int Stone;
    public TextMeshProUGUI StoneText;
    int Population;
    public int PopulationCap;
    public TextMeshProUGUI PopulationText;

    private void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);

        FoodText.text = Food.ToString();

        GoldText.text = Gold.ToString();

        MetalText.text = Metal.ToString();

        WoodText.text = Wood.ToString();  

        StoneText.text = Stone.ToString();

        PopulationText.text = $"{Population}/{PopulationCap}";

    }

    //Gets Variable from string and Adds amount to it
    public void AdjustResource(int amount, string type)
    {
        int currentValue = (int)GetType().GetField(type).GetValue(this);
        GetType().GetField(type).SetValue(this, currentValue + amount);
        if (type != "Population" && type != "PopulationCap")
        {
            TextMeshProUGUI text = (TextMeshProUGUI)GetType().GetField(type + "Text").GetValue(this);
            text.text = (currentValue + amount).ToString();
        }
        else PopulationText.text = $"{Population}/{PopulationCap}";
    }

}
