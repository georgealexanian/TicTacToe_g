using Logic;
using Logic.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameScene.Windows.SettingsWindow
{
    public class DifficultyToggle : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private Toggle toggle;
        
        private DifficultyLevel _difficultyLevel;
        
        
        public void Initialize(DifficultyLevel difficultyLevel, ToggleGroup toggleGroup)
        {
            _difficultyLevel = difficultyLevel;
            label.text = _difficultyLevel.ToString();
            toggle.isOn = _difficultyLevel == GameManager.Instance.GameDifficulty;
            toggle.group = toggleGroup;
        }


        public void OnValueChanged(bool value)
        {
            if (value)
            {
                GameManager.Instance.GameDifficulty = _difficultyLevel;
            }
        }
    }
}
