using UnityEngine;
using UnityEngine.UI;

public class FaceController : MonoBehaviour
{
    [SerializeField] private Texture[] sprites;
    [SerializeField] private RawImage face;
    private int spriteIndex = 0;
    private int direction = 1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteIndex = 0;
    }

    public void ChangeFace()
    {
        spriteIndex += 1 * direction;
        face.texture = sprites[spriteIndex];
        if (spriteIndex == 0 || spriteIndex == sprites.Length - 1)
        {
            direction = direction * -1;
        }
    }

    public void ResetFace()
    {
        face.texture = sprites[0];
        spriteIndex = 0;
        direction = 1;
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
