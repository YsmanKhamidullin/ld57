using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.VisualNovel
{
    public class Dialogue : MonoBehaviour
    {
        [TextArea]
        public string Text;

        [Range(0f, 1f)]
        public float CharactersAlpha = 1f;

        [field: SerializeField]
        public List<DialogueCharacter> VisibleCharacters { get; private set; }

        [SerializeField]
        private List<Image> _charactersTemplate;

        [SerializeField]
        private Color _nameColor = new Color(215, 215, 215, 1f);

        [SerializeField]
        private Color _textColor = new Color(215, 215, 215, 1f);

        [SerializeField]
        private TextMeshProUGUI _nameLabel;

        [SerializeField]
        private SlowRevealText _text;

        [SerializeField]
        private Image _backgroundImage;

        [SerializeField]
        private bool _isNameHidden;

        private void OnValidate()
        {
            if (Application.isPlaying)
                return;
            UpdateAll();
        }

        [Button]
        public void UpdateAll()
        {
            UpdateCharactersName();
            UpdateNameColor();
            UpdateTextColor();
            UpdateCharactersImage();
            if (Application.isPlaying)
            {
                return;
            }

            _text.SetText(Text, Text.Length);
        }

        private void UpdateTextColor()
        {
            _text.SetColor(_textColor);
        }

        private void UpdateNameColor()
        {
            _nameLabel.color = _nameColor;
        }

        private void UpdateCharactersName()
        {
            var firstChar = VisibleCharacters.FirstOrDefault(c => c.IsActiveTalk);
            if (firstChar != default)
            {
                _nameLabel.gameObject.SetActive(true);
                if (_isNameHidden)
                {
                    _nameLabel.text = "???";
                }
                else
                {
                    _nameLabel.text = firstChar.Character.Name;
                }
            }
            else
            {
                _nameLabel.gameObject.SetActive(false);
            }
        }

        private void UpdateCharactersImage()
        {
            foreach (var c in _charactersTemplate)
            {
                c.gameObject.SetActive(false);
            }

            if (Application.isPlaying)
            {
                return;
            }

            for (var i = 0; i < VisibleCharacters.Count; i++)
            {
                var c = VisibleCharacters[i];
                var character = c.Character;
                if (character.FullSprite == null)
                {
                    continue;
                }

                var charImage = _charactersTemplate[i];
                charImage.gameObject.SetActive(c.IsVisible);
                Color activeColor = c.IsActiveTalk ? Color.white : new Color(200f / 255f, 200f / 255f, 200f / 255f, 1f);
                activeColor = activeColor.Alpha(CharactersAlpha);
                charImage.enabled = true;
                charImage.sprite = character.FullSprite;
                charImage.color = activeColor;
            }
        }

        public async UniTask<string> Show()
        {
            UpdateAll();
            gameObject.SetActive(true);
            var resultText = await _text.Show();
            await UniTask.WaitForSeconds(0.5f);
            return resultText;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetVisibleCharacters(List<DialogueCharacter> characters)
        {
            VisibleCharacters = characters;
        }

        public void SetBackgroundAlpha(float currentAlpha)
        {
            _backgroundImage.Alpha(currentAlpha);
        }
    }

    [Serializable]
    public class DialogueCharacter
    {
        [CharacterSelector]
        public Character Character;

        public bool IsActiveTalk;
        public bool IsVisible = true;
    }
}