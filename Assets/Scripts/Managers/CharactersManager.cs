using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersManager : MonoBehaviour
{
    public GameObject containerCharacters;

    public GameObject prefabPlayer;
    public GameObject prefabNPC;

    private GameObject[] _players;

    private GameObject _playerInstance;

    public GameObject Player => _playerInstance;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void InitializedCharacters(Vector3[] positions)
    {
        DestroyAllCharacter();

        _players = new GameObject[positions.Length];
        var indexPlayer = Random.Range(0, positions.Length);
        _playerInstance = Instantiate(prefabPlayer, positions[indexPlayer], Quaternion.identity);

        for (int i = 0; i < positions.Length; i++)
        {
            GameObject player = i == indexPlayer
                ? _playerInstance
                : Instantiate(prefabNPC, positions[i], Quaternion.identity);
            player.transform.parent = containerCharacters.transform;
            _players[i] = player;
        }
    }

    public void DestroyAllCharacter()
    {
        if (_players == null) return;
        for (var i = 0; i < _players.Length; i++)
        {
            Destroy(_players[i]);
        }

        Destroy(_playerInstance);
    }

    public void EnableOnlyPlayer()
    {
        for (var i = 0; i < _players.Length; i++)
        {
            _players[i].SetActive(_players[i].CompareTag("Player"));
        }
    }

    public void EnableAllCharacter()
    {
        for (var i = 0; i < _players.Length; i++)
        {
            _players[i].SetActive(true);
        }
    }
}