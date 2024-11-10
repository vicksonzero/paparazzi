using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;

namespace DicksonMd.Photon
{
    public partial class BasicSpawner
    {
        [SerializeField]
        private NetworkPrefabRef _playerPrefab;

        private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

        [SerializeField]
        public InputActionAsset moveActionAsset;

        public Joystick joystick;
        public Transform trackingCity;

        private void Start()
        {
        }

        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            _logger.Log($"OnPlayerJoined {player.PlayerId}", true);
            if (runner.IsServer)
            {
                // Create a unique position for the player
                Vector3 spawnPosition =
                    new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * 3, 0, 0);
                spawnPosition *= 0.01f;
                
                _logger.Log($"spawnPosition = {spawnPosition.ToString("F3")}", true);
                
                NetworkObject networkPlayerObject =
                    runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
                // Keep track of the player avatars for easy access
                _spawnedCharacters.Add(player, networkPlayerObject);

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

            data.direction = joystick.isDragging
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