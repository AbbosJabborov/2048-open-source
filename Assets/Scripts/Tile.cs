using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Tile : MonoBehaviour
{
    public int value;
    public TextMeshProUGUI valueText;
    public Image image;

    public void SetValue(int newValue)
    {
        value = newValue;
        valueText.text = value == 0 ? "" : value.ToString();
        UpdateColor();
    }

    public void MoveTo(Vector3 targetPos, float duration = 0.15f)
    {
        transform.DOKill();
        transform.DOMove(targetPos, duration).SetEase(Ease.InOutQuad);
    }

    void UpdateColor()
    {
        Color32[] tileColors =
        {
            new Color32(232, 242, 255, 255), // 0  (empty frost)
            new Color32(215, 230, 255, 255), // 2
            new Color32(198, 218, 255, 255), // 4
            new Color32(178, 207, 255, 255), // 8
            new Color32(158, 196, 255, 255), // 16
            new Color32(138, 185, 255, 255), // 32
            new Color32(118, 174, 255, 255), // 64
            new Color32(140, 200, 230, 255), // 128
            new Color32(120, 185, 215, 255), // 256
            new Color32(100, 165, 195, 255), // 512
            new Color32(82, 147, 178, 255),  // 1024
            new Color32(65, 130, 160, 255)   // 2048 (deep winter blue)
        };

        int index = Mathf.Clamp((int)Mathf.Log(value == 0 ? 1 : value, 2), 0, tileColors.Length - 1);
        image.color = tileColors[index];
    }
}