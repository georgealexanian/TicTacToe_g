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

        private List<Cell> _cachedCells = new List<Cell>();


        private void Start()
        {
            CacheGridCells();
        }


        private float CalculateCellSize()
        {
            if (!gameView)
            {
                return 0;
            }
            
            float size = 0;
            var gridRectTr = gameView.GetComponent<RectTransform>();
            size = (gridRectTr.rect.width / GameManager.Instance.GridSize) - gridLayout.spacing.x;
            return size;
        }


        private void CacheGridCells()
        {
            var cellSize = CalculateCellSize();

            if (cellSize == 0)
            {
                return;
            }

            var cellSizeVector2 = new Vector2(cellSize, cellSize);
            cellPrefab.GetComponent<RectTransform>().sizeDelta = cellSizeVector2;
            gridLayout.cellSize = cellSizeVector2;
            
            var count = GameManager.Instance.GridSize * GameManager.Instance.GridSize;
            var cachedCellCount = _cachedCells.Count;
            for (int i = 0; i < count - cachedCellCount; i++)
            {
                var cellGo = Instantiate(cellPrefab, gridLayout.transform);
                _cachedCells.Add(cellGo.GetComponent<Cell>());
            }
        }


        private void SetUpGridView(float cellSize)
        {
            if (_cachedCells == null || _cachedCells.Count == 0)
            {
                return;
            }
            
        }
    }
}
