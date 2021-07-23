using System;
using UnityEngine;

namespace UI.GameScene.Windows
{
    [RequireComponent(typeof(Canvas))]
    public class BaseWindow : MonoBehaviour
    {
        private Canvas _canvas;
        
        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        public void CloseWindow()
        {
            gameObject.SetActive(false);
        }
    }
}
