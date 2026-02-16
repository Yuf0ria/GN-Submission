using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class InputfieldManagerScript : NetworkBehaviour
{
    public PlayerNameHandler playerHandler;
    public Button btn;
    public GameObject panel;

    private void Start()
    {
        btn.onClick.AddListener(OnBtnClick);
        panel =  GameObject.FindWithTag("Panel");
    }

    public void OnBtnClick()
    {
        playerHandler.OnConfirmButtonClick();
        //get dropdown color
        //find game object then set active false
        if (panel != null)
        {
            panel.SetActive(false);
        }
    }
}
