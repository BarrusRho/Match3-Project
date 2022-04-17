using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class Gem : MonoBehaviour
    {
        public Board mainBoard;
        public Vector2Int positionIndex;

        private Camera _mainCamera;
        private Vector2 _firstTouchPosition;
        private Vector2 _finalTouchPosition;
        private float _swipeAngle = 0f;
        private bool _isMousePressed;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
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
            Debug.Log($"Pressed - {name} at {_firstTouchPosition}");
        }

        private void CalculateAngle()
        {
            _swipeAngle = Mathf.Atan2((_finalTouchPosition.y - _firstTouchPosition.y), (_finalTouchPosition.x - _firstTouchPosition.x));
            _swipeAngle = _swipeAngle * 180 / Mathf.PI;

            if (Vector3.Distance(_firstTouchPosition, _finalTouchPosition) < 0.5f)
            {
                MoveGemPieces();
            }
            Debug.Log($"SwipeAngle - {_swipeAngle}");
        }

        private void MoveGemPieces()
        {

        }
    }
}
