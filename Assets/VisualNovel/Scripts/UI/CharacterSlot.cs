using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.VisualNovel
{
    public class CharacterSlot : MonoBehaviour
    {
        public DialogueCharacter Current { get; private set; }

        [SerializeField]
        private Image _imageParent;

        public void Add(DialogueCharacter currentCharacter)
        {
            SetSprite(currentCharacter);
            _imageParent.enabled = currentCharacter.Character.FullSprite != null;
            _imageParent.gameObject.SetActive(true);
            Refresh(currentCharacter);
        }

        public void Refresh(DialogueCharacter freshCharacter)
        {
            gameObject.SetActive(freshCharacter.IsVisible);
            Current = freshCharacter;
            ResolveIsTalking(freshCharacter.IsActiveTalk);
        }

        public void Clear()
        {
            _imageParent.gameObject.SetActive(false);
            Current = null;
        }


        private void SetSprite(DialogueCharacter currentCharacter)
        {
            var character = currentCharacter.Character;
            var sprite = character.FullSprite;
            _imageParent.sprite = sprite;
        }

        private void ResolveIsTalking(bool isTalk)
        {
            Color color = TalkColor(isTalk);
            _imageParent.color = color;
        }

        public static Color TalkColor(bool isTalk)
        {
            return isTalk ? Color.white : new Color(200f / 255f, 200f / 255f, 200f / 255f, 1f);
        }

        public void SetAlpha(float a)
        {
            _imageParent.Alpha(a);
        }
    }
}