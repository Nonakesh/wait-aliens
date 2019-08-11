using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ResourceList : MonoBehaviour
{
    public string Prepend, Append;
    
    public ResourceType Type;

    public Text Text;

    public void Update()
    {
        var builder = new StringBuilder();

        Text.text = $"{Prepend}{ResourceManager.GetResource(Type)}{Append}";
    }
}