using TMPro;
using UnityEngine;

namespace DicksonMd.UI
{
    public class VariableWatcherRow : MonoBehaviour
    {
        [SerializeField]
        private string label;

        [SerializeField]
        private TMP_Text labelText;

        [SerializeField]
        private TMP_Text valueText;

        public string Label => label;

        public void Init(string key, string val)
        {
            SetLabel(key);
            SetValue(val);
        }

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(label)) return;
            SetLabel(label);
        }

        public void SetLabel(string msg)
        {
            label = msg;
            labelText.text = label + ":";
            name = label + "Row";
        }

        public void SetValue(string msg)
        {
            valueText.text = msg;
        }

        public void DestroyAfter(float delayInSeconds)
        {
            Destroy(gameObject, delayInSeconds);
        }
    }
}