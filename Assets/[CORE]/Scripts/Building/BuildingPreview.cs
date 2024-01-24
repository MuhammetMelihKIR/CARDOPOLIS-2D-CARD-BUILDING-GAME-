using UnityEngine;
public class BuildingPreview : MonoBehaviour
{
   private BuildingSO _buildingSo;
   
   [SerializeField] private Grid Grid;
   [SerializeField] private GameObject TilePrefab;

   [HideInInspector] public int index, angleX;
   private void Update()
   {
      transform.localRotation = Quaternion.Euler(angleX, 0,0);
   }

   public void RotateBuilding()
   {
      index = (index + 1) % 2;
      angleX += 180;
      if (angleX>=360)
      {
         angleX = 0;
      }
   }
   public void Init(BuildingSO buildingSo)
   {
      _buildingSo = buildingSo;
      for (int i = 0; i < buildingSo.size.Length; i++)
      {
         for (int j = 0; j < buildingSo.size[i]; j++)
         {
            GameObject tile = Instantiate(TilePrefab, transform);
            int offsetI, offsetJ;
          
            if (index== 0)
            {
               offsetJ = j;
               offsetI = -i;
                    
            }
            else
            {
               offsetJ = j;
               offsetI = i;
                   
            }
            tile.transform.position = transform.TransformPoint(new Vector3(offsetJ, offsetI, 0));
            
         }
      }
   }

   public void DestroyPreview()
   {
      foreach (Transform i in transform)
      {
         Destroy(i.gameObject);
         
      }
   }

   public void ChangeColorGreen()
   {
      foreach (Transform i in transform)
      {
         i.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, .5f);
      }
   }
   public void ChangeColorRed()
   {
      foreach (Transform i in transform)
      {
         i.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, .5f);
      }
   }
}
