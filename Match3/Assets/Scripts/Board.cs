using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class Board : MonoBehaviour
    {
        private Camera mainCamera;
        public int boardWidth;
        public int boardHeight;
        public GameObject backgroundTilePrefab;
        public Gem[] gems;
        public Gem[,] allGems;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Start()
        {
            allGems = new Gem[boardWidth, boardHeight];
            CameraSetup();
            BoardSetup();
        }

        private void CameraSetup()
        {
            var xValue = (boardWidth / 2);
            var yValue = (boardHeight / 2);
            var zValue = -10;
            mainCamera.transform.position = new Vector3(xValue, yValue, zValue);
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

                    var gemToUse = Random.Range(0, gems.Length);
                    SpawnGem(new Vector2Int(x, y), gems[gemToUse]);
                }
            }
        }

        private void SpawnGem(Vector2Int position, Gem gemToSpawn)
        {
            Gem gem = Instantiate(gemToSpawn, new Vector3(position.x, position.y, 0f), Quaternion.identity);
            gem.transform.parent = this.transform;
            gem.name = $"Gem - {position.x}, {position.y}";
            allGems[position.x, position.y] = gem;

            gem.SetupGem(position, this);
        }
    }
}
