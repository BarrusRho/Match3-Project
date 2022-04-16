using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class Gem : MonoBehaviour
    {
        public Board mainBoard;
        public Vector2Int positionIndex;

        public void SetupGem(Vector2Int position, Board board)
        {
            positionIndex = position;
            mainBoard = board;
        }
    }
}
