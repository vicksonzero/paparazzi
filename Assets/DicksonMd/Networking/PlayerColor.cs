using Fusion;
using UnityEngine;

namespace DicksonMd.Networking
{
    public class PlayerColor : MonoBehaviour
    {
        [Networked]
        public Color Color
        {
            get => color;
            set
            {
                color = value;
                if (renderer) renderer.material.color = color;
            }
        }

        public MeshRenderer renderer;

        [SerializeField]
        private Color color = Color.white;


        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}