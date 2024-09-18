using UnityEngine;

public class MainPanel : MonoBehaviour
{
    public void BtnClickGameStart()
    {
        Utils.LoadScene("Scenes/Game");
        //Utils.LoadScene("Game");
        //Utils.LoadScene(SceneNames.Game);
    }
}
