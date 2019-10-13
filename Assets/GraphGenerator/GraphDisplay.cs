﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphDisplay : MonoBehaviour
{
    private MeshGenerator meshGenerator;
    private ImageProcessor imageProcessor;

    // Start is called before the first frame update
    void Start()
    {
        meshGenerator = new MeshGenerator(new Vector2(0.01f, 0.01f));

        Func<float, float, float> cone = (x, z) =>
        {
            float a = 1;
            float b = 1;
            float c = 1;

            //return 1f - (x + z);
            return c * Mathf.Sqrt((Mathf.Pow(x, 2) / Mathf.Pow(a, 2)) + (Mathf.Pow(z, 2) / Mathf.Pow(b, 2)));
        };

        MeshData meshData = meshGenerator.GenerateMeshData(
            cone,
            new Vector2(-1, -1),
            new Vector2(1, 1)
        );

        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.vertices = meshData.vertices;
        mesh.triangles = meshData.triangles;
        mesh.uv = meshData.uvs;

        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().mesh = mesh;

        imageProcessor = new ImageProcessor();
        StartCoroutine(
            imageProcessor.PostImage(
                "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAABDgAAAEgCAYAAABRrcPeAAAAAXNSR0IArs4c6QAAAERlWElmTU0AKgAAAAgAAYdpAAQAAAABAAAAGgAAAAAAA6ABAAMAAAABAAEAAKACAAQAAAABAAAEOKADAAQAAAABAAABIAAAAACdM7CzAAAAHGlET1QAAAACAAAAAAAAAJAAAAAoAAAAkAAAAJAAAEKmcLmr9AAAQABJREFUeAHsnQm4HEW5sFW4qIgIshowskQMCSEXcCEIiFGRRUAQwXBZRZCA4AVkR3ZlEQnIIiRBCEJYZRMEDQQQNCQkEEQQQmJIQAQBCYuy03/eun+Nk5Mz5/TM6enpnnm/55mnz5nprq56u7qWr776vve8pxtJkuQD8z+bz//8ZP7nN/M/M+d/Xpj/eWv+R5GABCQgAQlIQAISkIAEJCABCUhAAoUisIB6Y37O1pj/OX/+51+FyqWZkYAEJCABCUhAAhKQgAQkIAEJSEACPRAICo75vy86/3P8/M+bPZzrTxKQgAQkIAEJSEACEpCABCQgAQlIoJAE3jM/V4vM/9xQyNyZKQlIQAISkIAEJCABCUhAAhKQgAQkkIIACo4fpDjPUyQgAQlIQAISkIAEJCABCUhAAhKQQGEJoOD4a2FzZ8YkIAEJSEACEpCABCQgAQlIQAISkEAKAig43k1xnqdIQAISkIAEJCABCUhAAhKQgAQkIIHCEkDBoUhAAhKQgAQkIAEJSEACEpCABCQggVITUMFR6sdn5iUgAQlIQAISkIAEJCABCUhAAhKAgAoO64EEJCABCUhAAhKQgAQkIAEJSEACpSeggqP0j9ACSEACEpCABCQgAQlIQAISkIAEJKCCwzogAQlIQAISkIAEJCABCUhAAhKQQOkJqOAo/SO0ABKQgAQkIAEJSEACEpCABCQgAQmo4LAOSEACEpCABCQgAQlIQAISkIAEJFB6Aio4Sv8ILYAEJCABCUhAAhKQgAQkIAEJSEACKjisAxKQgAQkIAEJSEACEpCABCQgAQmUnoAKjtI/QgsgAQlIQAISkIAEJCABCUhAAhKQgAoO64AEJCABCUhAAhKQgAQkIAEJSEACpSeggqP0j9ACSEACEpCABCQgAQlIQAISkIAEJKCCwzogAQlIQAISkIAEJCABCUhAAhKQQOkJqOAo/SO0ABKQgAQkIAEJSEACEpCABCQgAQmo4LAOSEACEpCABCQgAQlIQAISkIAEJFB6Aio4Sv8ILYAEJCABCUhAAhKQgAQkIAEJSEACKjisAxKQgAQkIAEJSEACEpCABCQgAQmUnoAKjtI/QgsgAQlIQAISkIAEJCABCUhAAhKQgAoO64AEJCABCUhAAhKQgAQkIAEJSEACpSeggqP0j9ACSEACEpCABCQgAQlIQAISkIAEJKCCwzogAQlIQAISkIAEJCABCUhAAhKQQOkJqOAo/SO0ABKQgAQkIAEJSEACEpCABCQgAQmo4LAOSEACEpCABCQgAQlIQAISkIAEJFB6Aio4Sv8ILYAEJCABCUhAAhKQgAQkIAEJSEACKjisAxKQgAQkIAEJSEACEpCABCQgAQmUnoAKjtI/QgsgAQlIQAISkIAEJCABCUhAAhKQgAoO64AEJCABCUhAAhKQgAQkIAEJSEACpSeggqP0j9ACSEACEpCABCQgAQlIQAISkIAEJKCCwzogAQlIQAISkIAEJCABCUhAAhKQQOkJqOAo/SO0ABKQgAQkIAEJSEACEpCABCQgAQmo4LAOSEACEpCABCQgAQlIQAISkIAEJFB6Aio4Sv8ILYAEJCABCUhAAhKQgAQkIAEJSEACKjisAxKQgAQkIAEJSEACEpCABCQgAQmUnoAKjtI/QgsgAQlIQAISkIAEJCABCUhAAhKQgAoO64AEJCABCUhAAhKQgAQkIAEJSEACpSeggqP0j9ACSEACEpCABCQgAQlIQAISkIAEJKCCwzogAQlIQAISkIAEJCABCUhAAhKQQOkJqOAo/SO0ABKQgAQkIAEJSEACEpCABCQgAQmo4LAOSEACEpCABCQgAQlIQAISkIAEJFB6Aio4Sv8ILYAEJCABCUhAAhKQgAQkIAEJSEACKjisAxKQgAQkIAEJSEACEpCABCQgAQmUnoAKjtI/QgsgAQlIQAISkIAEJCABCUhAAhKQgAoO64AEJCABCUhAAhKQgAQkIAEJSEACpSeggqP0j9ACSEACEpCABCQgAQlIQAISkIAEJKCCwzogAQlIQAISkIAEJCABCUhAAhKQQOkJqOAo/SO0ABKQgAQkIAEJSEACEpCABCQgAQmo4LAOSEACEpCABCQgAQlIQAISkIAEJFB6Aio4Sv8ILYAEJCABCUhAAhKQgAQkIAEJSEACKjisAxKQgAQkIAEJSEACEpCABCQgAQmUnoAKjtI/QgsgAQlIQAISkIAEJCABCUhAAhKQgAoO64AEJCABCUhAAhKQgAQkIAEJSEACpSeggqP0j9ACSEACEpCABCQgAQlIQAISkIAEJKCCwzogAQlIQAISkIAEJCABCUigTQi8+eabyaRJk5Jf/vKXyZVXXpk88MADyTvvvNMmpbMYEuiZgAqOnvn4qwQkIAEJSEACEpCABCQggVIQmDhxYrLssssm73nPe5LFF188HPmbz0EHHZTMmzevFOUwkxJolIAKjkbJeZ0EJCABCUhAAhKQgAQkIIGCEJg9e3ZFodG/f//kzDPPTEaPHp0cffTRyXLLLVf57eKLLy5Ijs2GBLInoIIje6amKAEJSEACEpCABCQgAQlIIDcCb731VkWBgUKjO/nb3/6WbLTRRuG8JZZYInn33Xe7O63P3z3//PPJhRdemIwYMSJZa621khVXXDF8hgwZkuy///7J1KlT+3wPE5BALQIqOGqR8XsJSEACEpCABCQgAQlIQAIlIHDrrbcGxcXIkSN7ze2MGTPCuRMmTOj13LQnoCz59a9/nay//vrJYostlqBAiVtjqo+LLrpo8qEPfSg54IAD0ibteRKoi4AKjrpwebIEJCABCUhAAhKQgAQkIIFiEdh9992DQuGxxx5LlTGcjt5///2pzu3tpMmTJyeDBg1KPvzhD3er1KhWcFT/jUXH22+/3Vvy/i6Bugio4KgLlydLQAISkIAEJCABCUhAAhIoFoHoYyPvXI0fPz75wAc+UFOxgdLjgx/8YDiHrSpLLbVU+BsrD77fY4898s6y92tzAio42vwBWzwJSEACEpCABCQgAQlIoL0JoGQYOHBgroW85557FlJsLLLIImF7ytJLL51gVXLdddclc+fOXSBfc+bMqVh7oOS4+eabF/jdfyTQFwIqOPpCz2slIAEJSEACEpCABCQgAQm0mABbP9jyUY/cd999CdFWGpW43eR973tfUFig1MC3BummkY9+9KNBQUJY23//+99pLvEcCfRKQAVHr4g8QQISkIAEJCABCUhAAhKQQHEJoGxYd911U2fwtddeq1hfpL6o6kScikYFx6GHHppMmjSp7qgs1157bVCM4JB03LhxVan7pwQaJ6CCo3F2XikBCUhAAhKQgAQkIAEJSKDlBFA2fOQjH0mdD6wsooIi9UVVJ952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"
            )
        );
    }

    // Update is called once per frame
    void Update()
    {
        if (imageProcessor.updated)
        {
            print(imageProcessor.text);
            imageProcessor.updated = false;
        }
    }
}
