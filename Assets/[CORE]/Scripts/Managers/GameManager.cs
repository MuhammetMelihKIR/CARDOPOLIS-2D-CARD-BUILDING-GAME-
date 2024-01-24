using System;
using UnityEngine;
public class GameManager : MonoBehaviour
{
    [SerializeField] private Camera MainCamera;
    public int GoldCount,GemCount;
    [SerializeField] private EventManager EventManager;
    private void OnEnable()
    {
        EventManager.reduceResources.AddListener(OnReduceResources);
        EventManager.addResources.AddListener(OnAddResources);
    }
    private void OnDisable()
    {
        EventManager.reduceResources.RemoveListener(OnReduceResources);
        EventManager.addResources.RemoveListener(OnAddResources);
    }
    private void Update()
    {
        EventManager.ResourcesChange(GoldCount,GemCount);
        if (Input.GetMouseButtonUp(0))
        {
            EventManager.MouseUp(MainCamera.ScreenToWorldPoint(Input.mousePosition));
        }
    }
    private void OnReduceResources(int gold, int gem)
    {
        GoldCount -= gold;
        GemCount -= gem;
        EventManager.ResourcesChange(GoldCount,GemCount);
    }
    private void OnAddResources(int gold, int gem)
    {
        GoldCount += gold;
        GemCount += gem;
        EventManager.ResourcesChange(GoldCount,GemCount);
    }
    public void Restart(int gem,int gold)
    {
        GemCount = gem;
        GoldCount = gold;
        EventManager.ResourcesChange(GoldCount,GemCount);
    }
    
    public void Save()
    {
        PlayerPrefs.SetInt("Gold",GoldCount);
        PlayerPrefs.SetInt("Gem",GemCount);
    }

    public void Load()
    {
        GoldCount = PlayerPrefs.GetInt("Gold");
        GemCount = PlayerPrefs.GetInt(("Gem"));
        EventManager.ResourcesChange(GoldCount,GemCount);
    }
}
