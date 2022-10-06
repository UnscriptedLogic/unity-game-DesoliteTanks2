using Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BreakTextureHandler : MonoBehaviour
{
    [SerializeField] private BaseHealthClass baseHealthClass;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Texture2D[] breakTextures;
    [SerializeField] private string materialName;

    private Material material;

    private void Awake()
    {
        baseHealthClass.OnHealthChanged += SetTexture;

        for (int i = 0; i < meshRenderer.materials.Length; i++)
        {
            if (meshRenderer.materials[i].name.Contains(materialName))
            {
                material = meshRenderer.materials[i];
                material.EnableKeyword("_DetailAlbedoMap");
                material.EnableKeyword("_DetailMask");
                material.SetTexture("_DetailMask", null);
                material.SetTexture("_DetailAlbedoMap", null);
            }
        }
    }

    private void SetTexture(float damageAmount)
    {
        float healthPercent = baseHealthClass.CurrentHealth / baseHealthClass.MaxHealth * 100f;
        int textureIndex = Mathf.Clamp(Mathf.RoundToInt((100 - healthPercent) / 100 * (breakTextures.Length - 1)), 0, breakTextures.Length - 1);
        material.SetTexture("_DetailMask", breakTextures[textureIndex]);
        material.SetTexture("_DetailAlbedoMap", breakTextures[textureIndex]);
    }
}
