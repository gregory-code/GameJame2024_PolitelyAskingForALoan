using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageFlash : MonoBehaviour
{
    [SerializeField] Player player;

    [SerializeField] Color defaultColor;
    [SerializeField] Color damagedColor;
    [SerializeField] string colorAttributeName = "_Color";

    Color currentColor;

    List<Material> materials = new List<Material>();
    [SerializeField] float lerpSpeed = 2;

    private void Awake()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material = new Material(renderer.material);
            materials.Add(renderer.material);
            currentColor = renderer.material.GetColor(colorAttributeName);
        }
        player.onTakeDamage += TakeDamage;
        currentColor = defaultColor;
    }

    private void TakeDamage(Vector3 shotDirection, Rigidbody shotRigidbody, bool wouldKill)
    {
        if (Mathf.Abs((currentColor - defaultColor).grayscale) < 0.1)
        {
            currentColor = damagedColor;
            SetMaterialColor(currentColor);
        }
    }

    private void Update()
    {
        currentColor = Color.Lerp(currentColor, defaultColor, Time.deltaTime * lerpSpeed);
        SetMaterialColor(currentColor);
    }

    private void SetMaterialColor(Color color)
    {
        foreach (Material material in materials)
        {
            material.SetColor(colorAttributeName, color);
        }
    }
}
