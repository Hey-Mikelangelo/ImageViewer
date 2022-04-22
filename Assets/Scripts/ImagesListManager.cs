using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class ImagesListManager : MonoBehaviour
{
    public List<ImageComponent> imagesComponents = new List<ImageComponent>();
    [SerializeField] private GameObject imageComponentPrefab;
    [SerializeField] private Transform imagesHolderTransform;
    [SerializeField] bool fromInput;
    [SerializeField] private InputField imagesFolderPathInputField;
    [SerializeField] private int preallocateCount = 1;
    private void OnValidate()
    {
        if (imageComponentPrefab.TryGetComponent<ImageComponent>(out _) == false)
        {
            Debug.LogError($"{nameof(imageComponentPrefab)} should have {typeof(ImageComponent)} component");
        }
    }

    private void Awake()
    {
        EnsureEnougthImagesCount(preallocateCount);
        EnableImageComponents(0);
    }

    private void OnEnable()
    {
        RefreshImagesList();
    }

    public void RefreshImagesList()
    {
        string folderPath = imagesFolderPathInputField.text ;
        var imageTexturesWithMetadatas = FileExplorer.GetImageTextureWithMetadataInFolder(folderPath);
        int imagesCount = imageTexturesWithMetadatas.Count;
        EnsureEnougthImagesCount(imagesCount);
        EnableImageComponents(imagesCount);
        SetImagesData(imageTexturesWithMetadatas);
    }

    private void SetImagesData(List<ImageTextureWithMetadata> imageTextureWithMetadatas)
    {
        Assert.IsTrue(imagesComponents.Count >= imageTextureWithMetadatas.Count);

        for (int i = 0; i < imageTextureWithMetadatas.Count; i++)
        {
            ImageComponent imageComponent = imagesComponents[i];
            ImageTextureWithMetadata imageTextureWithMetadata = imageTextureWithMetadatas[i];
            imageComponent.SetImageTexture(imageTextureWithMetadata.texture);
            imageComponent.SetImageMetadata(imageTextureWithMetadata.metadata);
        }
    }

    private void EnableImageComponents(int count)
    {
        for (int i = 0; i < imagesComponents.Count; i++)
        {
            bool enableImage = i < count;
            EnableImageComponent(imagesComponents[i], enableImage);
        }
    }

    private void EnableImageComponent(ImageComponent imageComponent, bool enable)
    {
        imageComponent.gameObject.SetActive(enable);
    }

    private void EnsureEnougthImagesCount(int count)
    {
        int missingImagesCount = count - imagesComponents.Count;

        if (missingImagesCount <= 0) return;

        for (int i = 0; i < missingImagesCount; i++)
        {
            var imageComponent = GetNewImageComponent();
            imagesComponents.Add(imageComponent);
        }
    }

    private ImageComponent GetNewImageComponent()
    {
        ImageComponent imageComponent = GameObject.Instantiate(imageComponentPrefab).GetComponent<ImageComponent>();
        imageComponent.transform.SetParent(imagesHolderTransform);
        return imageComponent;
    }
}
