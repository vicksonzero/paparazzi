using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace DicksonMd.Utils
{
    public class BackButtonLeave : MonoBehaviour
    {
        public InputActionReference exitAction;
        // public KeyCode[] keyCodes =
        // {
        //     KeyCode.Escape,
        //     KeyCode.Backspace,
        // };

        public enum BackTo
        {
            ExitGame,
            nextScene
        };

        public BackTo onPressBackButton;
        public string sceneName;


        // Use this for initialization
        void Start()
        {
            exitAction.action.performed += ctx =>
            {
                Debug.Log("exitAction.action.performed");
                if (onPressBackButton == BackTo.ExitGame)
                {
                    Application.Quit();
                }
                else
                {
                    SceneManager.LoadScene(sceneName);
                }
            };
        }

        // Update is called once per frame
        void Update()
        {
//             var keyCodeIterator = keyCodes.AsEnumerable();
//
// #if (UNITY_IOS || UNITY_ANDROID)
//             keyCodeIterator = keyCodeIterator.Concat(new[] { KeyCode.Escape });
// #endif
//
//             foreach (var keycode in keyCodeIterator)
//             {
//                 if (Input.GetKeyDown(keycode))
//                 {
//                     if (onPressBackButton == BackTo.ExitGame)
//                     {
//                         Application.Quit();
//                     }
//                     else
//                     {
//                         SceneManager.LoadScene(sceneName);
//                     }
//                 }
//             }
        }
    }
}