using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DicksonMd.Networking
{
    public class LobbyPanel : MonoBehaviour
    {
        private Logger _logger;

        [SerializeField]
        private BasicSpawner basicSpawner;

        [SerializeField]
        private TMP_InputField roomIdInput;

        [SerializeField]
        private Button[] joinRoomButtons;

        public string[] roomNames;

        public void StartGameAsHost()
        {
            var roomName = roomIdInput.text;
            _logger.Log($"Hosting game '{roomName}'...");
            basicSpawner.StartGameAsync(GameMode.Host, roomName);
        }

        public void StartGameAsClient()
        {
            var roomName = roomIdInput.text;
            _logger.Log($"Joining game '{roomName}'...");
            basicSpawner.StartGameAsync(GameMode.Client, roomName);
        }

        public void JoinRoomByIndex(int roomIndex)
        {
            if (roomIndex >= roomNames.Length)
            {
                _logger.LogError($"roomIndex {roomIndex} out of bounds.");
                return;
            }

            var roomName = roomNames[roomIndex];

            _logger.Log($"Joining game '{roomName}' or be the Host...");
            basicSpawner.StartGameAsync(GameMode.AutoHostOrClient, roomName);
        }


        // Start is called before the first frame update
        void Start()
        {
            _logger = _logger ? _logger : FindObjectOfType<Logger>();

            for (var i = 0; i < joinRoomButtons.Length; i++)
            {
                var button = joinRoomButtons[i];

                var text = button.GetComponentInChildren<TMP_Text>();
                if (text)
                {
                    text.text = i < roomNames.Length ? roomNames[i] : "";
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}