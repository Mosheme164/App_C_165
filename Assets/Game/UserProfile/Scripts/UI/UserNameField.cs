using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace UserProfile.UI
{
    public class UserNameField : MonoBehaviour
    {
        [SerializeField] private GameObject _normalState = null;
        [SerializeField] private GameObject _editingState = null;
    
        [Space]
    
        [SerializeField] private Text _nameText = null;

        [SerializeField] private InputField _editNameField = null;

        private void Awake()
        {
            _normalState.SetActive(true);
            _editingState.SetActive(false);
        
            _editNameField.onEndEdit.AddListener(SaveNewName);
            _editNameField.onValueChanged.AddListener(ValidateInput);

            UserProfileStorage.OnChangedUserName += UpdateNameText;

            UpdateNameText(UserProfileStorage.UserName);
        }

        private void ValidateInput(string text)
        {
            text = text.Replace(" ", "");
            
            _editNameField.text = text;
        }

        private void OnDestroy()
        {
            UserProfileStorage.OnChangedUserName -= UpdateNameText;
        }

        private void SaveNewName(string name)
        {
            _normalState.SetActive(true);
            _editingState.SetActive(false);

            UserProfileStorage.UserName = name;
        }

        private void UpdateNameText(string name)
        {
            _nameText.text = name;
        }

        public void StartEditing()
        {
            _normalState.SetActive(false);
            _editingState.SetActive(true);

            _editNameField.text = UserProfileStorage.UserName;
        
            _editNameField.ActivateInputField();
        }
    }
}
