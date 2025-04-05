using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class ZoneUtil
{
    public enum ZoneType
    {
        Hand,
        Center,
    }

    public static ZoneType Check()
    {
        var graphicRaycaster = Root.Instance.ServiceUi.FullScreenRayCaster;
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();

        graphicRaycaster.Raycast(pointerData, results);

        foreach (var result in results)
        {
            if (result.gameObject.TryGetComponent<BaseCanvasZone>(out var zone))
            {
                switch (zone)
                {
                    case HandZone:
                        return ZoneType.Hand;
                    case CenterZone:
                        return ZoneType.Center;
                }
            }
        }

        return ZoneType.Hand;
    }
}