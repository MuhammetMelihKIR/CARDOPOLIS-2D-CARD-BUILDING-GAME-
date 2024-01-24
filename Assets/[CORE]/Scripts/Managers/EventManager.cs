using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu]
public class EventManager : ScriptableObject
{
    public UnityEvent<Vector3> mouseUp;

    public UnityEvent<int, int> addResources;
    public UnityEvent<int, int> reduceResources;
    public UnityEvent<int, int> resourcesChange;

    public UnityEvent<BuildingSO> pointerDownOnBuildingCard;

    public UnityEvent<BuildingBase, bool> buildingPlaces;

    public UnityEvent saveGame;
    public UnityEvent loadGame;
    
    public void MouseUp(Vector3 position)
    {
         mouseUp.Invoke(position);
    }
    
    public void AddResources(int gold, int gem)
    {
        addResources.Invoke(gold,gem);
    }

    public void ReduceResources(int gold, int gem)
    {
        reduceResources.Invoke(gold,gem);
    }

    public void ResourcesChange(int gold, int gem)
    {
        resourcesChange.Invoke(gold,gem);
    }

    public void PointerDownOnBuildingCard(BuildingSO buildingSo)
    {
        pointerDownOnBuildingCard.Invoke(buildingSo);
    }

    public void BuildingPlace(BuildingBase buildingBase,bool isLoad)
    {
        buildingPlaces.Invoke(buildingBase,isLoad);
        if (!isLoad)
        {
            ReduceResources(buildingBase.goldCost,buildingBase.gemCost);
        }
    }
    public void SaveGame()
    {
        saveGame.Invoke();
    }

    public void LoadGame()
    {
        loadGame.Invoke();
    }

    public void RestartGame()
    {
        PlayerPrefs.DeleteAll();
        LoadGame();
    }
}
