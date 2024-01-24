using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    private BuildingSO _selectedBuildingSo;
    private Camera _camera;
    private BuildingBase _buildingBase;
    
    [SerializeField] private EventManager EventManager;
    [SerializeField] private Grid Grid;
    [SerializeField] private BuildingPreview BuildingPreview;
    private void OnEnable()
    {
        EventManager.pointerDownOnBuildingCard.AddListener(SelectBuilding); 
        EventManager.mouseUp.AddListener(PointerUp);
    }
    private void OnDisable()
    {
        EventManager.pointerDownOnBuildingCard.RemoveListener(SelectBuilding);
        EventManager.mouseUp.RemoveListener(PointerUp);
    }
    private void Start()
    {
        _camera = Camera.main;
    }
    private void Update()
    {
        if (_selectedBuildingSo != null)
        {
            Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 actualPos = new Vector3(mousePos.x, mousePos.y, 0);
            BuildingPreview.transform.position=actualPos;
            
            CheckIfSuitable();
        }
        else
        {
            ResetIndex();
        }

        if ((Input.GetKeyDown(KeyCode.R) || Input.GetMouseButtonDown(1)))
        {
            BuildingPreview.RotateBuilding();
            Grid.IndexUpdate();
        }
    }
    private void ResetIndex()
    {
        Grid.index = 0;
        BuildingPreview.index = 0;
        BuildingPreview.angleX = 0;
    }

    private void CheckIfSuitable()
    {
        bool check = Grid.CheckIfSuitable(_selectedBuildingSo, BuildingPreview.transform.position);
        if (check)
        {
            BuildingPreview.ChangeColorGreen();
        }
        else
        {
            BuildingPreview.ChangeColorRed();
        }
    }
    private void SelectBuilding(BuildingSO buildingSO)
    {
        _selectedBuildingSo = buildingSO;
        BuildingPreview.gameObject.SetActive(true);
        BuildingPreview.Init(buildingSO);
    }
    private void PointerUp(Vector3 pos)
    {
        if (_selectedBuildingSo != null)
        {
            Grid.TryPlaceBuilding(pos,_selectedBuildingSo);
        }
        _selectedBuildingSo = null;
        BuildingPreview.DestroyPreview();
        BuildingPreview.gameObject.SetActive(false);
    }
}
