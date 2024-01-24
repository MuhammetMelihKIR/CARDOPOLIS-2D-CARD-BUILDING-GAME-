using UnityEngine;
public class BuildingBase : MonoBehaviour
{
   private ProgressBar _progressBar;
   private SpriteRenderer _spriteRenderer;
   private float _cooldownProduction;
   private float _time;
   private bool _isWorking = true;
   
   [SerializeField] private EventManager EventManager;
   
   [HideInInspector] public int goldCost, gemCost,goldProduction, gemProduction; 
   private void Awake()
   {
      _spriteRenderer = GetComponent<SpriteRenderer>();
   }
   private void Update()
   {
       if (_isWorking)
       {
          _time += Time.deltaTime;
          _progressBar.SetBar(_time,_cooldownProduction);
          if (_time >= _cooldownProduction)
          {
             _time -= _cooldownProduction;
             GainResources();
          }
       } 
   } 
   public void Initialize(BuildingSO buildingSo)
   {
      _spriteRenderer.sprite = buildingSo.buildingSprite;
      
      goldCost = buildingSo.goldCost;
      gemCost = buildingSo.gemCost;

      goldProduction = buildingSo.goldProduction;
      gemProduction = buildingSo.gemProduction;

      _cooldownProduction = buildingSo.cooldownProduction;
   }

   private void GainResources()
   {
      EventManager.AddResources(goldProduction,gemProduction);
      _progressBar.FloatText(goldProduction,gemProduction);
   }

   public void SetCooldownBar(ProgressBar progressBar, bool isLoad)
   {
      _progressBar = progressBar;
      if (!isLoad)
      {
         _progressBar.FloatText(-goldCost,-gemCost);
      }
   }
    
}
