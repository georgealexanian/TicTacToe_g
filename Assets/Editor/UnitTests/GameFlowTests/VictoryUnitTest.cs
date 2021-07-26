using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Logic;
using Logic.Managers;
using NUnit.Framework;
using UI.GameScene.GameView;
using UnityEngine;
using UnityEngine.TestTools;
using Debug = UnityEngine.Debug;


namespace Editor.UnitTests.GameFlowTests
{
    public class VictoryUnitTest
    {
        private GameManager _gameManager;
        
        [UnityTest]
        public IEnumerator Check_Victory_Test()
        {
            var holderGo = new GameObject();
            _gameManager = holderGo.AddComponent<GameManager>();
            _gameManager.Initialize();
            var type = typeof(GameManager);

            for (int i = 0; i < 9; i++)
            {
                var boardCell = holderGo.AddComponent<BoardCell>();
                boardCell
                    .GetType()
                    .GetProperty(nameof(boardCell.BoardCellPosition))?
                    .SetValue(boardCell, new BoardCellPosition(i - (i / 3) * 3 + 1, i / 3 + 1));
                Debug.Log(boardCell.BoardCellPosition.x + "      " + boardCell.BoardCellPosition.y);
            }

            var boardCells = holderGo.GetComponents<BoardCell>().ToList();
            type
                .GetProperty(nameof(_gameManager.CurrentGameCells))?
                .SetValue(_gameManager, boardCells);
            type
                .GetProperty(nameof(_gameManager.GridSize))?
                .SetValue(_gameManager, 3);

            List<BoardCellPosition> receivedList = null;
            const int successfulTestCount = 4;
            var testCounter = 0;
            _gameManager.VictoryAction += (victoriousCells) =>
            {
                testCounter++;
                receivedList = victoriousCells;
            };

            SetHorizontalWin(ref boardCells);
            _gameManager.StartCheckingVictory(new BoardCellPosition(0, 0));
            SetVerticalWin(ref boardCells);
            _gameManager.StartCheckingVictory(new BoardCellPosition(0, 0));
            SetFirstDiagonalWin(ref boardCells);
            _gameManager.StartCheckingVictory(new BoardCellPosition(0, 0));
            SetSecondDiagonalWin(ref boardCells);
            _gameManager.StartCheckingVictory(new BoardCellPosition(0, 0));

            var timer = 0.0;
            while (receivedList == null || testCounter < successfulTestCount)
            {
                timer += Time.deltaTime;
                yield return null;
                if (timer > 2)
                {
                    Debug.Log(timer + " seconds have passed with no result");
                    Assert.Fail();
                }
            }
        }


        private void SetHorizontalWin(ref List<BoardCell> boardCells)
        {
            boardCells[0].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[0], PlayerMark.Crosses);
            boardCells[1].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[1], PlayerMark.Crosses);
            boardCells[2].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[2], PlayerMark.Crosses);
            boardCells[3].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[3], PlayerMark.Unknown);
            boardCells[4].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[4], PlayerMark.Noughts);
            boardCells[5].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[5], PlayerMark.Crosses);
            boardCells[6].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[6], PlayerMark.Unknown);
            boardCells[7].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[7], PlayerMark.Noughts);
            boardCells[8].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[8], PlayerMark.Crosses);
        }


        private void SetVerticalWin(ref List<BoardCell> boardCells)
        {
            boardCells[0].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[0], PlayerMark.Crosses);
            boardCells[1].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[1], PlayerMark.Noughts);
            boardCells[2].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[2], PlayerMark.Unknown);
            boardCells[3].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[3], PlayerMark.Crosses);
            boardCells[4].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[4], PlayerMark.Unknown);
            boardCells[5].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[5], PlayerMark.Unknown);
            boardCells[6].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[6], PlayerMark.Crosses);
            boardCells[7].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[7], PlayerMark.Noughts);
            boardCells[8].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[8], PlayerMark.Noughts);
        }
        
        
        private void SetFirstDiagonalWin(ref List<BoardCell> boardCells)
        {
            boardCells[0].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[0], PlayerMark.Crosses);
            boardCells[1].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[1], PlayerMark.Noughts);
            boardCells[2].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[2], PlayerMark.Unknown);
            boardCells[3].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[3], PlayerMark.Unknown);
            boardCells[4].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[4], PlayerMark.Crosses);
            boardCells[5].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[5], PlayerMark.Unknown);
            boardCells[6].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[6], PlayerMark.Crosses);
            boardCells[7].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[7], PlayerMark.Noughts);
            boardCells[8].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[8], PlayerMark.Crosses);
        }
        
        
        private void SetSecondDiagonalWin(ref List<BoardCell> boardCells)
        {
            boardCells[0].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[0], PlayerMark.Unknown);
            boardCells[1].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[1], PlayerMark.Noughts);
            boardCells[2].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[2], PlayerMark.Crosses);
            boardCells[3].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[3], PlayerMark.Unknown);
            boardCells[4].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[4], PlayerMark.Crosses);
            boardCells[5].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[5], PlayerMark.Unknown);
            boardCells[6].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[6], PlayerMark.Crosses);
            boardCells[7].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[7], PlayerMark.Noughts);
            boardCells[8].GetType().GetProperty("CellMarkType")?.SetValue(boardCells[8], PlayerMark.Noughts);
        }
    }
}
