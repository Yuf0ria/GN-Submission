//<summary>
//renders the information taken from the panel to the player
//Show the name of the player above the capsule
//Show the material color that the player took
//</summary>
using Fusion;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerCustomization : NetworkBehaviour
{
    #region Serialized Fields
    // Remove [SerializeField] - materials will be fetched automatically
    [HideInInspector] public Material[] materials;
    [SerializeField] private Renderer _playerRenderer;
    [SerializeField] private TMP_Text _name;
    #endregion
    #region Networked
    [Networked] private NetworkString<_32>  _playerName { get; set; }
    [Networked] private int _matIndex { get; set; }
    #endregion

    //for checking if materials are duplicating
    private int _lastMaterialIndex = -1;

    // Automatically fetch materials from PlayerSpawnManager
    private void Awake()
    {
        PlayerSpawnManager spawnManager = FindObjectOfType<PlayerSpawnManager>();
        if (spawnManager != null)
        {
            materials = spawnManager.materials;
            Debug.Log($"Materials fetched from PlayerSpawnManager: {materials.Length} materials found");
        }
        else
        {
            Debug.LogError("PlayerSpawnManager not found in scene!");
        }
    }

    //instantiate player info
    public void InsPlayerInfo(string name, int matIndex)
    {
        if (HasStateAuthority)
        {
            _playerName = name;
            _matIndex = matIndex;
            Debug.Log($"Player info set: Name={name}, MaterialIndex={matIndex}");
        }
        else
        {
            Debug.LogWarning("No state authority to set player info");
        }
    }

    public override void Render()
    {
        // Update name
        if (_name != null)
        {
            _name.text = _playerName.ToString();
        }
        else Debug.LogError("Name is null");

        // Update material - FIXED LOGIC
        if (materials != null && materials.Length > 0)
        {
            if (_matIndex >= 0 && _matIndex < materials.Length)
            {
                // Only update if material changed
                if (_matIndex != _lastMaterialIndex)
                {
                    _playerRenderer.material = materials[_matIndex];
                    _lastMaterialIndex = _matIndex;
                    Debug.Log($"Material applied: {materials[_matIndex].name}");
                }
            }
            else
            {
                Debug.LogWarning($"Material index {_matIndex} out of range (0-{materials.Length - 1})");
            }
        }
        else
        {
            Debug.LogWarning("No materials found in array");
        }
    }
}
