using System.Collections;
using System.Collections.Generic;
using DicksonMd.UI;
using UnityEngine;

public class TrackedCity : MonoBehaviour
{
    private VariableWatcherRow _variableWatcherRow;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        _variableWatcherRow = _variableWatcherRow
            ? _variableWatcherRow
            : FindObjectOfType<VariableWatcher>().Add("CityPosition", "");

        _variableWatcherRow.SetValue(transform.position.ToString("F3"));
    }
}