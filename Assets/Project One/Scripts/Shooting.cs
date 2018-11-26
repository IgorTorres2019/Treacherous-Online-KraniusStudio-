using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : Photon.MonoBehaviour
{
    public static Shooting instance;

    [SerializeField]
    private Player_Moviment _player_Moviment;
    private Player_Moviment Player_Moviment
    {
        get { return _player_Moviment; }
    }

    [SerializeField]
    private PlayerHealth _playerHealth;
    private PlayerHealth PlayerHealth
    {
        get {return _playerHealth; }
    }

    public int Municao = 100;
    public Text municao;
    public int DamagePerShot = 20;
    public float timeSetShoot = 0.15f;
    public float range = 600f;
    float timer;
    RaycastHit shootHit;
    int shootableMask,IDMatador;
    ParticleSystem weaponParticle;
    LineRenderer weaponLine;
    AudioSource weaponAudio;
    Light weaponLigth;
    float EffectsDysplayTime = 0.2f;
    public bool gatilho = false;
    string Myname;
    public float Tempo = 0;

    private void Awake()
    {
        instance = this;
        shootableMask = LayerMask.GetMask("shootable");
        weaponParticle = GetComponent<ParticleSystem>();
        weaponLigth = GetComponent<Light>();
        weaponAudio = GetComponent<AudioSource>();
        weaponLine = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

       if (Input.GetButton("Fire1") && timer >= timeSetShoot && Municao > 0)
        {
            Shoot();
        }

        if (timer >= timeSetShoot * EffectsDysplayTime)
        {
            DisableEffects();
        }

        if(Municao > 100)
        {
            Municao = 100;
        }
    }

    public void DisableEffects()
    {
        weaponLine.enabled = false;
        weaponLigth.enabled = false;
    }

    public void Shoot()
    {
        print(_player_Moviment.PhotonView.isMine +"É eu que atirei.");
        if (!_player_Moviment.PhotonView.isMine)
            return;
        _player_Moviment.GetComponent<PhotonView>().RPC("EfeitoTiro", PhotonTargets.Others,_player_Moviment.GetComponent<PhotonView>().viewID);

        timer = 0f;
        Municao -= 1;
        weaponAudio.Play();
        weaponLigth.enabled = true;
        weaponParticle.Stop();
        weaponParticle.Play();

        weaponLine.enabled = true;
        weaponLine.SetPosition(0, transform.position);

        Myname = PhotonNetwork.playerName;
        IDMatador = _player_Moviment.photonView.viewID;

        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(camRay,out shootHit,range,shootableMask))
        {
            Debug.DrawLine(transform.position, shootHit.point);

            PlayerHealth PH = shootHit.collider.GetComponent<PlayerHealth>();
            PhotonView PHOTO = shootHit.collider.GetComponent<PhotonView>();
         
            if (PHOTO != null)
            {
                MangePlayer.instance.Damaged = true;
                PH.GetComponent<PhotonView>().RPC("TakeDamage",PhotonPlayer.Find(PHOTO.viewID),DamagePerShot,shootHit.point,PHOTO.viewID,Myname,IDMatador);
                print(PHOTO.viewID);
            }
            else
            {
                Debug.Log("Nao acertou algo que tem o componente PhotonView. 'metodo Shoot Physics => PhotonView PHOTO = PH.GetComponent<PhotonView>(); '");
            }
            weaponLine.SetPosition(1,shootHit.point);
        }
        else
        {
            weaponLine.SetPosition(1,transform.position + camRay.direction * range);
        }
    }

    public void shooth(int ID)
    {
        if (ID != _player_Moviment.photonView.viewID)
            return;
        weaponAudio.Play();
        weaponLigth.enabled = true;
        weaponParticle.Stop();
        weaponParticle.Play();
        DisableEffects();
    }
}
