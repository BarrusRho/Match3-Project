using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class MatchFinder : MonoBehaviour
    {
        private Board _board;

        private void Awake()
        {
            _board = FindObjectOfType<Board>();
        }

        public void FindAllMatches()
        {
            for (int x = 0; x < _board.boardWidth; x++)
            {
                for (int y = 0; y < _board.boardHeight; y++)
                {
                    Gem currentGem = _board.allGems[x, y];
                    if (currentGem != null)
                    {
                        if (x > 0 && x < _board.boardWidth - 1)
                        {
                            Gem leftGem = _board.allGems[x - 1, y];
                            Gem rightGem = _board.allGems[x + 1, y];
                            if (leftGem != null && rightGem != null)
                            {
                                if (leftGem.gemType == currentGem.gemType && rightGem.gemType == currentGem.gemType)
                                {
                                    currentGem.isGemMatched = true;
                                    leftGem.isGemMatched = true;
                                    rightGem.isGemMatched = true;
                                }
                            }
                        }
                    }
                    if (y > 0 && y < _board.boardHeight - 1)
                    {
                        Gem belowGem = _board.allGems[x, y - 1];
                        Gem aboveGem = _board.allGems[x, y + 1];
                        if (belowGem != null && aboveGem != null)
                        {
                            if (belowGem.gemType == currentGem.gemType && aboveGem.gemType == currentGem.gemType)
                            {
                                currentGem.isGemMatched = true;
                                belowGem.isGemMatched = true;
                                aboveGem.isGemMatched = true;
                            }
                        }
                    }
                }
            }
        }
    }
}
