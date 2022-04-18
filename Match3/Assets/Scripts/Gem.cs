using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class Gem : MonoBehaviour
    {
        private Camera _mainCamera;
        private Vector2 _firstTouchPosition;
        private Vector2 _finalTouchPosition;
        private float _swipeAngle = 0f;
        private bool _isMousePressed;
        private Gem _otherGem;
        public Board mainBoard;
        public Vector2Int positionIndex;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Vector2.Distance(transform.position, positionIndex) > .01f)
            {
                transform.position = Vector2.Lerp(transform.position, positionIndex, mainBoard.gemSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = new Vector3(positionIndex.x, positionIndex.y, 0f);
                mainBoard.allGems[positionIndex.x, positionIndex.y] = this;
            }

            if (_isMousePressed == true && Input.GetMouseButtonUp(0))
            {
                _isMousePressed = false;
                _finalTouchPosition = _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _mainCamera.transform.position.z)) * -1;
                CalculateAngle();
            }
        }

        public void SetupGem(Vector2Int position, Board board)
        {
            positionIndex = position;
            mainBoard = board;
        }

        private void OnMouseDown()
        {
            _firstTouchPosition = _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _mainCamera.transform.position.z)) * -1;
            _isMousePressed = true;
            Debug.Log($"Pressed = {name} at {_firstTouchPosition}");
        }

        private void CalculateAngle()
        {
            _swipeAngle = Mathf.Atan2((_finalTouchPosition.y - _firstTouchPosition.y), (_finalTouchPosition.x - _firstTouchPosition.x));
            _swipeAngle = _swipeAngle * 180 / Mathf.PI;

            if (Vector3.Distance(_firstTouchPosition, _finalTouchPosition) < 0.5f)
            {
                MoveGemPieces();
            }
            Debug.Log($"SwipeAngle = {_swipeAngle}");
        }

        private void MoveGemPieces()
        {
            if (_swipeAngle < 45 && _swipeAngle > -45 && positionIndex.x < mainBoard.boardWidth - 1)
            {
                _otherGem = mainBoard.allGems[positionIndex.x + 1, positionIndex.y];
                _otherGem.positionIndex.x--;
                positionIndex.x++;
            }
            else if (_swipeAngle > 45 && _swipeAngle <= 135 && positionIndex.y < mainBoard.boardHeight - 1)
            {
                _otherGem = mainBoard.allGems[positionIndex.x, positionIndex.y + 1];
                _otherGem.positionIndex.y--;
                positionIndex.y++;
            }
            else if (_swipeAngle < -45 && _swipeAngle >= -135 && positionIndex.y > 0)
            {
                _otherGem = mainBoard.allGems[positionIndex.x, positionIndex.y - 1];
                _otherGem.positionIndex.y++;
                positionIndex.y--;
            }
            else if (_swipeAngle > 135 || _swipeAngle < -135 && positionIndex.x > 0)
            {
                _otherGem = mainBoard.allGems[positionIndex.x - 1, positionIndex.y];
                _otherGem.positionIndex.x++;
                positionIndex.x--;
            }

            mainBoard.allGems[positionIndex.x, positionIndex.y] = this;
            mainBoard.allGems[_otherGem.positionIndex.x, _otherGem.positionIndex.y] = _otherGem;
        }
    }
}
