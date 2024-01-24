using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class UIManager : MonoBehaviour
{
   private List<ProgressBar> _progressBars;
   private bool _isFirstChange = true;

   [SerializeField] private Canvas MainCanvas;
   [SerializeField] private Canvas MenuPanel;
   [SerializeField] private EventManager EventManager;
   [SerializeField] private TextMeshProUGUI MainGoldText, MainGemText;
   [SerializeField] private ProgressBar ProgressBar;
   [SerializeField] private FloatingText FloatingText;
   
   private void Awake()
   {
      _progressBars = new List<ProgressBar>();
      MenuPanel.gameObject.SetActive(true);
   }
   private void OnEnable()
   {
      EventManager.resourcesChange.AddListener(OnResourcesChange);
      EventManager.buildingPlaces.AddListener(OnBuildingPlaced);
   }

   private void OnDisable()
   {
      EventManager.resourcesChange.RemoveListener(OnResourcesChange);
   }

   private void OnResourcesChange( int gold, int gem)
   {
      if(!_isFirstChange)
      {
         CreateFloatingText(int.Parse(MainGoldText.text), gold ,int.Parse(MainGemText.text),gem);
      }
      else
      {
       _isFirstChange = false;  
      }
      
      MainGoldText.text = gold.ToString();
      MainGemText.text = gem.ToString();
   }
   private void CreateFloatingText(int oldGold, int newGold, int oldGem, int newGem)
   {
      int goldDiff = newGold - oldGold;
      int gemDiff = newGem - oldGem;
      
      var GoldTransform = MainGoldText.transform;
      var GemTransform = MainGemText.transform;

      if (goldDiff !=0)
      {
         FloatingText goldFloatingText = Instantiate(FloatingText,MainGoldText.transform);
         goldFloatingText.Init(goldDiff,GoldTransform.position) ;
      }

      if (gemDiff !=0)
      {
         FloatingText gemFloatingText = Instantiate(FloatingText,MainGemText.transform); 
         gemFloatingText.Init(gemDiff, GemTransform.position);
      }
      
   }
   private void OnBuildingPlaced(BuildingBase buildingBase,bool isLoad)
   {
      ProgressBar bar = Instantiate(ProgressBar, MainCanvas.transform);
      bar.transform.position = buildingBase.transform.position ;
      buildingBase.SetCooldownBar(bar,isLoad);
     _progressBars.Add(bar);
   }

   public void ReloadUI()
   {
      if(_progressBars!=null)
      {
         foreach (ProgressBar bar in _progressBars) Destroy(bar.gameObject);
         _progressBars.Clear();
      }
   }
   
   
   #region BUTTONS

   public void OnPressSaveGame()
   {
      EventManager.SaveGame();
      MenuPanel.gameObject.SetActive(false);
   }

   public void OnPressLoadGame()
   {
      EventManager.LoadGame(); 
      MenuPanel.gameObject.SetActive(false);
   }
   public void OnPressRestart()
   {
      ReloadUI();
      _isFirstChange = true;
      EventManager.RestartGame();
      MenuPanel.gameObject.SetActive(false);
   }

   public void OnPressExit()
   {
      EventManager.SaveGame();
      Application.Quit();
   }

   #endregion
  
}
