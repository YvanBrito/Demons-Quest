using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class PressButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler// required interface when using the OnPointerDown method.
{
    public Player player;

    //Do this when the mouse is clicked over the selectable object this script is attached to.
    public void OnPointerDown(PointerEventData eventData)
    {
        if (transform.name == "AttackButton")
        {
            player.Attack();
        }
        else if (transform.name == "UpButton")
        {
            player.Jump();
        }
        else if (transform.name == "LeftButton")
        {
            player.amount = -1;
            Debug.Log(this.gameObject.name + " Was Clicked Down.");
        }
        else if (transform.name == "RightButton")
        {
            player.amount = 1;
            Debug.Log(this.gameObject.name + " Was Clicked Down.");
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (transform.name == "RightButton" || transform.name == "LeftButton")
        {
            player.amount = 0;
            Debug.Log(this.gameObject.name + " Was Clicked Up.");
        }
    }
}