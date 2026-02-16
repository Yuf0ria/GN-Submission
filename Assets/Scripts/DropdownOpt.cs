
using UnityEngine;
using Fusion;
using TMPro;

public class DropdownManager : NetworkBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private Renderer targetRenderer; //renderer ng player
    
    [SerializeField] private Material[] materials;
    
    [Networked] //pang register, yes alam ko na to YIPPEE
    public int SelectedMaterialIndex { get; set; }
    private int lastSelectedMaterialIndex = -1;

    void Start()
    {
        // Ensure the dropdown is assigned
        if (dropdown == null)
        {
            Debug.LogError("Dropdown not assigned!");
            return;
        }
        SetupDropdown();
        dropdown.onValueChanged.AddListener(OnMaterialSelected);
    }
    
    void SetupDropdown()
    {
        dropdown.ClearOptions();
        
        // Add material names to dropdown
        foreach (Material mat in materials)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(mat.name));
        }
        
        dropdown.RefreshShownValue();
    }
    
    void OnMaterialSelected(int index)
    {
        if (HasInputAuthority)
        {
            SelectedMaterialIndex = index;
        }
    }
    
    public override void Render()
    {
        // Update material when index changes
        if (SelectedMaterialIndex != lastSelectedMaterialIndex)
        {
            ApplyMaterial(SelectedMaterialIndex);
            dropdown.value = SelectedMaterialIndex;
            lastSelectedMaterialIndex = SelectedMaterialIndex;
        }
    }
    
    void ApplyMaterial(int index)
    {
        if (index >= 0 && index < materials.Length)
        {
            targetRenderer.material = materials[index];
        }
    }
}

