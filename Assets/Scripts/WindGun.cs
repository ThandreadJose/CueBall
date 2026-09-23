using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class WindGun : HeldItem
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created



    private void Awake()
    {
        charge = 0;
        chargedir = 1;
        gunType = "GUN";
    }

    public override void Charge(MouseController mc)
    {
        base.Charge(mc);

            charge += chargedir * Time.deltaTime;

            if (charge > 1)
            {
                charge = 1;
                chargedir = chargedir * -1;
                
            }
            else if (charge < 0)
            {
                charge = 0;
                chargedir = chargedir * -1;
            }

            if (charge < 0.3)
            {
                currentSprite = sprite1;
            }
            else if (charge >= 0.9)
            {
                currentSprite = sprite3;
            }
            else if (charge >= 0.6)
            {
                currentSprite = sprite2;
            }
    }


}
