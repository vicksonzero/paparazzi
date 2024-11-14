using System.Collections;
using System.Collections.Generic;
using DicksonMd.UI;
using UnityEngine;

public class TrackedCity : MonoBehaviour
{
    private VariableWatcherRow _variableWatcherRow;
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
        }

        if (_variableWatcherRow) _variableWatcherRow.SetValue(transform.position.ToString("F3"));
    }
}