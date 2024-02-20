using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControllerDeneme : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventManagement.Invoke_OnActiveWinPanel();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ComboController.Instance.ShowComboImage(4);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            FloatingTextController.Instance.SpawnFloatingText(transform.position, 10, SpriteType.Gold,false);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            BallSpawner.Instance.SpawnBallPieceToAchivement(10, transform.position);
        }
    }
}
