using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ResourceList : MonoBehaviour
{
    public ResourceType Type;

    public Text Text;

    public void Update()
    {
        var builder = new StringBuilder();

        var name = Enum.GetName(typeof(ResourceType), Type);
        Text.text = $"{name}: {ResourceManager.GetResource(Type)}";
    }
}