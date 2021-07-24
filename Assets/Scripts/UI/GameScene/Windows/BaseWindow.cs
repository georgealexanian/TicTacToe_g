using System;
using Logic.Managers;
using UnityEngine;

namespace UI.GameScene.Windows
{
    [RequireComponent(typeof(Canvas))]
    public class BaseWindow<T> : MonoBehaviour
    {
        private Canvas _canvas;
        private event Action<string> WindowCloseCallBack;
        
        
        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        
        public void OpenWindow(Action<string> winCloseCallBack = null)
        {
            gameObject.SetActive(true);
            WindowCloseCallBack = winCloseCallBack;
        }

        
        public void CloseWindow()
        {
            gameObject.SetActive(false);
            WindowCloseCallBack?.Invoke(typeof(T).Name);
            WindowsManager.Instance.WindowClosedCallBack.Invoke(typeof(T).Name);
        }
    }
}
