using Fusion;
using TMPro;
using UnityEngine;

public class PlayerNameHandler : NetworkBehaviour
{
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_Text displayNameText; // Where to show the name
    
    [Networked]
    public NetworkString<_32> PlayerName { get; set; }
    
    void Start()
    {
        // Set initial name when spawned
        if (HasInputAuthority)
        {
            PlayerName = "Player" + Object.InputAuthority.PlayerId;
            nameInputField.text = PlayerName.ToString();
        }
    }
    
    // Call this from a "Confirm" button
    public void OnConfirmButtonClick()
    {
        if (HasInputAuthority)
        {
            PlayerName = nameInputField.text;
        }
    }
    
    // Update display every frame
    void Update()
    {
        if (displayNameText != null)
        {
            displayNameText.text = PlayerName.ToString();
        }
    }
}


