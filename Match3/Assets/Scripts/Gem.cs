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

        public enum GemType { blue, green, red, yellow, purple }
        public GemType gemType;
        public bool isGemMatched;
        [HideInInspector] public Board board;
        [HideInInspector] public Vector2Int positionIndex;
        [HideInInspector] public Vector2Int previousPosition;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Vector2.Distance(transform.position, positionIndex) > .01f)
            {
                transform.position = Vector2.Lerp(transform.position, positionIndex, board.gemSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = new Vector3(positionIndex.x, positionIndex.y, 0f);
                board.allGems[positionIndex.x, positionIndex.y] = this;
            }

            if (_isMousePressed == true && Input.GetMouseButtonUp(0))
            {
                _isMousePressed = false;
                _finalTouchPosition = _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _mainCamera.transform.position.z)) * -1;
                CalculateAngle();
            }
        }

        public void SetupGem(Vector2Int position, Board gameBoard)
        {
            positionIndex = position;
            board = gameBoard;
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
            previousPosition = positionIndex;

            if (_swipeAngle < 45 && _swipeAngle > -45 && positionIndex.x < board.boardWidth - 1)
            {
                _otherGem = board.allGems[positionIndex.x + 1, positionIndex.y];
                _otherGem.positionIndex.x--;
                positionIndex.x++;
            }
            else if (_swipeAngle > 45 && _swipeAngle <= 135 && positionIndex.y < board.boardHeight - 1)
            {
                _otherGem = board.allGems[positionIndex.x, positionIndex.y + 1];
                _otherGem.positionIndex.y--;
                positionIndex.y++;
            }
            else if (_swipeAngle < -45 && _swipeAngle >= -135 && positionIndex.y > 0)
            {
                _otherGem = board.allGems[positionIndex.x, positionIndex.y - 1];
                _otherGem.positionIndex.y++;
                positionIndex.y--;
            }
            else if (_swipeAngle > 135 || _swipeAngle < -135 && positionIndex.x > 0)
            {
                _otherGem = board.allGems[positionIndex.x - 1, positionIndex.y];
                _otherGem.positionIndex.x++;
                positionIndex.x--;
            }

            board.allGems[positionIndex.x, positionIndex.y] = this;
            board.allGems[_otherGem.positionIndex.x, _otherGem.positionIndex.y] = _otherGem;

            StartCoroutine(CheckMovementCoroutine());
        }

        public IEnumerator CheckMovementCoroutine()
        {
            yield return new WaitForSeconds(0.5f);
            board.matchFinder.FindAllMatches();

            if (_otherGem != null)
            {
                if (isGemMatched == false && _otherGem.isGemMatched == false)
                {
                    _otherGem.positionIndex = positionIndex;
                    positionIndex = previousPosition;

                    board.allGems[positionIndex.x, positionIndex.y] = this;
                    board.allGems[_otherGem.positionIndex.x, _otherGem.positionIndex.y] = _otherGem;
                }
            }
        }
    }
}
