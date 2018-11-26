using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelecaoDePlayers : MonoBehaviour
{
    [SerializeField]
    private Image image_Perso;
    [SerializeField]
    public Text txt_NamePerso;
    [SerializeField]
    private Sprite[] sprites_persos;
    private int persos = 0;

    public void Direita()
    {
        if (persos <= 3)
        {
            persos += 1;
        }
        else if (persos > 3)
        {
            persos = 0;
        }
    }
    public void Esquerda()
    {
        if (persos >= 1)
        {
            persos -= 1;
        }
        else if (persos < 1)
        {
            persos = 4;
        }
    }

    private void Update()
    {
        Escolhas();
    }

    private void Escolhas()
    {
        switch (persos)
        {
            case 0:
                txt_NamePerso.text = "VanGuard";
                image_Perso.sprite = sprites_persos[0];
                SetPlayer();
                break;
            case 1:
                txt_NamePerso.text = "Capitao";
                image_Perso.sprite = sprites_persos[1];
                SetPlayer();
                break;
            case 2:
                txt_NamePerso.text = "VanGuardMini";
                image_Perso.sprite = sprites_persos[2];
                SetPlayer();
                break;
            case 3:
                txt_NamePerso.text = "Gigantus";
                image_Perso.sprite = sprites_persos[3];
                SetPlayer();
                break;
            case 4:
                txt_NamePerso.text = "Macaco";
                image_Perso.sprite = sprites_persos[4];
                SetPlayer();
                break;
        }
    }

    private void SetPlayer()
    {
        MangePlayer.instance.ApplayPersonagem();
    }



}
