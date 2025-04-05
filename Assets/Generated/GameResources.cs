using Game.Core.VisualNovel;
using UnityEngine;
using UnityEngine.UI;

// This file is auto-generated. Do not modify manually.

public static class GameResources
{
    public static class Enemies
    {
        public static class Easy
        {
            public static EnemyAttack Kiddo_OneShot => Resources.Load<EnemyAttack>("Enemies/Easy/Kiddo_OneShot");
        }
        public static class Hard
        {
            public static EnemyAttack Bully => Resources.Load<EnemyAttack>("Enemies/Hard/Bully");
        }
        public static EnemyAttack Father => Resources.Load<EnemyAttack>("Enemies/Father");
        public static EnemyAttack Mother => Resources.Load<EnemyAttack>("Enemies/Mother");
        public static Image Projectile => Resources.Load<Image>("Enemies/Projectile");
        public static EnemyAttack _Enemy => Resources.Load<EnemyAttack>("Enemies/_Enemy");
    }
    public static class Fonts
    {
        public static TMPro.TMP_FontAsset KayPhoDu_Bold_SDF => Resources.Load<TMPro.TMP_FontAsset>("Fonts/KayPhoDu-Bold SDF");
        public static TMPro.TMP_FontAsset KayPhoDu_Regular_SDF => Resources.Load<TMPro.TMP_FontAsset>("Fonts/KayPhoDu-Regular SDF");
    }
    public static class Npc
    {
        public static Material M__Light => Resources.Load<Material>("Npc/M__Light");
        public static NPC Npc_Light => Resources.Load<NPC>("Npc/Npc_Light");
        public static Material ParticlesUnlit => Resources.Load<Material>("Npc/ParticlesUnlit");
    }
    public static class VisualNovel
    {
        public static DialogueSequence Battle_Tutor_01 => Resources.Load<DialogueSequence>("VisualNovel/Battle_Tutor_01");
        public static DialogueSequence Intro_01 => Resources.Load<DialogueSequence>("VisualNovel/Intro_01");
    }
    public static DG.Tweening.Core.DOTweenSettings DOTweenSettings => Resources.Load<DG.Tweening.Core.DOTweenSettings>("DOTweenSettings");
}
