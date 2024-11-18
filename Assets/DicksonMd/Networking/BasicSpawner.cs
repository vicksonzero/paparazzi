using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DicksonMd.Networking
{
    public partial class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
    {
        private Logger _logger;
        private NetworkRunner _runner;

        private void Awake()
        {
            _logger = FindObjectOfType<Logger>();
        }

        public async void StartGameAsync(GameMode mode, string roomId)
        {
            _runner = _runner ? _runner : GetComponent<NetworkRunner>();
            if (_runner.State is NetworkRunner.States.Running or NetworkRunner.States.Starting)
            {
                _logger.LogError($"_runner is already created");
            }
            // if (_runner != null)
            // {
            //     
            //     logger.LogError($"_runner is already created");
            // }
            // // Create the Fusion runner and let it know that we will be providing user input
            // _runner = gameObject.AddComponent<NetworkRunner>();
            _runner.ProvideInput = true;

            // Create the NetworkSceneInfo from the current scene
            var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
            var sceneInfo = new NetworkSceneInfo();
            if (scene.IsValid)
            {
                sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
            }

            // Start or join (depends on gamemode) a session with a specific name
            await _runner.StartGame(new StartGameArgs()
            {
                GameMode = mode,
                SessionName = roomId,
                Scene = scene,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });
            _logger.Log($"(StartGame) '{roomId}' Started.");
        }

        // private void OnGUI()
        // {
        //     if (_runner == null)
        //     {
        //         if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
        //         {
        //             StartGame(GameMode.Host);
        //         }
        //
        //         if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
        //         {
        //             StartGame(GameMode.Client);
        //         }
        //     }
        // }


        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
            _logger.Log($"(OnShutdown)");
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
            _logger.Log($"(OnConnectedToServer) {runner.SessionInfo.Name}");
        }

        public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
        {
            _logger.Log($"(OnDisconnectedFromServer) {reason.ToString()}");
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request,
            byte[] token)
        {
            _logger.Log($"(OnConnectRequest)");
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
            _logger.Log($"(OnConnectFailed) {reason.ToString()}");
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
            _logger.Log($"(OnSessionListUpdated)");
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
            _logger.Log($"(OnHostMigration)");
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
            _logger.Log($"(OnSceneLoadDone)");
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
        }

        public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
        {
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key,
            ArraySegment<byte> data)
        {
        }

        public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
        {
        }
    }
}