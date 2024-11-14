using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DicksonMd.Game;
using DicksonMd.Utils;
using UnityEngine;

public class Npc : MonoBehaviour
{
    public static Dictionary<string, int> fingerprints = new();
    public static int countRerolls = 0;
    public static int globalNpcCount = 0;
    public int npcId;

    public List<NpcFeature> features = new();

    private void Awake()
    {
        npcId = ++globalNpcCount;

        features = GetComponentsInChildren<NpcFeature>().ToList();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public List<int> GetFeatureList()
    {
        features.Sort((a, b) => string.Compare(a.name, b.name, StringComparison.InvariantCulture));
        return features.Select(x => x.featureIndex).ToList();
    }

    public string GetFingerprint() => string.Join(", ", GetFeatureList());


    public void RandomizeModels()
    {
        string fingerprint = "";
        var hasFingerprint = false;
        const int retries = 100;
        for (var i = 0; i < retries; i++)
        {
            foreach (var npcFeature in features)
            {
                npcFeature.RandomizeModel(false);
            }

            fingerprint = GetFingerprint();

            if (!fingerprints.ContainsKey(fingerprint))
            {
                fingerprints[fingerprint] = 1;
                hasFingerprint = true;
                break;
            }

            // Debug.Log($"Duplicate fingerprint found: {fingerprint}. Generating a new one");
            countRerolls++;
        }

        ActivateModels();

        if (!hasFingerprint)
        {
            // Debug.LogWarning($"fingerprint '{fingerprint}' is not gonna be unique after {retries} retries.");
        }
    }


    void ActivateModels()
    {
        if (Application.isPlaying)
        {
            name = $"Npc-{npcId} [{GetFingerprint()}]";
        }

        foreach (var npcFeature in features)
        {
            npcFeature.ActivateModel();
        }
    }
}