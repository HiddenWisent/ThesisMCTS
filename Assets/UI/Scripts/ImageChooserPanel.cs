using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageChooserPanel : MonoBehaviour
{
    [SerializeField] 
    private ImageChooser _imageChooser;

    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }

    public void ChooseImage(Sprite sprite)
    {
        gameObject.SetActive(false);
        _imageChooser.ChangeSprite(sprite);
    }
}
