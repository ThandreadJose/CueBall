using UnityEngine;

public class UpdateRoom : SpecialEvents
{

    [SerializeField] int roomId;

    public override void eventAction()
    {
        if (!occured)
        {
            if (room.isTalking())
            {
                GameController gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();
                gameController.SetRoom(roomId);
                occured = true;
            }
        }
    }
}
