using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FightWindow : BaseWindow
{
    public Image EnemyWillFillImage;
    public Image PlayerWillFillImage;
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
        EnemyImage.sprite = enemy.EnemySprite;
        UpdateWill();
    }

    public void UpdateWill()
    {
        EnemyWillFillImage.DOFillAmount(_enemy.CurrentWill / (float)_enemy.MaxWill, 0.15f).SetEase(Ease.OutSine);
        PlayerWillFillImage.DOFillAmount(_player.CurrentWill / (float)_player.MaxWill, 0.15f).SetEase(Ease.OutSine);
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
                () => { PlayerHeart.SetPos(_heartBattleCell.transform.position); })
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
        if (!Onboarding.isOnboardingFirstBattleStarted || Onboarding.IsInOnboarding || _isMovingPlayer || !_isBattleFieldShowed)
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
}