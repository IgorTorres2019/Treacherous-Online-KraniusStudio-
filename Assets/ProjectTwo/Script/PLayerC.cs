using UnityEngine;

public class PLayerC : Photon.MonoBehaviour
{
    public static PLayerC instance;
    public bool UseTransformView = true;
    public bool Equipado = false, abaixado = false;


    [SerializeField]
    private PlayerHealth _playerHealth;
    private PlayerHealth PlayerHealth
    {
        get { return _playerHealth; }
    }

    [SerializeField]
    private Shooting _shooting;
    private Shooting Shooting
    {
        get { return _shooting; }
    }

    [SerializeField]
    private GameObject[] Armas;

    float vel = 6f, velrot = 100, MoveY, MoveX, H = 2.0f;

    public PhotonView PhotonView;
    private Vector3 TargetPosition;
    private Quaternion TargetRotation;

    ManageChat _manageChat;



    Rigidbody Rb;
    Animator anim;
   
    private void Awake()
    {
        instance = this;
        PhotonView = GetComponent<PhotonView>();
        anim = GetComponent<Animator>();
        Rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (PhotonView.isMine)
        {
            MoverPlayer();
        }
        else
        {
            SmothMove();
        }
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (UseTransformView)
            return;

        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            TargetPosition = (Vector3)stream.ReceiveNext();
            TargetRotation = (Quaternion)stream.ReceiveNext();
        }
    }

    private void SmothMove()
    {
        if (UseTransformView)
            return;
        transform.position = Vector3.Lerp(transform.position, TargetPosition, 0.25f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, TargetRotation, 500 * Time.deltaTime);
    }

    void MoverPlayer()
    {
        float rotaçao = Input.GetAxis("Horizontal") * velrot;
        float Mover = Input.GetAxis("Vertical") * vel;
        float Hor = H * Input.GetAxis("Mouse X");

        rotaçao *= Time.deltaTime;
        Mover *= Time.deltaTime;

        transform.Rotate(0, Hor, 0);

        MoveY = Input.GetAxis("Vertical");
        MoveX = Input.GetAxis("Horizontal");

        anim.SetFloat("X", MoveX, 0.1f, Time.deltaTime);
        anim.SetFloat("Y", MoveY, 0.1f, Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Equipado)
            {
                photonView.RPC("DesEquipaArma", PhotonTargets.AllBuffered);
                Equipado = false;
            }
            else if (!Equipado)
            {
                photonView.RPC("EquipaArma", PhotonTargets.AllBuffered);
                Equipado = true;
            }
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("Running", true);
        }
        else
        {
            anim.SetBool("Running", false);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (abaixado && !Equipado)
            {
                anim.SetTrigger("abaixado");
                photonView.RPC("abaixa", PhotonTargets.AllBuffered);
                abaixado = false;
            }
            else if(!abaixado && Equipado)
            {
                anim.SetTrigger("Equip");
                photonView.RPC("Levanta", PhotonTargets.AllBuffered);
                abaixado = true;
            }
        }
        

    }
    public void esc(string nameplayer)
    {
        photonView.RPC("Escrevendo", PhotonTargets.Others,nameplayer);
        print("chamou o metodo esc");
    }
    public void Enviar(string name, string msg)
    {
        photonView.RPC("EnviarMSG", PhotonTargets.All, name, msg);
    }

    [PunRPC]
    public void EnviarMSG(string name, string mensagem)
    {
        ManageChat.instance.ApplyMensagem(name, mensagem);
        print("Ta chamando sim!");
    }

    [PunRPC]
    public void Dead()
    {
        _playerHealth.Morreu();
    }

    [PunRPC]
    public void EquipaArma()
    {
        anim.SetTrigger("Equip");
        Armas[1].gameObject.SetActive(true);
        Armas[0].gameObject.SetActive(false);
    }
    [PunRPC]
    public void DesEquipaArma()
    {
        anim.SetTrigger("DesEquip");
        Armas[1].gameObject.SetActive(false);
        Armas[0].gameObject.SetActive(true);
    }

    [PunRPC]
    public void abaixa()
    {
        anim.SetTrigger("abaixado");
    }

    [PunRPC]
    public void Levanta()
    {
        anim.SetTrigger("Equip");
    }

    [PunRPC]
    public void killeds(string matador, string morto, int Arma)
    {
        MangePlayer.instance.Kills(matador, morto, Arma);
    }

    [PunRPC]
    public void EfeitoTiro(int id)
    {
        _shooting.shooth(id);
    }
}
