using UnityEngine;
using UnityEngine.UI;

public class ImageComponent : MonoBehaviour
{
    [SerializeField] private RawImage rawImage;
    [SerializeField] private TMPro.TextMeshProUGUI creationTime;
    [SerializeField] private TMPro.TextMeshProUGUI imageFileName;
    [SerializeField] private bool fitWidth = true;
    private float initialRectTransformWidth;
    private float initialRectTransformHeight;

    private void Awake()
    {
        initialRectTransformWidth = rawImage.rectTransform.sizeDelta.x;
        initialRectTransformHeight = rawImage.rectTransform.sizeDelta.y;
    }

    public void SetImageTexture(Texture texture)
    {
        rawImage.texture = texture;
        if (fitWidth)
        {
            rawImage.FitRectWidthToAspectRatio();
            rawImage.rectTransform.FitSizeToRect(initialRectTransformWidth, initialRectTransformHeight);
        }
        else
        {
            rawImage.FitRectHeightToAspectRatio();
            rawImage.rectTransform.FitSizeToRect(initialRectTransformWidth, initialRectTransformHeight);
        }
    }


    public void SetImageMetadata(ImageMetadata metadata)
    {
        creationTime.text = metadata.creationTime.ToLongDateString();
        imageFileName.text = metadata.fileName;
    }
}
