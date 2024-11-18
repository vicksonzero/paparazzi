using System;
using System.Linq;
using System.Xml.Schema;
using DicksonMd.Extensions;
using DicksonMd.UI;
using EditorCools;
using Fusion;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.XR.ARFoundation;
using Logger = DicksonMd.Networking.Logger;

namespace DicksonMd.Game
{
    public class Game : NetworkBehaviour
    {
        public TrackedCity city;
        public NpcSpawner npcSpawner;

        public ARTrackedImageManager arTrackedImageManager;

        [SerializeField]
        public Logger logger;
        private VariableWatcherRow _variableWatcherRow;
        private VariableWatcherRow _variableWatcherRow2;
        [SerializeField]
        private VariableWatcher variableWatcher;

        // Start is called before the first frame update
        void Start()
        {
            city = FindObjectOfType<TrackedCity>();
            if (city) SetCity(city);

            StartTrackingAr();
        }

        public void SetCity(TrackedCity c)
        {
            if (!c) throw new Exception($"{nameof(TrackedCity)} expected!@");
            Debug.Log($"StartGame!!");
            city = c;
        }

        [Button]
        public void StartGame()
        {
            logger.Log($"Game.StartGame!!");
            
            transform.SetParent(city.transform, false);
            npcSpawner.SpawnNpcs();
            
            
            var fingerprints = npcSpawner.spawnParent.GetComponentsInChildren<Npc>()
                .Select(x => x.GetFingerprint())
                .ToList();
            Debug.Log($"Fingerprints: \n{string.Join("\n", fingerprints.Take(10))}");

            var duplicates = fingerprints
                .CountDuplicates()
                .Where(x => x.Value > 1)
                .ToList();

            var str = string.Join("\n",
                duplicates
                    .Select(x => $"{x.Value:00}x [{x.Key}]")
                    .OrderByDescending(x => x)
            );
            Debug.Log(
                duplicates.Count > 0
                    ? $"Duplicate NPCs created: \n{str}"
                    : $"NO Duplicate NPCs created after '{Npc.countRerolls}' rerolls");
        }

        public void StartTrackingAr()
        {
            if (!arTrackedImageManager)
            {
                Debug.LogWarning($"arTrackedImageManager not found! Skipping AR.");
                return;
            }
            if (logger)
                logger.Log($"arTrackedImageManager.trackables.count {arTrackedImageManager.trackables.count}", true);
            foreach (var trackable in arTrackedImageManager.trackables)
            {
                Debug.Log("First Trackable found!");
                if (!city) SetCity(trackable.GetComponent<TrackedCity>());
                break;
            }

            arTrackedImageManager.trackedImagesChanged += args =>
            {
                var trackable = args.added.FirstOrDefault();

                if (trackable is not null)
                {
                    if (logger) logger.Log($"(trackedImagesChanged) {trackable.name}", true);
                    if (!city) SetCity(trackable.GetComponent<TrackedCity>());
                    transform.SetParent(trackable.transform, false);
                }
            };
        }

        // Update is called once per frame
        void Update()
        {
            if (!_variableWatcherRow)
            {
                if (!variableWatcher) return;
                _variableWatcherRow = FindObjectOfType<VariableWatcher>().Add("GamePosition", "");
            }

            if (_variableWatcherRow) _variableWatcherRow.SetValue(transform.localPosition.ToString("F3"));
        }
    }
}