using Logic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameScene.Windows.SettingsWindow
{
    public class DifficultyToggle : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private Toggle toggle;

        private GameStates.DifficultyLevel _difficultyLevel;
        
        
        public void Initialize(GameStates.DifficultyLevel difficultyLevel)
        {
            _difficultyLevel = difficultyLevel;

            label.text = _difficultyLevel.ToString();
        }
    }
}
