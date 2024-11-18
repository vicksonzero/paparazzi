using System.Collections;
using System.Linq;
using DicksonMd.UI;
using Fusion;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

namespace DicksonMd.Networking
{
    public class Player : NetworkBehaviour
    {
        private Logger _logger;
        private NetworkTransform _nt;
        private VariableWatcherRow _variableWatcherRow;
        public float moveSpeed = 0.01f;

        public ARTrackedImageManager arTrackedImageManager;

        private void Awake()
        {
            _logger = FindObjectOfType<Logger>();
            _logger.Log($"Player spawned at {transform.position}");
            _nt = GetComponent<NetworkTransform>();
        }

        private void Start()
        {
            if (!arTrackedImageManager) arTrackedImageManager = FindObjectOfType<ARTrackedImageManager>();
            if (!arTrackedImageManager) return;
            
            _logger.Log($"arTrackedImageManager.trackables.count {arTrackedImageManager.trackables.count}", true);
            foreach (var trackable in arTrackedImageManager.trackables)
            {
                transform.SetParent(trackable.transform, false);
                break;
            }

            arTrackedImageManager.trackedImagesChanged += args =>
            {
                var trackable = args.added.FirstOrDefault();

                if (trackable is not null)
                {
                    _logger.Log($"(trackedImagesChanged) {trackable.name}", true);
                    transform.SetParent(trackable.transform, false);
                }
            };

            StartCoroutine(WatchPosition());
        }

        void OnDestroy()
        {
            if (_variableWatcherRow)
            {
                _variableWatcherRow.SetValue("(Player Left)");
                _variableWatcherRow.DestroyAfter(5);
            }
        }

        private IEnumerator WatchPosition()
        {
            if (!_variableWatcherRow)
            {
                _variableWatcherRow = FindObjectOfType<VariableWatcher>().Add($"Player{_nt.Object.Id.Raw}Pos", "");
            }

            while (true)
            {
                _variableWatcherRow.SetValue(transform.position.ToString("F3"));
                yield return new WaitForSeconds(0.25f);
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