using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private EventManager EventManager;
    [SerializeField] private GameManager GameManager;
    [SerializeField] private UIManager UIManager;
    [SerializeField] private Grid Grid;
    
    private void OnEnable()
    {
        EventManager.saveGame.AddListener(SaveGame);
        EventManager.loadGame.AddListener(LoadGame);
    }

    private void OnDisable()
    {
        EventManager.saveGame.RemoveListener(SaveGame);
        EventManager.loadGame.RemoveListener(LoadGame);
    }

    private void Start()
    {
        LoadGame();
    }
    private void SaveGame()
    {
        GameManager.Save();
        Grid.SaveGrid();
    }

    private void LoadGame()
    {
        if (PlayerPrefs.HasKey("Gold"))
        {
            UIManager.ReloadUI();
            GameManager.Load();
            Grid.LoadGrid();
        }
        else
        {
            Grid.NewGame();
            GameManager.Restart(10,10);
        }
    }
}
