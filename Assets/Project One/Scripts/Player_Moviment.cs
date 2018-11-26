using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Moviment : Photon.MonoBehaviour
{
    public static Player_Moviment instance;

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

    public GameObject[] referencias;

    public PhotonView PhotonView;
    public bool UseTransformView = true;

    private Vector3 TargetPosition;
    private Quaternion TargetRotation;

    public float jumpImpulso = 2f;
    public float Forca = 65, tempodePulo = 0f;
    bool podePular = true;
    float vel = 6f, velrot = 100,MoveY,MoveX,H = 2.0f;
    public bool Equipado = false, DesEquipado = true;

    Rigidbody Rb;
    Animator anim;
    int FloorMask;
    RaycastHit hit;

    [SerializeField]
    private GameObject[] Posicoes;

    private void Awake()
    {
        instance = this;
        PhotonView = GetComponent<PhotonView>();
        FloorMask = LayerMask.GetMask("shootable");
        anim = GetComponent<Animator>();
        Rb = GetComponent<Rigidbody>();
        
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Manager.instance.Paineis[3].gameObject.SetActive(false);
        if (PhotonNetwork.isMasterClient)
        {
            photonView.RPC("TempoDeJogo", PhotonTargets.MasterClient);
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

    void Update()
    {
        if (PhotonView.isMine)
        {
            MoverPlayer();
            Jump();
        }
        else
        {
            SmothMove();
        }
     
    }

    void MoverPlayer()
    {
        tempodePulo += Time.deltaTime;

        float rotaçao = Input.GetAxis("Horizontal") * velrot;
        float Mover = Input.GetAxis("Vertical") * vel;
        float Hor = H * Input.GetAxis("Mouse X"); 

        rotaçao *= Time.deltaTime;
        Mover *= Time.deltaTime;

        transform.Rotate(0, Hor, 0);
        
        MoveY = Input.GetAxis("Vertical");
        MoveX = Input.GetAxis("Horizontal");
  
        anim.SetFloat("X", MoveX);
        anim.SetFloat("Y", MoveY);

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (Equipado)
            {
                photonView.RPC("DesEquipaArma", PhotonTargets.OthersBuffered);
                anim.SetTrigger("DesEquip");
                Posicoes[1].gameObject.SetActive(false);
                Posicoes[0].gameObject.SetActive(true);
                Equipado = false;
                DesEquipado = true;

            }
            else if(DesEquipado)
            {
                photonView.RPC("EquipaArma", PhotonTargets.OthersBuffered);
                anim.SetTrigger("Equip");
                Posicoes[1].gameObject.SetActive(true);
                Posicoes[0].gameObject.SetActive(false);
                DesEquipado = false;
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

        if (Input.GetKeyDown(KeyCode.Space) && podePular && tempodePulo > 1f)
        {
            Rb.AddForce(Vector3.up * Forca, ForceMode.Impulse);
            tempodePulo = 0f;
            podePular = false;
        }
    }

    void Jump()
    {
        if (Physics.Linecast(transform.position, transform.position - Vector3.up ,FloorMask))
        {
            print("está encostando!");
            podePular = true;
            Debug.DrawLine(transform.position, transform.position - Vector3.up, Color.green);
        }
        else if (!Physics.Linecast(transform.position, transform.position - Vector3.up, FloorMask))
        {
            print("Nao ta encostando!");
            podePular = false;
            Debug.DrawRay(transform.position, transform.TransformDirection(-Vector3.up) * 0.1f, Color.green);
        }
    }

    [PunRPC]
    public void TempoDeJogo()
    {
        Manager.instance.Cronometragem();
    }

    public void Enviar(string name, string msg)
    {
        photonView.RPC("EnviarMSG", PhotonTargets.All, name, msg);
    }

    [PunRPC]
    public void Morri(string Name)
    {
        MangePlayer.instance.morreu(Name);
    }

    [PunRPC]
    public void AtribuiPontos(string matador)
    {
        MangePlayer.instance.Kill(matador, 1);
    }

    [PunRPC]
    public void EnviarMSG(string name, string mensagem)
    {
        ManageChat.instance.ApplyMensagem(name, mensagem);
        print("Ta chamando sim!");
    }

    public void esc(string nameplayer)
    {
        photonView.RPC("Escrevendo", PhotonTargets.Others, nameplayer);
        print("chamou o metodo esc");
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
        Posicoes[1].gameObject.SetActive(true);
        Posicoes[0].gameObject.SetActive(false);
    }
    [PunRPC]
    public void DesEquipaArma()
    {
        anim.SetTrigger("DesEquip");
        Posicoes[1].gameObject.SetActive(false);
        Posicoes[0].gameObject.SetActive(true);
    }

    [PunRPC]
    public void killeds(string matador,string morto)
    {
        Manager.instance.Kills(matador, morto);
    }

    [PunRPC]
    public void EfeitoTiro(int id)
    {
        _shooting.shooth(id);
    }
}
