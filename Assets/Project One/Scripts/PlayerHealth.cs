using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    [SerializeField]
    private Player_Moviment _player_moviment;
    private Player_Moviment Player_Moviment
    {
        get { return _player_moviment; }
    }

    [SerializeField]
    private Shooting _shooting;
    private Shooting Shooting
    {
        get { return _shooting; }
    }
    public GameObject PrefabMorto;
    public static PlayerHealth instance;
    public int StartingHealth = 100;
    public float sinkSpeed = 2.5f;
    public int CurrentHealth;
    public Slider HealthSlider;
    public Image DanoImage;
    public AudioClip DeadClip;
    public float FlashSpeed = 5f;
    public GameObject IsMinekk;
    public GameObject[] Huds;
    public Color FlashColor = new Color(1f,0f,0f,0.1f);
    public Color Cornormal = new Color(0f, 0f, 0f, 0f);
    Animator Anim;
    AudioSource PlayAudio;
    Player_Moviment player_Moviment;
    ParticleSystem particulas;
    bool IsDead,Damaged;
    public Text municao;
    CapsuleCollider cap;

    private void Awake()
    {
        instance = this;
        cap = GetComponent<CapsuleCollider>();
        particulas = GetComponentInChildren<ParticleSystem>();
        Anim = GetComponent<Animator>();
        PlayAudio = GetComponent<AudioSource>();
        player_Moviment = GetComponent<Player_Moviment>();
        CurrentHealth = StartingHealth;
    }

    private void Update()
    {
        if (Damaged)
        {
            DanoImage.color = FlashColor;
        }
        else
        {
            DanoImage.color = Color.Lerp(DanoImage.color, Color.clear, FlashSpeed * Time.deltaTime);
        }
        Damaged = false;
        municao.text = Shooting.Municao.ToString() +"/100";
        if(CurrentHealth > 100)
        {
            CurrentHealth = 100;
        }
        HealthSlider.value = CurrentHealth;
    }

    [PunRPC]
    public void TakeDamage(int amount, Vector3 hitPoint,int MeuID,string matador,int IDMatador)
    {
        PhotonView MeuPhotonView = GetComponent<PhotonView>();
        print(MeuPhotonView.viewID + "TakeDamage, Meu Id Local");
        if (MeuID != MeuPhotonView.viewID)
            return;

        print("Me acertou sim. " + MeuID);
        if (IsDead)
            return;

        Damaged = true;
        CurrentHealth -= amount;
        particulas.transform.position = hitPoint;
        particulas.Play();

        PlayAudio.Play();

        if(CurrentHealth <= 0 && !IsDead)
        {
            Death(matador,IDMatador);
        }
    }

    private void Death(string matador,int IDMatador)
    {
        Shooting.enabled = false;
        HealthSlider.value = 0;
        player_Moviment.GetComponent<PhotonView>().RPC("Dead", PhotonTargets.AllBuffered);
        player_Moviment.GetComponent<PhotonView>().RPC("AtribuiPontos",PhotonTargets.AllBuffered,matador);
        player_Moviment.GetComponent<PhotonView>().RPC("Morri", PhotonTargets.AllBuffered, SaveDados.instance.NamePlayerCarregado);
        player_Moviment.GetComponent<PhotonView>().RPC("killeds", PhotonTargets.All, matador, SaveDados.instance.NamePlayerCarregado);
        IsDead = true;
        PlayAudio.clip = DeadClip;
        PlayAudio.Play();
        CameraVisao.instance.transform.SetParent(Manager.instance.Paineis[7].transform);
        CamAuxio.instance.transform.SetParent(Manager.instance.Paineis[7].transform);
        CamAuxio.instance.enabled = false;
        CameraVisao.instance.enabled = false;
        Manager.instance.moorreu();
        player_Moviment.enabled = false;

    }

    public void Morreu()
    {
        Instantiate(PrefabMorto, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
