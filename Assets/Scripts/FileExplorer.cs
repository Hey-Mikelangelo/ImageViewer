using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public static class FileExplorer
{
    public static readonly string[] imageFilesExtensions =
    {
        "png", "jpg"
    };

    public static Texture2D GetImageTextureFromDisc(string discPath)
    {
        var imageBytes = File.ReadAllBytes(discPath);
        var texture = new Texture2D(2, 2);
        texture.LoadImage(imageBytes);
        return texture;
    }

    public static string[] GetFilesWithFormat(string folderPath, string[] fileFormats)
    {
        if (Directory.Exists(folderPath)== false)
        {
            Debug.LogError("Directory " + folderPath + " does not exist");
            return new string[0];
        }
        string[] allFiles = Directory.GetFiles(folderPath);
        List<string> matchingFiles = new List<string>();
        for (int i = 0; i < allFiles.Length; i++)
        {
            string file = allFiles[i];
            if (file.EndsWithAny(fileFormats))
            {
                matchingFiles.Add(file);
            }
        }
        return matchingFiles.ToArray();
    }


    public static List<ImageTextureWithMetadata> GetImageTextureWithMetadataInFolder(string folderPath)
    {
        string[] imageFilePaths = FileExplorer.GetFilesWithFormat(folderPath, imageFilesExtensions);
        List<ImageTextureWithMetadata> datas = new List<ImageTextureWithMetadata>(imageFilePaths.Length);
        for (int i = 0; i < imageFilePaths.Length; i++)
        {
            string imagePath = imageFilePaths[i];
            Texture2D imageTexture = FileExplorer.GetImageTextureFromDisc(imagePath);
            FileInfo fileInfo = new FileInfo(imagePath);

            ImageMetadata metadata = new ImageMetadata
            {
                creationTime = fileInfo.CreationTime,
                fileName = fileInfo.Name
            };
            var data = new ImageTextureWithMetadata()
            {
                texture = imageTexture,
                metadata = metadata
            };
            datas.Add(data);

        }
        return datas;
    }

}

public static class Extensions
{
    public static void FitRectWidthToAspectRatio(this RawImage rawImage)
    {
        Texture texture = rawImage.texture;
        if (texture == null) return;
        float textureHeight = texture.height;
        float imageRectTranformHeight = rawImage.rectTransform.sizeDelta.y;
        float ratio = Math.AspectRatio(texture.width, textureHeight);
        float matchedWidth = Math.AspectRatioWidth(imageRectTranformHeight, ratio);
        rawImage.rectTransform.sizeDelta = new Vector2(matchedWidth, imageRectTranformHeight);
    }

    public static void FitRectHeightToAspectRatio(this RawImage rawImage)
    {
        Texture texture = rawImage.texture;
        if (texture == null) return;
        float textureWidth = texture.width;
        float imageRectTranformWidth = rawImage.rectTransform.sizeDelta.x;
        float ratio = Math.AspectRatio(textureWidth, texture.height);
        float matchedHeight = Math.AspectRatioHeight(imageRectTranformWidth, ratio);
        rawImage.rectTransform.sizeDelta = new Vector2(imageRectTranformWidth, matchedHeight);
    }

    public static void FitSizeToRect(this RectTransform rectTransform, float maxWidth, float maxHeight)
    {
        Vector2 rectTransformSize = rectTransform.sizeDelta;
        float widthMult = maxWidth / rectTransformSize.x;
        float heightMult = maxHeight / rectTransformSize.y;
        float validMult = widthMult < heightMult ? widthMult : heightMult;
        rectTransform.sizeDelta = rectTransformSize * validMult;

    }

    public static bool EndsWithAny(this string matchedString, string[] values)
    {
        for (int i = 0; i < values.Length; i++)
        {
            if (matchedString.EndsWith(values[i]))
            {
                return true;
            }
        }
        return false;
    }
}


public static class Math
{
    public static float AspectRatio(float width, float heigth)
    {
        return width / heigth;
    }

    public static float AspectRatioHeight(float width, float ratio) => width / ratio;
    public static float AspectRatioWidth(float height, float ratio) => height * ratio;
}