using System.Collections;
using System.Collections.Generic;
using DicksonMd.Photon;
using Fusion;
using TMPro;
using UnityEngine;

public class LobbyPanel : MonoBehaviour
{
    private Logger _logger;

    [SerializeField]
    private BasicSpawner basicSpawner;

    [SerializeField]
    private TMP_InputField roomIdInput;


    public void StartGameAsHost()
    {
        _logger.Log($"Hosting game '{roomIdInput.text}'...");
        basicSpawner.StartGameAsync(GameMode.Host, roomIdInput.text);
    }

    public void StartGameAsClient()
    {
        _logger.Log($"Joining game '{roomIdInput.text}'...");
        basicSpawner.StartGameAsync(GameMode.Client, roomIdInput.text);
    }


    // Start is called before the first frame update
    void Start()
    {
        _logger = _logger ? _logger : FindObjectOfType<Logger>();
    }

    // Update is called once per frame
    void Update()
    {
    }
}