using Game.Core.VisualNovel;
using UnityEngine;

// This file is auto-generated. Do not modify manually.

public static class GameResources
{
    public static class Enemies
    {
        public static Enemy Mother => Resources.Load<Enemy>("Enemies/Mother");
        public static Enemy _Enemy => Resources.Load<Enemy>("Enemies/_Enemy");
    }
    public static class Fonts
    {
        public static TMPro.TMP_FontAsset KayPhoDu_Bold_SDF => Resources.Load<TMPro.TMP_FontAsset>("Fonts/KayPhoDu-Bold SDF");
        public static TMPro.TMP_FontAsset KayPhoDu_Regular_SDF => Resources.Load<TMPro.TMP_FontAsset>("Fonts/KayPhoDu-Regular SDF");
    }
    public static class VisualNovel
    {
        public static DialogueSequence Intro_01 => Resources.Load<DialogueSequence>("VisualNovel/Intro_01");
    }
    public static DG.Tweening.Core.DOTweenSettings DOTweenSettings => Resources.Load<DG.Tweening.Core.DOTweenSettings>("DOTweenSettings");
}
