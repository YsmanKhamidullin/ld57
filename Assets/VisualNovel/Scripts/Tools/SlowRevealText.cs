using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Game.Core.VisualNovel
{
    public class SlowRevealText : MonoBehaviour
    {
        public bool IsWriting;

        [SerializeField]
        private TextMeshProUGUI _textLabel;

        private float _betweenCharactersTime = 0.02f;


        public void Pause()
        {
            IsWriting = false;
        }

        public void Resume()
        {
            IsWriting = true;
        }

        public async UniTask<string> Show()
        {
            _textLabel.maxVisibleCharacters = 0;
            _betweenCharactersTime = 0.02f;
            if (IsWriting)
            {
                return _textLabel.text;
            }

            IsWriting = true;
            _textLabel.ForceMeshUpdate();

            var totalVisibleCharacters = _textLabel.textInfo.characterCount;
            while (IsWriting)
            {
                _textLabel.maxVisibleCharacters++;
                IsWriting = _textLabel.maxVisibleCharacters <= totalVisibleCharacters;
                await UniTask.WaitForSeconds(Timings.Get(_betweenCharactersTime));
            }

            IsWriting = false;
            return _textLabel.text;
        }

        public void SetText(string reactText, int visibleCount = 0)
        {
            _textLabel.text = reactText;
            _textLabel.maxVisibleCharacters = visibleCount;
            _textLabel.ForceMeshUpdate();
        }

        public void CompleteCurrent()
        {
            // _betweenCharactersTime = 0.001f;
            _textLabel.ForceMeshUpdate();
            IsWriting = false;
            var totalVisibleCharacters = _textLabel.textInfo.characterCount;
            _textLabel.maxVisibleCharacters = totalVisibleCharacters;
            _textLabel.ForceMeshUpdate();
        }

        public void SetColor(Color textColor)
        {
            _textLabel.color = textColor;
        }
    }
}