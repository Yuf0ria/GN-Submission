using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UILogin : MonoBehaviour
{
    public TMP_InputField InputField;
    void Start()
    {
        if(PlayerPrefs.HasKey("Player"))
            InputField.text = PlayerPrefs.GetString("Player");//brain no work but hope-core for this
    }

    //if player join store value
    //joingame();
    public void JoinGame(){
        PlayerPrefs.GetString("Player", InputField.text);
        PlayerPrefs.Save(); //:D

        SceneManager.LoadScene("SampleScene");
        SceneManager.UnloadSceneAsync("Login");
    }
}
