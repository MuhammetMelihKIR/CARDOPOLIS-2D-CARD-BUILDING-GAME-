using System;
using System.Collections.Generic;
using UnityEngine;
public class Grid : MonoBehaviour
{
    private List<BuildingBase> _buildingBases;
    private Tile[,] _tiles;
    private int sizeX, sizeY;
    
    [SerializeField] private EventManager EventManager;
    [SerializeField] private BuildingsList BuildingsList;
    [SerializeField] private Tile TilePrefab;
    [SerializeField] private BuildingSO TestBuilding;
    
    [HideInInspector] public int index;
    
    private void Awake()
    {
        _buildingBases = new List<BuildingBase>();
        _tiles = new Tile[0,0];
        
        CreateEmptyGrid(10, 10);
        
    }
    public void IndexUpdate()
    {
        index= (index + 1) % 2;
        if (index>=2)
        {
            index = 0;
        }
    }
   
    private void CreateEmptyGrid(int sizeX, int sizeY)
    {
        this.sizeX = sizeX;
        this.sizeY = sizeY;
        _tiles = new Tile[sizeX, sizeY];
        for (var index0 = 0; index0 < _tiles.GetLength(0); index0++)
        for (var index1 = 0; index1 < _tiles.GetLength(1); index1++)
        {
            Tile i = _tiles[index0, index1];
            i = Instantiate(TilePrefab, this.transform);
            i.Initialize(new Vector2(index0, index1), this.transform);
            _tiles[index0, index1] = i;
        }
    }

    private Vector2 ScreenToGridPoint(Vector3 point)
    {
        Vector3 gridPoint = transform.InverseTransformPoint(point);
        if (gridPoint.x >= sizeX + 0.5 || gridPoint.x <= -0.5 || gridPoint.y >= sizeY + 0.5 || gridPoint.y <= -0.5)
        {
            return -Vector2.one;
        }
        
        gridPoint = new Vector3((int) Mathf.Round(gridPoint.x), (int) Mathf.Round(gridPoint.y));
        return gridPoint;
    }

    private void OnMouseUp(Vector3 pos) 
    {
        if (CheckPos(TestBuilding, ScreenToGridPoint(pos)))
        {
            SetBuilding(TestBuilding, ScreenToGridPoint(pos));
        }
    }

    public void TryPlaceBuilding(Vector3 pos, BuildingSO buildingSO)
    {
        if (CheckPos(buildingSO, ScreenToGridPoint(pos)))
        {
            SetBuilding(buildingSO, ScreenToGridPoint(pos));
        }
    }

    private bool CheckPos(BuildingSO buildingSo, Vector2 pos)
    {
        for (int i = 0; i < buildingSo.size.Length; i++)
        {
            for (int j = 0; j < buildingSo.size[i]; j++)
            {
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
                try
                {
                    if (_tiles[(int)pos.x + offsetJ, (int)pos.y + offsetI].tileType != Tile.TileTypes.Empty)
                    {
                        return false;
                    }
                        
                }
                catch (IndexOutOfRangeException)
                {
                    return false;
                }
            }
        }
     
        return true;
    }

    public bool CheckIfSuitable(BuildingSO buildingSo, Vector3 pos)
    {
        return (CheckPos(buildingSo, ScreenToGridPoint(pos)));
    }
 
    public void SetBuildingPosition( BuildingSO buildingSO, Vector2 pos)
    {
        _tiles[(int)pos.x, (int)pos.y].SetBuildingBase(buildingSO.id);

        for (int i = 0; i < buildingSO.size.Length; i++)
        {
            for (int j = 0; j < buildingSO.size[i]; j++)             
            { 
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
                _tiles[(int)pos.x + offsetJ, (int)pos.y + offsetI].SetBuilding();
            }
        }

        index = 0;
    }

    private Vector3 GridToWorldPoint(Vector2 pos)
    {
        return transform.TransformPoint(pos);
    }

    private void SetBuilding(BuildingSO buildingSO, Vector2 pos)
    {
        SetBuildingPosition(buildingSO,pos);
        BuildingBase buildingBase = Instantiate(buildingSO.prefab);
        buildingBase.Initialize(buildingSO);
        buildingBase.transform.position = GridToWorldPoint(pos);
        EventManager.BuildingPlace(buildingBase,false);
        _buildingBases.Add(buildingBase);
    }
    public void SaveGrid()
    {
        PlayerPrefs.SetInt("sizeX", sizeX);
        PlayerPrefs.SetInt("sizeY", sizeY);
        string saveData = "";

        for (int i = 0; i < sizeX; i++)
        {
            for (int j = 0; j < sizeY; j++)
            {
                Tile tile = _tiles[i, j];
                if (tile.tileType != Tile.TileTypes.Empty)
                {
                    saveData += $"{i}_{j}_{(tile.isBuildingBase ? tile.baseTypeID.ToString() : "B")},";
                }
                else
                {
                    saveData += "E,";
                }
            }
        }

        PlayerPrefs.SetString("Grid", saveData);
    }

    public void LoadGrid()
    {
        DestroyGrid();
        
        if (PlayerPrefs.HasKey("Grid"))
        {
           string loadData = PlayerPrefs.GetString("Grid");
           string[] dataEntries = loadData.Split(',');

           int x, y;
           x = PlayerPrefs.GetInt("sizeX");
           y = PlayerPrefs.GetInt("sizeY");
           CreateEmptyGrid(x, y);

           foreach (string dataEntry in dataEntries)
           {
               if (!string.IsNullOrEmpty(dataEntry))
               {
                   string[] entryParts = dataEntry.Split('_');

                   if (entryParts.Length == 3)
                   {
                       int i, j;
                       if (int.TryParse(entryParts[0], out i) && int.TryParse(entryParts[1], out j))
                       {
                           Tile tile = _tiles[i, j];

                           if (entryParts[2] != "E")
                           {
                               tile.SetBuilding();
                               if (entryParts[2] != "B")
                               {
                                   int id;
                                   if (int.TryParse(entryParts[2], out id))
                                   {
                                       tile.SetBuildingBase(id);
                                       BuildingBase buildingBase = Instantiate(BuildingsList.buildings[id].prefab);
                                       buildingBase.Initialize(BuildingsList.buildings[id]);
                                       buildingBase.transform.position = GridToWorldPoint(new Vector2(i, j));
                                       EventManager.BuildingPlace(buildingBase, true);
                                       _buildingBases.Add(buildingBase);
                                   }
                                       
                               }
                           }
                       }
                       
                   }
                   
               }
           }
        }
    
    }
    

    private void DestroyGrid()
    {
        foreach (Tile i in _tiles)
        {
            Destroy(i.gameObject);
        }
        foreach(BuildingBase i in _buildingBases) Destroy(i.gameObject);
        _buildingBases.Clear();
    }

    public void NewGame()
    {
        DestroyGrid();
        CreateEmptyGrid(10,10);
    }
}