using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Core.VisualNovel
{
    public class DialogueSequenceCharacters : MonoBehaviour
    {
        [SerializeField]
        private List<CharacterSlot> _characterSlots;

        private Dialogue _dialogue;

        public void SetUp(Dialogue dialogue)
        {
            _dialogue = dialogue;
            var currentCharacters = dialogue.VisibleCharacters;
            RefreshCharacterSlots(currentCharacters);
            AddNotExistingCharacters(currentCharacters);
            OverrideRender();
        }

        private void OverrideRender()
        {
            for (int i = 0; i < _characterSlots.Count; i++)
            {
                var slot = _characterSlots[i];
                slot.SetAlpha(_dialogue.CharactersAlpha);
            }
        }

        private void AddNotExistingCharacters(List<DialogueCharacter> currentCharacters)
        {
            foreach (var currentCharacter in currentCharacters)
            {
                bool isExist = _characterSlots.Any(c =>
                    c.Current != null && c.Current.Character.Id == currentCharacter.Character.Id);
                if (isExist)
                {
                    continue;
                }

                var emtySlot = _characterSlots.FirstOrDefault(c => c.Current == null);
                if (emtySlot != null)
                {
                    emtySlot.Add(currentCharacter);
                }
                else
                {
                    Debug.LogWarning($"Cant find slot for dialogue character: {currentCharacter.Character.Id}");
                }
            }
        }

        private void RefreshCharacterSlots(List<DialogueCharacter> currentCharacters)
        {
            for (var i = 0; i < _characterSlots.Count; i++)
            {
                var slot = _characterSlots[i];
                if (slot.Current == null)
                {
                    continue;
                }

                var freshCharacter = currentCharacters.FirstOrDefault(c => c.Character.Id == slot.Current.Character.Id);
                if (freshCharacter != null)
                {
                    slot.Refresh(freshCharacter);
                    continue;
                }

                slot.Clear();
            }
        }
    }
}