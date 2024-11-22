using System.Collections;
using System.Linq;
using Cinemachine;
using DicksonMd.UI;
using Fusion;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace DicksonMd.Networking
{
    public class NetworkPcPlayer : NetworkBehaviour
    {
        private Logger _logger;
        private NetworkTransform _nt;
        private VariableWatcherRow _variableWatcherRow;
        public float moveSpeed = 0.01f;

        public Camera mainCamera;
        public CinemachineVirtualCamera playerCamera;
        public CinemachineVirtualCamera cameraCamera;
        public CinemachineVirtualCamera cameraCameraZoomed;

        public GameObject eyeModel;

        private void Awake()
        {
            _logger = FindObjectOfType<Logger>();
            _logger.Log($"Player spawned at {transform.position}");
            _nt = GetComponent<NetworkTransform>();
        }

        private void Start()
        {
            if (this.Object.HasInputAuthority)
            {
                playerCamera.gameObject.SetActive(true);
                eyeModel.layer = LayerMask.NameToLayer("LocalPlayer");
            }
        }

        void OnDestroy()
        {
            if (_variableWatcherRow)
            {
                _variableWatcherRow.SetValue("(Player Left)");
                _variableWatcherRow.DestroyAfter(5);
            }
        }


        public override void FixedUpdateNetwork()
        {
            if (GetInput(out NetworkInputData data))
            {
                data.direction.Normalize();
                transform.Translate(moveSpeed * data.direction * Runner.DeltaTime);
            }
        }
    }
}