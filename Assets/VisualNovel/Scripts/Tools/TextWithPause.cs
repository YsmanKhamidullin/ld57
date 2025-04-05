using System;
using System.Collections;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Game.Core.VisualNovel
{
    public class TextWithPause : MonoBehaviour
    {
        public event Action OnStopShowText;

        [SerializeField]
        private TextMeshProUGUI _label;

        [SerializeField]
        private PauseInfo _pauseInfo;

        private bool IsFinished => _index >= _finalText.Length;
        private int _index;
        private string _actualText = "";
        private string _finalText = "";

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void ShowText(string value)
        {
            if (_index != 0)
            {
                FinishCurrentText();
            }

            _finalText = value;
            _actualText = "";
            _index = 0;
            gameObject.SetActive(true);

            ReproduceText();
        }

        private void ReproduceText()
        {
            if (IsFinished)
            {
                OnStopShowText?.Invoke();
                _index = 0;
                _actualText = "";
                gameObject.SetActive(false);
                return;
            }

            char letter = _finalText[_index];
            _label.text = Write(letter);
            _index += 1;
            StartCoroutine(PauseBetweenChars(letter));
        }

        private string Write(char letter)
        {
            _actualText += letter;
            return _actualText;
        }

        private IEnumerator PauseBetweenChars(char symbol)
        {
            float pauseDuration = symbol switch
            {
                '.' => _pauseInfo._dotPause,
                ',' => _pauseInfo._commaPause,
                ' ' => _pauseInfo._spacePause,
                _ => _pauseInfo._normalPause,
            };
            yield return new WaitForSeconds(pauseDuration);
            ReproduceText();
        }

        private void FinishCurrentText()
        {
            _label.text = _finalText;
            _index = _finalText.Length;
        }

        [Button]
        private void Test()
        {
            ShowText(
                "Sometimes, I think the forest remembers every step I've ever taken. It's comforting to know I'm never truly alone.");
        }

        [Button]
        private void TestFont()
        {
            _label.text =
                "Tangalar yetarli emas\nТиындар жеткіліксіз\nKifayət qədər pul yoxdur\nYeterli bozuk para yok\nНедостаточно монет\nNot Enough Coins\n! , . / ; ' [ ] $ # @ % ^ - &  * ( ) - = +";
        }

        [Serializable]
        public class PauseInfo
        {
            public float _dotPause = 0.5f;
            public float _commaPause = 0.1f;
            public float _spacePause = 0.05f;
            public float _normalPause = 0.05f;
            public float _finalPause = 3f;
        }
    }
}