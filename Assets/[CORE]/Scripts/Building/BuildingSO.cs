using UnityEngine;

[CreateAssetMenu]
public class BuildingSO : ScriptableObject
{
    public int id;
    public string buildingName;
    public Sprite buildingSprite;
    public int[] size;
    public int goldCost, gemCost;
    
    [Header("PRODUCTION")]
    public float cooldownProduction;
    public int goldProduction, gemProduction;
    public BuildingBase prefab;

}
