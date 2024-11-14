using System.Collections;
using System.Collections.Generic;
using DicksonMd.UI;
using UnityEngine;

public class TrackedCity : MonoBehaviour
{
    private VariableWatcherRow _variableWatcherRow;
    private VariableWatcherRow _variableWatcherRow2;
    private VariableWatcher _variableWatcher;

    // Start is called before the first frame update
    void Start()
    {
        _variableWatcher = FindObjectOfType<VariableWatcher>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_variableWatcherRow)
        {
            if (!_variableWatcher) return;
            _variableWatcherRow = FindObjectOfType<VariableWatcher>().Add("CityPosition", "");
            _variableWatcherRow2 = FindObjectOfType<VariableWatcher>().Add("CityScale", "");
        }

        if (_variableWatcherRow) _variableWatcherRow.SetValue(transform.position.ToString("F3"));
        if (_variableWatcherRow2)
        {
            var tr = transform;
            var scaleStr = "";
            do
            {
                scaleStr += transform.localScale.ToString("F3") + "; \n";
                tr = tr.parent;
            } while (tr);

            _variableWatcherRow2.SetValue(scaleStr);
        }
    }
}