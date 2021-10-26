using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetPosition : MonoBehaviour
{
    private void OnMouseUp()
    {
        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Game.game.DrawPath(new Vector2((cursorPos.x), (cursorPos.z)));
    }
}
