﻿using System.Collections.Generic;
using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace DicksonMd.Networking
{
    public partial class BasicSpawner
    {
        [SerializeField]
        private NetworkPrefabRef _playerPrefab;

        [SerializeField]
        private NetworkPrefabRef _pcPlayerPrefab;

        private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

        [SerializeField]
        public InputActionAsset moveActionAsset;

        public Joystick joystick;
        public Transform trackingCity;

        [SerializeField]
        private Game.Game game;

        public Transform spawnPoint;

        private void Start()
        {
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            _logger.Log($"OnPlayerJoined {player.PlayerId}", true);
            if (runner.IsServer)
            {
                if (_playerPrefab.IsValid)
                {
                    // Create a unique position for the player
                    Vector3 spawnPosition =
                        new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 0, 0);
                    spawnPosition *= 0.01f;

                    _logger.Log($"spawnPosition = {spawnPosition.ToString("F3")}", true);

                    NetworkObject networkPlayerObject =
                        runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player,
                            (r, obj) =>
                            {
                                _logger.Log($"(onBeforeSpawned) {obj.Name}");
                                obj.GetComponent<PlayerColor>().Color = Random.ColorHSV();
                            });
                    // Keep track of the player avatars for easy access
                    _spawnedCharacters.Add(player, networkPlayerObject);
                }

                if (_pcPlayerPrefab.IsValid)
                {
                    NetworkObject networkPlayerObject =
                        runner.Spawn(_pcPlayerPrefab, spawnPoint.position, spawnPoint.rotation, player,
                            (r, obj) =>
                            {
                                _logger.Log($"(onBeforeSpawned) {obj.Name}");
                                obj.GetComponent<PlayerColor>().Color = Random.ColorHSV();
                            });
                    // Keep track of the player avatars for easy access
                    _spawnedCharacters.Add(player, networkPlayerObject);
                }


                if (runner.IsServer)
                {
                    game.StartGame();
                }
            }
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            _logger.Log($"OnPlayerLeft {player.PlayerId}");
            if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
            {
                runner.Despawn(networkObject);
                _spawnedCharacters.Remove(player);
            }
        }


        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
            var data = new NetworkInputData();
            var inputVector = moveActionAsset["Move"].ReadValue<Vector2>();

            data.direction = joystick && joystick.isDragging
                ? new Vector3(joystick.value.x, 0, joystick.value.y)
                : new Vector3(inputVector.x, 0, inputVector.y);
            // if (.GetKey(KeyCode.W))
            //     data.direction += Vector3.forward;
            //
            // if (Input.GetKey(KeyCode.S))
            //     data.direction += Vector3.back;
            //
            // if (Input.GetKey(KeyCode.A))
            //     data.direction += Vector3.left;
            //
            // if (Input.GetKey(KeyCode.D))
            //     data.direction += Vector3.right;

            input.Set(data);
        }
    }
}