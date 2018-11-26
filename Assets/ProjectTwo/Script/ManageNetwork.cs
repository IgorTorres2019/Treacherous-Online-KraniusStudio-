using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ManageNetwork : MonoBehaviour
{
    [SerializeField]
    private Button StartGame_btn;

    [SerializeField]
    private Button JoinRandom_btn;

    [SerializeField]
    private InputField inpu_NameRoom;

    [SerializeField]
    private Dropdown dropmaxPlayer,ModoParida,TipodeArmas,PublicOuPrivado;

    [SerializeField]
    private Text infoFinalRoom;

    [SerializeField]
    private Sprite[] Salas = new Sprite[0];

    [SerializeField]
    private Transform[] Spawn = new Transform[0];

    [SerializeField]
    private Image img_salas;
    int nusalas = 0;

    int salasEsc = 0,modoGame = 0, armas = 0;
    bool pubpriv = false;
    byte maxPlay = 0;
    string publi;
    public Text salas, nameRooms;

    private void Update()
    {
        infoFinalRoom.text = inpu_NameRoom.text +" / "+ publi +" / "+ maxPlay.ToString();
        AtualizaServidor();
        MaxP();
        Tipoarma();
        ModoGame();
        PubPrivado();
        passaSala();
    }

    private void AtualizaServidor()
    {
        salas.text ="Roons Online : " + PhotonNetwork.GetRoomList().Length.ToString();

        if (!PhotonNetwork.connected)
        {
            print("Connectando ao servidor..");
            PhotonNetwork.ConnectUsingSettings("1.0");
        }
        if (PhotonNetwork.connected && inpu_NameRoom.text != "")
        {
            StartGame_btn.interactable = true;
        }
        else
        {
            StartGame_btn.interactable = false;
        }

        if (PhotonNetwork.connected)
        {
            JoinRandom_btn.interactable = true;
        }
        else
        {
            JoinRandom_btn.interactable = false;
        }
    }

    private void OnConnectedToMaster()
    {
        print("Conectado ao servidor.");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    private void OnJoinedLobby()
    {
        print("Conectado ao lobby");
    }

    public void CriarRoom()
    {
        RoomOptions RO = new RoomOptions() { isOpen = true, isVisible = pubpriv, MaxPlayers = maxPlay };
        PhotonNetwork.CreateRoom(inpu_NameRoom.text,RO,TypedLobby.Default);
    }

    public void Direita()
    { 
        if(nusalas <= 3)
        {
            nusalas += 1;
        }else if(nusalas > 3)
        {
            nusalas = 0;
        }
    }
    public void Esquerda()
    {
        if (nusalas >= 1)
        {
            nusalas -= 1;
        }
        else if (nusalas < 1)
        {
            nusalas = 4;
        }
    }

    private void passaSala()
    {
        switch (nusalas)
        {
            case 0:
                img_salas.sprite = Salas[nusalas];
                nameRooms.text = "sala01";
                break;
            case 1:
                img_salas.sprite = Salas[nusalas];
                nameRooms.text = "sala02";
                break;
            case 2:
                img_salas.sprite = Salas[nusalas];
                nameRooms.text = "sala03";
                break;
            case 3:
                img_salas.sprite = Salas[nusalas];
                nameRooms.text = "sala04";
                break;
            case 4:
                img_salas.sprite = Salas[nusalas];
                nameRooms.text = "sala05";
                break;
        }
    }

    private void OnCreatedRoom()
    {
        print("Sala Criada com sucesso.");
    }

    private void OnJoinedRoom()
    {
        PhotonNetwork.Instantiate("VanGuard", Spawn[0].transform.position,Quaternion.identity,0);
        ManageCanvas.instance.DesativaPainel();
    }

    public void EntrarRandom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    private void MaxP()
    {
        switch (dropmaxPlayer.value)
        {
            case 0:
                maxPlay = 4;
                break;

            case 1:
                maxPlay = 8;
                break;

            case 2:
                maxPlay = 12;
                break;

            case 3:
                maxPlay = 16;
                break;

            case 4:
                maxPlay = 20;
                break;
        }
    }
    private void ModoGame()
    {
        switch (ModoParida.value)
        {
            case 0:
                modoGame = 0;
                break;
            case 1:
                modoGame = 1;
                break;
            case 2:
                modoGame = 2;
                break;
        }
    }

    private void Tipoarma()
    {
        switch (TipodeArmas.value)
        {
            case 0:
                armas = 0;
                break;
            case 1:
                armas = 1;
                break;
            case 2:
                armas = 2;
                break;
        }
    }

    private void PubPrivado()
    {
        switch (PublicOuPrivado.value)
        {
            case 0:
                pubpriv = true;
                publi = "Public";
                break;
            case 1:
                pubpriv = false;
                publi = "Private";
                break;
        }
    }

    public void sairGame()
    {
        Application.Quit();
    }
}
