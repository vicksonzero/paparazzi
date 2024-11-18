using TMPro;
using UnityEngine;

namespace DicksonMd.Networking
{
    public class Logger : MonoBehaviour
    {
        public TMP_Text text;


        public void Log(string msg, bool? teeToConsole = null)
        {
            text.text = msg + "\n" + text.text;
            if (teeToConsole is not null && teeToConsole.Value)
            {
                Debug.Log(msg);
            }
        }

        public void LogError(string msg)
        {
            text.text = "ERR: " + msg + "\n" + text.text;
        }
    }
}