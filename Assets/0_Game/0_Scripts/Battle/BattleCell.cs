using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BattleCell : MonoBehaviour, IPointerDownHandler
{
    public Image Image;

    public void OnPointerDown(PointerEventData eventData)
    {
        Root.Instance.ServiceFight.TryMovePlayerTo(this);
    }
}