using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FightWindow : BaseWindow
{
    public Image EnemyWillFillImage;
    public Image PlayerWillFillImage;
    public Transform BattleField;

    public TextMeshProUGUI EnemyName;

    private ServiceFight _service;
    private Enemy _enemy;
    private Player _player;

    public void SetUp(ServiceFight serviceFight, Player player, Enemy enemy)
    {
        _player = player;
        _enemy = enemy;
        _service = serviceFight;
        EnemyName.text = enemy.EnemyName;
        UpdateWill();
    }

    private void UpdateWill()
    {
        EnemyWillFillImage.fillAmount = _enemy.CurrentWill / (float)_enemy.MaxWill;
        PlayerWillFillImage.fillAmount = _player.CurrentWill / (float)_player.MaxWill;
    }

    public async UniTask AnimateAppearAndFight()
    {
        CanvasGroup.alpha = 0f;
        await CanvasGroup.FadeOut();
    }

    public async UniTask SetUpActionPhase()
    {
        BattleField.gameObject.SetActive(false);
    }

    public async UniTask SetUpBattlePhase()
    {
        BattleField.gameObject.SetActive(true);
    }
}