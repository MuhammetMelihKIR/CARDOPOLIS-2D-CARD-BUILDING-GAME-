using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingCard : MonoBehaviour
{
    private Button _button;
    
    [SerializeField] private EventManager EventManager;
    [SerializeField] private BuildingSO BuildingSo;
    [SerializeField] private TextMeshProUGUI  NameText, GoldCostText, GemCostText;
    [SerializeField] private TextMeshProUGUI  GoldProductionText, GemProductionText;
    [SerializeField] private Image Image;

    private void Awake()
    {
        _button = GetComponent<Button>();
        Init();
    }
    private void OnEnable()
    {
        EventManager.resourcesChange.AddListener(OnResourceChange);
    }

    private void OnDisable()
    {
         EventManager.resourcesChange.AddListener(OnResourceChange);
    }
    private void Init()
    {
        Image.sprite = BuildingSo.buildingSprite;
        NameText.text = BuildingSo.buildingName;
        GoldCostText.text = BuildingSo.goldCost.ToString();
        GemCostText.text = BuildingSo.gemCost.ToString();

        GoldProductionText.text = BuildingSo.goldProduction.ToString();
        GemProductionText.text = BuildingSo.gemProduction.ToString();
    }
    public void OnPointerDown()
    {
        if (_button.interactable)
        {
            EventManager.PointerDownOnBuildingCard(BuildingSo);
        }
    }
    private void OnResourceChange(int goldCost, int gemCost)
    {
        _button.interactable = true;
        if ( goldCost < BuildingSo.goldCost  ||  gemCost<BuildingSo.gemCost   )
        {
            _button.interactable = false;
        }
    }
    
}
