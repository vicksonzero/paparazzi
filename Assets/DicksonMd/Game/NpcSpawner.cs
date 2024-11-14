using System.Collections.Generic;
using System.Linq;
using DicksonMd.Extensions;
using DicksonMd.Utils;
using EditorCools;
using Fusion;
using JetBrains.Annotations;
using UnityEngine;

namespace DicksonMd.Game
{
    public class NpcSpawner : MonoBehaviour
    {
        public Game game;
        public int spawnCount = 20;
        public int spawnFailedCount = 0;

        public GameObject npcPrefab;

        public List<BoxCollider> spawnBoxes = new();

        [CanBeNull]
        public List<float> spawnBoxWeights = null;

        public Transform spawnParent;
        public LayerMask avoidLayers;
        public GameObject debugCapsulePrefab;

        // Start is called before the first frame update
        void Start()
        {
        }

        private void UpdateSpawnBoxesWeightCache()
        {
            spawnBoxWeights = spawnBoxes
                .Select(x => x.bounds.Area())
                .ToList();
        }


        public void SpawnNpcs(bool initCache = true)
        {
            if (initCache)
            {
                GetSideWalksFromCityPrefab();
                UpdateSpawnBoxesWeightCache();
            }

            for (int i = 0; i < spawnCount; i++)
            {
                SpawnNpc();
            }
        }

        [Button("Update SideWalks SpawnBox")]
        public void GetSideWalksFromCityPrefab(bool includeInactive = false)
        {
            Debug.Log($"GetSideWalksFromCityPrefab");
            spawnBoxes.AddRange(game.city.GetComponentsInChildren<BoxCollider>(includeInactive)
                .Where(x => x.gameObject.layer == LayerMask.NameToLayer("SideWalks")));

            spawnBoxes = spawnBoxes.Distinct().ToList();
        }

        private void SpawnNpc()
        {
            var npc = Instantiate(
                npcPrefab,
                Vector3.zero,
                Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
            npc.gameObject.SetActive(false);
            if (spawnParent) npc.transform.SetParent(spawnParent);

            var npcCapsule = npc.GetComponent<CapsuleCollider>();
            if (npcCapsule)
            {
                if (spawnBoxWeights is null) UpdateSpawnBoxesWeightCache();

                const int retries = 100;
                var isSuccess = false;
                for (var i = 0; i < retries; i++)
                {
                    var spawnBoxIndex = DicksonRandom.FromWeights(spawnBoxWeights);
                    var spawnBox = spawnBoxes[spawnBoxIndex];

                    var pos = spawnBox.bounds.RandomPointInside();

                    pos.y = spawnBox.bounds.min.y +
                            npcCapsule.center.y;
                    var hasCollision = Physics.CheckCapsule(
                        npcCapsule.center + Vector3.up * (npcCapsule.height / 2),
                        npcCapsule.center + Vector3.down * (npcCapsule.height / 2),
                        npcCapsule.radius,
                        avoidLayers,
                        QueryTriggerInteraction.Collide);

                    if (!hasCollision)
                    {
                        pos.y = spawnBox.bounds.min.y;
                        npc.transform.position = pos;
                        npc.gameObject.SetActive(true);
                        isSuccess = true;
                        break;
                    }

                    Debug.Log($"SpawnNpc({Npc.globalNpcCount}): Collision({i}) at {pos}, retrying...");
                    if (debugCapsulePrefab) Instantiate(debugCapsulePrefab, pos, Quaternion.identity);
                }

                if (!isSuccess)
                {
                    Debug.LogWarning($"SpawnNpc({Npc.globalNpcCount}): Failed after {retries} tries");
                    spawnFailedCount++;
                    Destroy(npc.gameObject);
                    return;
                }
            }

            npc.GetComponent<Npc>().RandomizeModels();
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}