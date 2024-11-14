using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DicksonMd.Utils
{
    public static class DicksonRandom
    {
        public static int FromWeights(IReadOnlyList<float> normalizedWeights, float? drawnNumber = null)
        {
            var totalWeights = normalizedWeights.Sum();

            var drawnWeight = (drawnNumber ?? Random.value) * totalWeights;

            // Debug.Log($"FromWeights: from({normalizedWeights.Count}) {drawnWeight} / {totalWeights}");
            var remainingWeight = totalWeights;
            for (var i = normalizedWeights.Count - 1; i >= 0; i--)
            {
                remainingWeight -= normalizedWeights[i];
                if (drawnWeight >= remainingWeight) return i;
            }

            return 0;
        }
    }
}