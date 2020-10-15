using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject inGameUI;

    public GameObject managers;

    public GameObject camera;

    public int countNPC;
    public Bounds boundsRespawn;

    public float TimeToStart = 3f;

    private GameObject[] _screens;

    private MapManager _mapManager;
    private LifeCellsManager _lifeCellsManager;
    private CharactersManager _charactersManager;

    private CameraFollow _cameraFollow;

    // Start is called before the first frame update
    void Start()
    {
        Initialized();

        ShowScreen(ScreenUI.MainMenu);

        void Initialized()
        {
            _screens = new GameObject[]
            {
                inGameUI, mainMenuUI
            };

            _lifeCellsManager = managers.GetComponent<LifeCellsManager>();
            _mapManager = managers.GetComponent<MapManager>();
            _charactersManager = managers.GetComponent<CharactersManager>();
            _cameraFollow = camera.GetComponent<CameraFollow>();
            
            _mapManager.OutOfSpaceHasHappened += OutOfSpaceChange;
        }
    }

    private void OutOfSpaceChange(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            GameOver();
            Destroy(collider.gameObject);
        }
        else if (!collider.gameObject.CompareTag("NPC"))
            return;

        Destroy(collider.gameObject);
    }

    private void ShowScreen(ScreenUI screenUI)
    {
        foreach (var screen in _screens)
        {
            screen.SetActive(false);
        }

        if (screenUI == ScreenUI.MainMenu)
        {
            ShowMainMenu();
        }
        else if (screenUI == ScreenUI.InGame)
        {
            inGameUI.SetActive(true);
        }
    }

    private void ShowMainMenu()
    {
        mainMenuUI.SetActive(true);
        _lifeCellsManager.isActivationCells = false;
        InitializedGame();
    }

    public void StartNewGame()
    {
        _lifeCellsManager.isActivationCells = true;
        ShowScreen(ScreenUI.InGame);
        _charactersManager.EnableAllCharacter();
    }

    public void GameOver()
    {
        _mapManager.RemoveMap();
        _charactersManager.DestroyAllCharacter();

        ShowScreen(ScreenUI.MainMenu);
    }

    private void InitializedGame()
    {
        var positionCells = _mapManager.SpawnCells(countNPC + 1);
        _charactersManager.InitializedCharacters(positionCells);
        _charactersManager.EnableOnlyPlayer();
        _cameraFollow.target = _charactersManager.Player.transform;
    }


    private enum ScreenUI
    {
        InGame,
        MainMenu
    }
}