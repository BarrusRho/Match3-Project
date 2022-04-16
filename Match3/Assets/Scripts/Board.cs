using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class Board : MonoBehaviour
    {
        public int boardWidth;
        public int boardHeight;
        public Camera mainCamera;
        public GameObject backgroundTilePrefab;

        private void Start()
        {
            CameraSetup();
            BoardSetup();
        }

        private void CameraSetup()
        {
            var cameraOffset = -10;
            mainCamera.transform.position = new Vector3((boardWidth / 2), (boardHeight / 2), cameraOffset);
        }

        private void BoardSetup()
        {
            for (int x = 0; x < boardWidth; x++)
            {
                for (int y = 0; y < boardHeight; y++)
                {
                    var position = new Vector2(x, y);
                    GameObject backgroundTile = Instantiate(backgroundTilePrefab, position, Quaternion.identity);
                    backgroundTile.transform.parent = this.transform;
                    backgroundTile.name = $"BackgroundTile - {x}, {y}";
                }
            }
        }
    }
}
