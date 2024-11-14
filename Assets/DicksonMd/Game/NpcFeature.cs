using System;
using System.Collections.Generic;
using System.Linq;
using DicksonMd.Utils;
using EditorCools;
using UnityEngine;
using UnityEngine.Serialization;

namespace DicksonMd.Game
{
    public class NpcFeature : MonoBehaviour
    {
        // ReSharper disable once UnusedMember.Global
        [Button()]
        public void CleanUpModelList() => CleanUpModelListPrivate();

        [Range(0, 20)]
        public int featureIndex = 0;

        public List<Transform> featureModels = new();
        public List<float> featureIndexWeights = new List<float>();

        public void RandomizeModel(bool activateModel = true)
        {
            var normalizedWeights = featureModels
                .Select((_, i) => i >= featureIndexWeights.Count ? 1 : featureIndexWeights[i])
                .ToList();

            featureIndex = DicksonRandom.FromWeights(normalizedWeights);
            // Debug.Log($"WeightedRandom drawn '{name}' model {bodyIndex}/{models.Count}");

            if (activateModel) ActivateModel();
        }

        private void OnValidate()
        {
            if (featureIndex > featureModels.Count - 1) featureIndex = featureModels.Count - 1;

            ActivateModel();
        }

        public void CleanUpModelListPrivate()
        {
            var modelSets = featureModels.Select((model, i) => new
                {
                    model,
                    weight = i < featureIndexWeights.Count ? featureIndexWeights[i] : 1,
                })
                .Where(x => x.model != null)
                .Distinct()
                .ToList();
            featureModels = modelSets
                .Select(x => x.model)
                .ToList();
            featureIndexWeights = modelSets
                .Select(x => x.weight)
                .ToList();
        }

        public void ActivateModel()
        {
            if (featureIndex >= featureModels.Count)
                throw new IndexOutOfRangeException($"Cannot activate '{name}' model at [{featureIndex}]");
            for (var i = 0; i < featureModels.Count; i++)
            {
                var model = featureModels[i];
                model.gameObject.SetActive(featureIndex == i);
            }
        }
    }
}