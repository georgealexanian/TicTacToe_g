using System;
using System.Collections.Generic;
using Logic;
using Logic.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.GameScene.Windows.SettingsWindow
{
    public class SettingsWindow : BaseWindow
    {
        [SerializeField] private TextMeshProUGUI gameSize;
        [SerializeField] private Slider gameSizeSlider;
        [SerializeField] private TMP_Dropdown opponentTypeDropDown;
        [SerializeField] private ToggleGroup toggleGroup;
        [SerializeField] private GameObject togglePrefab;
        

        private void OnEnable()
        {
            opponentTypeDropDown.onValueChanged.AddListener(delegate
            {
                OnDropdownValueChanged(opponentTypeDropDown);
            });
            
            SetUpGameSizeInfo();
            SetUpOpponentTypeDropdown();
        }


        private void OnDisable()
        {
            opponentTypeDropDown.onValueChanged.RemoveAllListeners();
        }
        

        private void SetUpGameSizeInfo()
        {
            gameSize.text = GameManager.Instance.GridSize.ToString();
            gameSizeSlider.value = GameManager.Instance.GridSize;
        }


        private void SetUpOpponentTypeDropdown()
        {
            opponentTypeDropDown.ClearOptions();
            var options = new List<TMP_Dropdown.OptionData>();

            foreach (var opponentType in Enum.GetValues(typeof(GameStates.OpponentType)))
            {
                if (opponentType.ToString() != "Unknown")
                {
                    var option = new TMP_Dropdown.OptionData();
                    option.text = opponentType.ToString();
                    options.Add(option);
                }
            }
            
            opponentTypeDropDown.AddOptions(options);

            SetOpponentTypeDropdownValue();
        }


        private void SetOpponentTypeDropdownValue()
        {
            var optionIndex = opponentTypeDropDown.options
                .FindIndex(x => x.text == GameManager.Instance.OpponentType.ToString());
            opponentTypeDropDown.value = optionIndex;
        }


        public void GameSizeSliderValueChanged()
        {
            GameManager.Instance.GridSize = (int) gameSizeSlider.value;
            SetUpGameSizeInfo();
        }


        private void OnDropdownValueChanged(TMP_Dropdown dropdown)
        {
            var optionTextValue = dropdown.options[dropdown.value].text;
            Enum.TryParse(optionTextValue, out GameStates.OpponentType opponentType);
            GameManager.Instance.OpponentType = opponentType;
        }
    }
}
