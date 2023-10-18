using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum ImageTpye {
    happy,shy,what,wow
}

public class PlayerImageManager : MonoBehaviour
{
    public  List<Sprite>imageList = new List<Sprite>();
    
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeImage(ImageTpye image)
    {
        spriteRenderer.sprite = imageList[(int)image];
    }

}
