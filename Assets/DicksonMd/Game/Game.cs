using System.Linq;
using System.Xml.Schema;
using DicksonMd.Extensions;
using UnityEngine;

namespace DicksonMd.Game
{
    public class Game : MonoBehaviour
    {
        public TrackedCity city;
        public NpcSpawner npcSpawner;

        // Start is called before the first frame update
        void Start()
        {
            city = FindObjectOfType<TrackedCity>();
            if (city) StartGame(city);

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

        public void StartGame(TrackedCity c)
        {
            city = c;
            transform.SetParent(city.transform, false);
            npcSpawner.SpawnNpcs();
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}