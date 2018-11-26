using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject Cronometro;

    [SerializeField]
    private Camera cameraMain,CameraAuxi;

    [SerializeField]
    private Button StartGame_btn, botaoSpawnar;

    [SerializeField]
    private Button JoinRandom_btn;

    [SerializeField]
    private InputField inpu_NameRoom;

    [SerializeField]
    private Dropdown dropmaxPlayer, ModoParida, TipodeArmas, PublicOuPrivado , GrupoEscolha;

    [SerializeField]
    private Text infoFinalRoom;

    [SerializeField]
    private Sprite[] Salas = new Sprite[0];

    [SerializeField]
    private Transform[] SpawnGrupo = new Transform[0];

    public int GrupoA = 9,GrupoB = 19;

    int randEsc;

    [SerializeField]
    private Image img_salas;

    [SerializeField]
    private Text[] MensagInfo;

    public static Manager instance;
   
    GameObject Advers_EmRede;
    public GameObject[] Paineis = new GameObject[0];

    public Slider sliderTemp;

    public InputField MensagensEnvio;
    public bool sair = false;
    public Text mens;
    public Text[] mortoEvido;
    public Text Municao;
    public Image IconeArma;
    float tempo = 0f, tempo2 = 0, tempo3 = 0, temp4 = 0;
    bool kill = false, playerconect = false,morte = false, spawnTemp = false;
    bool msg = false, envia = false;
    PlayerHealth playerHealth;
    int nusalas = 0;
    int salasEsc = 0, modoGame = 0, armas = 0;
    bool pubpriv = false;
    byte maxPlay = 0;
    string publi;
    public Text salas, nameRooms;
    GameObject[] posCamera;

    public string PersoName;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        PhotonNetwork.Disconnect();
        int ran = Random.Range(0,9999);
        if(SaveDados.instance.NamePlayerCarregado == "")
        {
            SaveDados.instance.NamePlayerCarregado = "Visit: " + ran.ToString();
        }

        print("Connectando ao Servidor...");
        mens.text = "Deseja realmente sair? ";
    }

    public void EntrarSala()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void CriarRoom()
    {
        RoomOptions RO = new RoomOptions() { isOpen = true, isVisible = pubpriv, MaxPlayers = maxPlay };
        PhotonNetwork.CreateRoom(inpu_NameRoom.text, RO, TypedLobby.Default);
    }

    private void OnConnectedToMaster()
    {
        print("Connectado ao Servidor.");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
        StartGame_btn.interactable = true;
        JoinRandom_btn.interactable = true;
    }

    public void InstanciarNaSala()
    {
        GameObject player = (PhotonNetwork.Instantiate(PersoName, SpawnGrupo[randEsc].transform.position, Quaternion.identity, 0));

        PlayerHealth pp = player.GetComponent<PlayerHealth>();
        pp.HealthSlider = Paineis[5].GetComponent<Slider>();
        pp.DanoImage = Paineis[6].GetComponent<Image>();
        pp.municao = Municao.GetComponent<Text>();
        Paineis[0].gameObject.SetActive(false);

        Player_Moviment pm = player.GetComponent<Player_Moviment>();
        CameraVisao.instance.Cabeça[0] = pm.referencias[1].gameObject;
        CameraVisao.instance.Cabeça[1] = pm.referencias[2].gameObject;
        CameraVisao.instance.pos[0] = pm.referencias[3].gameObject;
        CameraVisao.instance.pos[1] = pm.referencias[4].gameObject;

        CamAuxio.instance.visaoT = pm.referencias[0].gameObject;
        CamAuxio.instance.CabecaM = pm.referencias[5].gameObject;


        cameraMain.enabled = true;
        CameraVisao.instance.enabled = true;
        CamAuxio.instance.enabled = true;
        Paineis[9].gameObject.SetActive(false);
    }

    private void OnJoinedRoom()
    {
        Paineis[1].gameObject.SetActive(false);
        Paineis[9].gameObject.SetActive(true);
    }

    public void LeaveRoom()
    {   
        Paineis[1].gameObject.SetActive(true);
        Paineis[9].gameObject.SetActive(false);
        CameraVisao.instance.enabled = false;
        cameraMain.transform.SetParent(Paineis[7].transform, false);
        CamAuxio.instance.enabled = false;
        CameraAuxi.transform.SetParent(Paineis[7].transform, false);
        CameraAuxi.gameObject.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }

    public void SpawnarDenovo()
    {
        ManageCanvas.instance.ativaPainel();

        GameObject player = (PhotonNetwork.Instantiate(PersoName, SpawnGrupo[randEsc].transform.position, Quaternion.identity, 0));
        PlayerHealth pp = player.GetComponent<PlayerHealth>();
        pp.HealthSlider = Paineis[5].GetComponent<Slider>();
        pp.DanoImage = Paineis[6].GetComponent<Image>();
        pp.municao = Municao.GetComponent<Text>();
        Paineis[0].gameObject.SetActive(false);

        Player_Moviment pm = player.GetComponent<Player_Moviment>();
        CameraVisao.instance.Cabeça[0] = pm.referencias[1].gameObject;
        CameraVisao.instance.Cabeça[1] = pm.referencias[2].gameObject;
        CameraVisao.instance.pos[0] = pm.referencias[3].gameObject;
        CameraVisao.instance.pos[1] = pm.referencias[4].gameObject;

        CamAuxio.instance.visaoT = pm.referencias[0].gameObject;
        CamAuxio.instance.CabecaM = pm.referencias[5].gameObject;

        CameraVisao.instance.enabled = true;
        CamAuxio.instance.enabled = true;

        botaoSpawnar.interactable = false;
        Paineis[8].gameObject.SetActive(false);
    }

    private void OnCreatedRoom()
    {
        print("Sala Criada com Sucesso.");
    }

    private void OnJoinedLobby()
    {
        print("Conectado ao lobby");
    }

    private void OnPhotonCreateRoomFailed()
    {
        MensagInfo[0].text = "Falha ao Criar Sala. Algo saiu errado !";
    }

    private void OnPhotonRandomJoinFailed()
    {
        MensagInfo[0].text = "There are currently no active rooms, create one right now!";
        JoinRandom_btn.interactable = false;
    }

    private void AtualizaServidor()
    {
        salas.text = "Roons Online : " + PhotonNetwork.GetRoomList().Length.ToString();

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

    private void Update()
    {
        infoFinalRoom.text = inpu_NameRoom.text + " / " + publi + " / " + maxPlay.ToString();
        AtualizaServidor();
        MaxP();
        Tipoarma();
        ModoGame();
        PubPrivado();
        passaSala();
        Grupos();

        PhotonNetwork.playerName = SaveDados.instance.NamePlayerCarregado;
        MensagInfo[1].text = SaveDados.instance.NamePlayerCarregado;

        if (kill)
        {
            conometro();
        }

        if (playerconect)
        {
            conometroInfoPlayerConnected();
        }

        if (morte)
        {
            CronometroMorreu();
        }

        if (spawnTemp)
        {
            CronometroSpawn();
        }
    }

    public void SairDaSala()
    {
        Paineis[3].gameObject.SetActive(false);
        PhotonNetwork.LoadLevel(0);
    }

    public void Kills(string matador, string morto)
    {
        mortoEvido[0].text = matador;
        IconeArma.gameObject.SetActive(true);
        mortoEvido[1].text = morto;
        kill = true;
    }

    void conometro()
    {
        tempo += Time.deltaTime;
        if (tempo >= 5f)
        {
            mortoEvido[0].text = "";
            IconeArma.gameObject.SetActive(false);
            mortoEvido[1].text = "";
            tempo = 0f;
            kill = false;
        }
    }

    public void SairGame()
    {
        Application.Quit();
    }

    public void EnviarMensagens()
    {
        Player_Moviment.instance.Enviar(PhotonNetwork.playerName,MensagensEnvio.text);
        MensagensEnvio.text = "";
        print("Foi enviada");
    } 

    public void Escrevendo()
    {
        Player_Moviment.instance.esc(SaveDados.instance.NamePlayerCarregado);
    } 

    private void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        print("Entrou na sala: " + player);
        MensagInfo[2].text = player +"\r\n" +"Entrou na Partida";
        playerconect = true;
    }

    private void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        print("Entrou na sala: " + player);
        MensagInfo[2].text = player +"\r\n" +"Saiu da Partida";
        playerconect = true;
    }


    void conometroInfoPlayerConnected()
    {
        tempo2 += Time.deltaTime;
        if (tempo2 >= 5f)
        {
            MensagInfo[2].text = "";
            tempo2 = 0f;
            playerconect = false;
        }
    }
    public void moorreu()
    {
        morte = true;
        ManageCanvas.instance.DesativaPainel();
    }

    void CronometroMorreu()
    {
        tempo3 += Time.deltaTime;
        if (tempo3 >= 2f)
        {
            Paineis[8].gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            morte = false;
            spawnTemp = true;
            tempo3 = 0f;
        }
    }

    void CronometroSpawn()
    {
        temp4 += Time.deltaTime;
        sliderTemp.value = temp4;
        if(temp4 >= 10f)
        {
            sliderTemp.value = 10;
            botaoSpawnar.interactable = true;
            spawnTemp = false;
            temp4 = 0f;
        }
    }

    public void Direita()
    {
        if (nusalas <= 3)
        {
            nusalas += 1;
        }
        else if (nusalas > 3)
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

    private void MaxP()
    {
        switch (dropmaxPlayer.value)
        {
            case 0:
                maxPlay = 2;
                break;

            case 1:
                maxPlay = 4;
                break;

            case 2:
                maxPlay = 8;
                break;

            case 3:
                maxPlay = 12;
                break;

            case 4:
                maxPlay = 16;
                break;
            case 5:
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

    private void Grupos()
    {
        switch (GrupoEscolha.value)
        {
            case 0:
                int randA = Random.Range(0, GrupoA);
                randEsc = randA;
                break;
            case 1:
                int randB = Random.Range(10, GrupoB);
                randEsc = randB;
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

    public void Cronometragem()
    {
        Instantiate(Cronometro, transform.position,Quaternion.identity);
    }
}
