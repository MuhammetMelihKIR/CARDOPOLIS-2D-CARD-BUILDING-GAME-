using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private SpriteRenderer sr;
    
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private Sprite buildingSprite;
    
    public int indexX;
    public int indexY;
    public bool isBuildingBase;
    public int baseTypeID;
    public enum TileTypes
    {
        Empty,
        Building
    }
    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public TileTypes tileType;

    public void SetEmpty()
    {
        tileType = TileTypes.Empty;
        sr.sprite = emptySprite;
    }

    public void SetBuilding()
    {
        tileType = TileTypes.Building;
        sr.sprite = buildingSprite;
    }

    public void SetBuildingBase(int id)
    {
        isBuildingBase = true;
        baseTypeID = id;

    }

    public void Initialize(Vector2 pos, Transform parent)
    {
        SetEmpty();
        indexX=(int)pos.x;
        indexY=(int)pos.y;
        transform.position = parent.TransformPoint(new Vector3(indexX, indexY));
    }
}
