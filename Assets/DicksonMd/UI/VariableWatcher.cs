using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace DicksonMd.UI
{
    public class VariableWatcher : MonoBehaviour
    {
        [SerializeField]
        private VariableWatcherRow variableWatcherRowPrefab;

        public IEnumerable<VariableWatcherRow> ByKey(string key)
        {
            return GetComponentsInChildren<VariableWatcherRow>()
                .Where(x => x.Label == key);
        }

        public VariableWatcherRow Add(string key, string value)
        {
            var row = Instantiate(variableWatcherRowPrefab, transform);
            row.Init(key, value);
            return row;
        }
    }
}