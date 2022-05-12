using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class Board : MonoBehaviour
    {
        private Camera _mainCamera;
        [HideInInspector] public MatchFinder matchFinder;
        public int boardWidth;
        public int boardHeight;
        public GameObject backgroundTilePrefab;
        public Gem[] gems;
        public Gem[,] allGems;
        public float gemSpeed;

        private void Awake()
        {
            _mainCamera = Camera.main;
            matchFinder = FindObjectOfType<MatchFinder>();
        }

        private void Start()
        {
            allGems = new Gem[boardWidth, boardHeight];
            CameraSetup();
            BoardSetup();
        }

        private void Update()
        {
            matchFinder.FindAllMatches();
        }

        private void CameraSetup()
        {
            var xValue = (boardWidth / 2);
            var yValue = (boardHeight / 2);
            var zValue = -10;
            _mainCamera.transform.position = new Vector3(xValue, yValue, zValue);
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
                    backgroundTile.name = $"BackgroundTile = {x}, {y}";

                    var gemToUse = Random.Range(0, gems.Length);
                    var iterations = 0;
                    while (IsMatched(new Vector2Int(x, y), gems[gemToUse]) == true && iterations < 100)
                    {
                        gemToUse = Random.Range(0, gems.Length);
                        iterations++;
                    }
                    
                    SpawnGem(new Vector2Int(x, y), gems[gemToUse]);
                }
            }
        }

        private void SpawnGem(Vector2Int position, Gem gemToSpawn)
        {
            Gem gem = Instantiate(gemToSpawn, new Vector3(position.x, position.y, 0f), Quaternion.identity);
            gem.transform.parent = this.transform;
            gem.name = $"Gem = {position.x}, {position.y}";
            allGems[position.x, position.y] = gem;

            gem.SetupGem(position, this);
        }

        private bool IsMatched(Vector2Int positionToCheck, Gem gemToCheck)
        {
            if (positionToCheck.x > 1)
            {
                if (allGems[positionToCheck.x - 1, positionToCheck.y].gemType == gemToCheck.gemType && allGems[positionToCheck.x - 2, positionToCheck.y].gemType == gemToCheck.gemType)
                {
                    return true;
                }
            }
            if (positionToCheck.y > 1)
            {
                if (allGems[positionToCheck.x, positionToCheck.y - 1].gemType == gemToCheck.gemType && allGems[positionToCheck.x, positionToCheck.y - 2].gemType == gemToCheck.gemType)
                {
                    return true;
                }
            }
            return false;
        }

        private void DestroyMatchedGemAt(Vector2Int position)
        {
            if (allGems[position.x, position.y] != null)
            {
                if (allGems[position.x, position.y].isGemMatched == true)
                {
                    Destroy(allGems[position.x, position.y].gameObject);
                    allGems[position.x, position.y] = null;
                }
            }
        }

        public void DestroyMatches()
        {
            for (int i = 0; i < matchFinder.currentMatches.Count; i++)
            {
                if (matchFinder.currentMatches[i] != null)
                {
                    DestroyMatchedGemAt(matchFinder.currentMatches[i].positionIndex);
                }
            }
        }
    }
}
