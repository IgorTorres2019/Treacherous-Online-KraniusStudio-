using UnityEngine;
using UnityEngine.UI;

public class MangePlayer : MonoBehaviour
{
    public static MangePlayer instance;
    public bool Chat = false, Sair = false, locked = false;
    float Tempo = 0f;
    int Limite = 0;
    float tempoMsg = 0f;
    bool podeMandar = true;
    public bool Damaged = false;
    public Color FlashColor = new Color(1f, 0f, 0f, 0.3f);
    public float FlashSpeed = 5f;
    public Image MiraImage;
    public GameObject posicaoPainel;

    public GameObject PainelDePontos;

    [SerializeField]
    private MangeLifePlayer _mangeLife;
    private MangeLifePlayer MangeLifePlayer
    {
        get { return _mangeLife; }
    }

    [SerializeField]
    private ManageChat _manageChat;
    private ManageChat ManageChat
    {
        get { return _manageChat; }
    }

    [SerializeField]
    private ManageKills _manageKills;
    private ManageKills ManageKills
    {
        get { return _manageKills; }
    }

    [SerializeField]
    private ManageMunicao _manageMunicao;
    private ManageMunicao ManageMunicao
    {
        get { return _manageMunicao; }
    }

    [SerializeField]
    private SelecaoDePlayers selecaoDePlayers;
    private SelecaoDePlayers SelecaoDePlayers
    {
        get { return selecaoDePlayers; }
    }

    [SerializeField]
    private LayoutPlayers layoutPlayers;
    private LayoutPlayers LayoutPlayers
    {
        get { return layoutPlayers; }
    }

    public static string NamePlayer;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        
        tempoMsg += Time.deltaTime;
        Comandos(); // atualiza o médoto .
        if (locked)
        {
            Lockmouse();
        }

        if (Damaged)
        {
            MiraImage.color = FlashColor;
        }
        else
        {
            MiraImage.color = Color.Lerp(MiraImage.color, Color.clear, FlashSpeed * Time.deltaTime);
        }
        Damaged = false;

    }

    public void EnviarMensagens()
    {
        if(tempoMsg <= 5 && podeMandar)
        {
            // envia a mensagem que foi escrita no input atravez de um rpc para todos na sala inclusive a si mesmo.
            PLayerC.instance.Enviar(NamePlayer, _manageChat.txt_mensagem.text);
            Limite += 1;
            if(tempoMsg <=10 && Limite >= 3)
            {
                podeMandar = false;
                tempoMsg = 0f;
            }
        }else if(tempoMsg >= 60)
        {
            podeMandar = true;
            Limite = 0;
            tempoMsg = 0f;
        }

        if(tempoMsg >= 5 && podeMandar)
        {
            tempoMsg = 0f;
            Limite = 0;
        }
    }

    public void Kill(string Name,int Pontos)
    {
        layoutPlayers.setaPontos(Name, Pontos);
    }

    public void morreu(string Name)
    {
        layoutPlayers.Setamorte(Name);
    }

     // seta na tela quem matou e quem foi morto. exibe tbm a arma usada.
    public void Kills(string matador,string morto,int arma)
    {
        ManageKills.ApplyKills(matador, morto, arma); // chama o método passando os parametros recebidos.
    }

    private void Comandos() // comandos para controlar a tela de Uis.
    {
        if (Input.GetKeyDown(KeyCode.T)) // exibe o chat na tela .
        {
            if (!Chat)
            {
                Cursor.lockState = CursorLockMode.None;
                _manageChat.gameObject.SetActive(true); 
                Chat = true;
            }
            else
            {
                locked = true;
                _manageChat.gameObject.SetActive(false); // esconde o chat.
                Chat = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Sair)
            {
                Cursor.lockState = CursorLockMode.None;
                Manager.instance.Paineis[3].gameObject.SetActive(true);
                Sair = true;
            }
            else
            {
                locked = true;
                Manager.instance.Paineis[3].gameObject.SetActive(false);
                Sair = false;
            }
        }

        if (Input.GetKey(KeyCode.Tab))
        {
            PainelDePontos.transform.position = MiraImage.transform.position;
        }
        else
        {
            PainelDePontos.transform.position = posicaoPainel.transform.position;
        }
    }

    public void Lockmouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        locked = false;
    }
    
    public void ApplayPersonagem()
    {
        Manager.instance .PersoName = selecaoDePlayers.txt_NamePerso.text;
    }
}
