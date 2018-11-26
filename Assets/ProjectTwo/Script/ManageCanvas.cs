using UnityEngine;

public class ManageCanvas : MonoBehaviour
{
    public static ManageCanvas instance;
    bool ativada = true;

    [SerializeField]
    private MangePlayer _managePlayer;
    private MangePlayer MangePlayer
    {
        get { return _managePlayer; }
    }

    [SerializeField]
    private GameObject escolhasSala;


    public void OnClickJoinRoom(string roomName)
    {
        if (PhotonNetwork.JoinRoom(roomName))
        {
            print("Entrou na sala com sucesso.");
        }
        else
        {
            print("Join room failed.");
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public void DesativaPainel()
    {
        escolhasSala.gameObject.SetActive(false);
    }

    public void ativaPainel()
    {
        escolhasSala.gameObject.SetActive(true);
    }
}
