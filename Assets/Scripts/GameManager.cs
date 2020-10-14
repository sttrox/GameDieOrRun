using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject inGameUI;

    public GameObject prefabPlayer;
    public GameObject prefabNPC;
    public GameObject managers;

    public GameObject camera;

    public int countNPC;
    public Bounds boundsRespawn;

    public float TimeToStart = 3f;

    private GameObject[] _screens;

    private ManagerMap _managerMap;
    private ManagerLifeCells _managerLifeCells;

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

            _managerLifeCells = managers.GetComponent<ManagerLifeCells>();
            _managerMap = managers.GetComponent<ManagerMap>();

            _cameraFollow = camera.GetComponent<CameraFollow>();
        }
    }

    // Update is called once per frame
    void Update()
    {
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
        _managerLifeCells.isActivationCells = false;
        InitializedGame();
    }

    public void StartNewGame()
    {
        _managerLifeCells.isActivationCells = true;
        ShowScreen(ScreenUI.InGame);
    }

    public void GameOver()
    {
        ShowScreen(ScreenUI.MainMenu);
    }

    private void InitializedGame()
    {
        GameObject player;
        var arrPlayers = InitializedPlayers(countNPC, out player);
        _cameraFollow.target = player.transform;
        _managerMap.RespawnPlayers(arrPlayers);
    }

    private GameObject[] InitializedPlayers(int countNPC, out GameObject playerInstance)
    {
        GameObject[] players = new GameObject[countNPC + 1];
        var indexPlayer = Random.Range(0, countNPC + 1);
        playerInstance = Instantiate(prefabPlayer);
        for (int i = 0; i < players.Length; i++)
        {
            GameObject player = i == indexPlayer ? playerInstance : Instantiate(prefabNPC);

            players[i] = player;
        }

        return players;
    }

    private enum ScreenUI
    {
        InGame,
        MainMenu
    }
}