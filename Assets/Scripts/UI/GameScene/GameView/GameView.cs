using System;
using System.Collections.Generic;
using Logic.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameScene.GameView
{
    public class GameView : MonoBehaviour
    {
        [SerializeField] private Transform gameView;
        [SerializeField] private GridLayoutGroup gridLayout;
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private GameObject gridSeparatorPrefab;
        [SerializeField] private Transform separatorHolder;

        private readonly List<BoardCell> _cachedCells = new List<BoardCell>();
        private readonly List<GameObject> _cachedGridSeparators = new List<GameObject>();


        public void Init()
        {
            var gridRectTr = gameView.GetComponent<RectTransform>();
            var cellSize = CalculateCellSize(gridRectTr);
            CacheGridCells(cellSize);
            SetUpGridView(cellSize, gridRectTr.sizeDelta.x);
        }


        private float CalculateCellSize(RectTransform gridRectTr)
        {
            if (!gameView)
            {
                return 0;
            }
            
            float size = 0;
            size = (gridRectTr.rect.width / GameManager.Instance.GridSize) - gridLayout.spacing.x;
            return size;
        }


        private void CacheGridCells(float cellSize)
        {
            if (cellSize == 0)
            {
                return;
            }

            var gameCellCount = GameManager.Instance.GridSize * GameManager.Instance.GridSize;
            var cachedCellCount = _cachedCells.Count;
            for (int i = 0; i < gameCellCount - cachedCellCount; i++)
            {
                var cellGo = Instantiate(cellPrefab, gridLayout.transform);
                _cachedCells.Add(cellGo.GetComponent<BoardCell>());
            }
        }


        private void SetUpGridView(float cellSize, float gridRectSize)
        {
            if (_cachedCells == null || _cachedCells.Count == 0)
            {
                return;
            }

            var cellSizeVector2 = new Vector2(cellSize, cellSize);
            var gameCellCount = GameManager.Instance.GridSize * GameManager.Instance.GridSize;
            for (int i = 0; i < _cachedCells.Count; i++)
            {
                _cachedCells[i].gameObject.SetActive(i < gameCellCount);
                _cachedCells[i].GetComponent<Button>().interactable = true;
                _cachedCells[i].GetComponent<RectTransform>().sizeDelta = cellSizeVector2;
                gridLayout.cellSize = cellSizeVector2;
            }
            
            SetUpGridSeparators(cellSize, gridRectSize);
        }


        private void SetUpGridSeparators(float cellSize, float gridRectSize)
        {
            var separatorCount = (GameManager.Instance.GridSize - 1) * 2;
            var cachedSeparatorCount = _cachedGridSeparators.Count;

            var gridSeparatorRectTr = gridSeparatorPrefab.GetComponent<RectTransform>();
            gridSeparatorRectTr.sizeDelta = new Vector2(gridRectSize, gridSeparatorRectTr.rect.height);
            
            for (int i = 0; i < separatorCount - cachedSeparatorCount; i++)
            {
                _cachedGridSeparators.Add(Instantiate(gridSeparatorPrefab, separatorHolder));
            }

            for (int i = 0; i < _cachedGridSeparators.Count; i++)
            {
                _cachedGridSeparators[i].SetActive(i < separatorCount);
                
                if (i < separatorCount / 2)
                {
                    _cachedGridSeparators[i].transform.localPosition = new Vector3
                    (
                        0,
                        ((cellSize + gridLayout.spacing.y) * (i + 1) + gridLayout.spacing.y * 2) * (-1),
                        0
                    );
                    _cachedGridSeparators[i].transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else if (i < separatorCount && i >= separatorCount / 2)
                {
                    _cachedGridSeparators[i].transform.localPosition = new Vector3
                    (
                        ((cellSize + gridLayout.spacing.y) * (i - separatorCount / 2 + 1) + gridLayout.spacing.y * 2),
                        gridRectSize * (-1),
                        0
                    );
                    _cachedGridSeparators[i].transform.rotation = Quaternion.Euler(0, 0, 90);
                }
            }
        }
    }
}
