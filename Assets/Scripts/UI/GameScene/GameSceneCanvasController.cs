using System;
using Logic.Managers;
using UnityEngine;

namespace UI.GameScene
{
    public class GameSceneCanvasController : MonoBehaviour
    {
        private void Awake()
        {
            AudioManager.Instance.Init();
        }
    }
}
