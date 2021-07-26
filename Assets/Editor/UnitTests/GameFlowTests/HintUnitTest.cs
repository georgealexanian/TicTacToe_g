using System.Collections;
using System.Linq;
using Logic;
using Logic.Managers;
using NUnit.Framework;
using UI.GameScene.GameView;
using UnityEngine;
using UnityEngine.TestTools;

namespace Editor.UnitTests.GameFlowTests
{
    public class HintUnitTest 
    {
        private GameManager _gameManager;
        private readonly BoardCellPosition _posToCheckAgainst = new BoardCellPosition(2, 5);

        
        [UnityTest]
        public IEnumerator Check_Hint_Action_Test()
        {
            yield return null;
            
            var holderGo = new GameObject();
            _gameManager = holderGo.AddComponent<GameManager>();
            _gameManager.Initialize();
            var type = typeof(GameManager);
            
            for (int i = 0; i < 25; i++)
            {
                var boardCell = holderGo.AddComponent<BoardCell>();
                boardCell
                    .GetType()
                    .GetProperty(nameof(boardCell.BoardCellPosition))?
                    .SetValue(boardCell, new BoardCellPosition(i - (i / 5) * 5 + 1, i / 5 + 1));
                boardCell.GetType()
                    .GetProperty(nameof(boardCell.CellMarkType))?
                    .SetValue(
                        boardCell, 
                        boardCell.BoardCellPosition.x == _posToCheckAgainst.x && boardCell.BoardCellPosition.y == _posToCheckAgainst.y 
                            ? PlayerMark.Unknown 
                            : i % 2 == 0 
                                ? PlayerMark.Crosses 
                                : PlayerMark.Noughts);
            }
            
            _gameManager.HintPositionAction += (hintCellPos) =>
            {
                if (hintCellPos.x == _posToCheckAgainst.x && hintCellPos.y == _posToCheckAgainst.y)
                {
                    // Debug.Log(hintCellPos.x + "       " + hintCellPos.y);
                    Assert.Pass();
                }
                else
                {
                    Assert.Fail();
                }
            };
            
            var boardCells = holderGo.GetComponents<BoardCell>().ToList();
            type
                .GetProperty(nameof(_gameManager.CurrentGameCells))?
                .SetValue(_gameManager, boardCells);
            type
                .GetProperty(nameof(_gameManager.GridSize))?
                .SetValue(_gameManager, 5);
            
            _gameManager.HintRequested(PlayerMark.Noughts, 1);
            
            var timer = 0.0;
            while (true)
            {
                yield return null;
                timer += Time.deltaTime;
                if (timer > 2)
                {
                    Debug.Log(timer + " seconds have passed with no result");
                    Assert.Fail();
                }
            }
        }
    }
}
