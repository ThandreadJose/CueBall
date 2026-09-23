using UnityEngine;
using UnityEngine.UI;

public class HeldItem : MonoBehaviour
{
    [Header("Weapon Data")]
    [SerializeField] protected RawImage rawImage;
    [SerializeField] protected Texture currentSprite;
    [SerializeField] protected MouseController mouseController;
    [SerializeField] protected string gunType = "NONE";
    protected float charge = 0;
    protected int chargedir = 1;

    [Header("Sprites")]
    [SerializeField] protected Texture sprite1;
    [SerializeField] protected Texture sprite2;
    [SerializeField] protected Texture sprite3;


    [Header("Bullet")]
    [SerializeField] protected GameObject bullet;


    public virtual void Charge(MouseController mc)
    {
        mouseController = mc;


    }
    public virtual float getCharge()
    {
        float currentCharge = charge;

        charge = 0;
        currentSprite = sprite1;
        chargedir = 1;
        return currentCharge;
    }

    public virtual float checkCharge()
    {
        return charge;
    }

    public virtual string getGun()
    {
        string gun = gunType;

        return gun;
    }

    public virtual GameObject getBullet()
    {
        return bullet;
    }

    public virtual Texture getSprite()
    {
        return currentSprite;
    }

    public virtual RawImage getHeldObject()
    {
        return rawImage;
    }

    private void Update()
    {
        rawImage.texture = currentSprite;
    }

}
