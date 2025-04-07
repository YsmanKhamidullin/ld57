using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FightWindow : BaseWindow
{
    public EnemyWill EnemyWill;
    public EnemyVisual EnemyVisual;
    public PlayerHeart PlayerHeart;
    public BattleField BattleField;
    public Transform CardsContainer;
    public TextMeshProUGUI EnemyName;
    public Image EnemyImage;

    private ServiceFight _service;
    private Enemy _enemy;
    private Player _player;

    private bool _isBattleFieldShowed;
    private bool _isMovingPlayer;

    public override void Awake()
    {
        base.Awake();
        Hide();
    }

    public void SetUp(ServiceFight serviceFight, Player player, Enemy enemy)
    {
        _player = player;
        _enemy = enemy;
        _service = serviceFight;
        EnemyName.text = enemy.EnemyName;
        EnemyName.color = enemy.Color;
        EnemyImage.sprite = enemy.EnemySprite;
        EnemyWill.SetUp(enemy);
        EnemyVisual.SetUp();
    }

    public override void Hide()
    {
        base.Hide();
        EnemyVisual.Hide();
    }

    public async UniTask UpdateWill()
    {
        EnemyWill.UpdateCurrent();
        await UniTask.WaitForSeconds(0.1f);
    }

    public async UniTask SetUpActionPhase()
    {
        _isBattleFieldShowed = false;
        await BattleField.transform.DOScale(Vector3.one * 0.6f, 0.25f).SetEase(Ease.InOutSine)
            .ToUniTask();
        CardsContainer.gameObject.SetActive(true);
    }

    public async UniTask SetUpBattlePhase()
    {
        _isBattleFieldShowed = false;
        await BattleField.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.InOutSine)
            .OnUpdate(
                () =>
                {
                    if (_heartBattleCell != null)
                    {
                        PlayerHeart.SetPos(_heartBattleCell.transform.position);
                    }
                })
            .ToUniTask();
        _isBattleFieldShowed = true;
        CardsContainer.gameObject.SetActive(false);
    }

    public async UniTask MovePlayerTo(int index)
    {
        var bc = BattleField.Cells[index];
        await MovePlayerHeart(bc);
    }

    public async UniTask TryMovePlayerTo(BattleCell battleCell)
    {
        if (!Onboarding.IsOnboardingFirstBattleStarted || Onboarding.IsInOnboarding || _isMovingPlayer ||
            !_isBattleFieldShowed)
        {
            return;
        }

        await MovePlayerHeart(battleCell);
    }

    private BattleCell _heartBattleCell;

    private async UniTask MovePlayerHeart(BattleCell battleCell)
    {
        _isMovingPlayer = true;
        _heartBattleCell = battleCell;
        await PlayerHeart.MoveToCell(battleCell);
        _isMovingPlayer = false;
    }

    public async UniTask HandleEnemyDeath(Enemy enemy)
    {
        bool isWillZero = enemy.CurrentWill <= 0;
        if (isWillZero && enemy.IsWinByMercy)
        {
            await EnemyVisual.StartDialogueBeforeVanish();
        }

        await AnimateVanishEnemy();
    }

    private async UniTask AnimateVanishEnemy()
    {
        await EnemyVisual.Vanish();
    }
}