using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Logic;
using Logic.Managers;
using NUnit.Framework;
using UI.GameScene.GameView;
using UnityEngine;
using UnityEngine.TestTools;

namespace Editor.UnitTests.GameFlowTests
{
    public class UndoUnitTest
    {
        private GameManager _gameManager;
        private readonly BoardCellPosition _posToCheckUndoAgainst = new BoardCellPosition(3, 2);
        
        
        [UnityTest]
        public IEnumerator Check_Undo_Action_Test()
        {
            yield return null;
            
            yield return null;
            
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

                if (boardCell.BoardCellPosition.x == _posToCheckUndoAgainst.x && boardCell.BoardCellPosition.y == _posToCheckUndoAgainst.y)
                {
                    boardCell
                        .GetType()
                        .GetProperty(nameof(boardCell.CellMarkType))?
                        .SetValue(boardCell, PlayerMark.Crosses);
                }
            }
            
            _gameManager.UndoAction += (undoCellPos) =>
            {
                if (undoCellPos.x == _posToCheckUndoAgainst.x && undoCellPos.y == _posToCheckUndoAgainst.y)
                {
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail();
                }
            };

            var trackedStack = new Stack<BoardCellPosition>();
            trackedStack.Push(_posToCheckUndoAgainst);
            type
                .GetField("_trackedSteps", BindingFlags.Instance | BindingFlags.NonPublic)?
                .SetValue(_gameManager, trackedStack);
            
            _gameManager.UndoRequested();

            var timer = 0.0;
            while (true)
            {
                timer += Time.deltaTime;
                yield return null;
                if (timer > 3)
                {
                    Debug.Log(timer + " seconds have passed with no result");
                    Assert.Fail();
                }
            }
        }
        
    }
}
