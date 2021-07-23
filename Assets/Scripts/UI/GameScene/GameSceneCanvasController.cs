using System;
using Logic.Managers;
using UnityEngine;

namespace UI.GameScene
{
    public class GameSceneCanvasController : MonoBehaviour
    {
        [SerializeField] private GameObject settingsWindow;
        public void SettingsButton()
        {
            settingsWindow.SetActive(true);
        }
    }
}
