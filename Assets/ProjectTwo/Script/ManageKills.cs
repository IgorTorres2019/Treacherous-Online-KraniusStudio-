using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageKills : MonoBehaviour
{
    float tempo = 0f;
    bool Kills = false;

    [SerializeField]
    private Text _text_Matador;
    public Text Text_Matador
    {
        get { return _text_Matador; }
    }

    [SerializeField]
    private Text _text_Morto;
    public Text Text_Morto
    {
        get { return _text_Morto; }
    }

    [SerializeField]
    private Image _armaImage;
    public Image ArmaImage
    {
        get { return _armaImage; }
    }

    [SerializeField]
    private Sprite[] Armas = new Sprite[0];

    private void Update()
    {
        if (Kills)
        {
            conometro();
        }
    }

    public void ApplyKills(string matador,string morto, int Arma)
    {
        _text_Matador.text = matador;
        _text_Morto.text = morto;
        _armaImage.sprite = Armas[Arma];
        _armaImage.gameObject.SetActive(true);
        Kills = true;
    }

    void conometro()
    {
        tempo += Time.deltaTime;
        if (tempo >= 5f)
        {
            _text_Matador.text = "";
            _text_Morto.text = "";
            _armaImage.gameObject.SetActive(false);
            tempo = 0f;
            Kills = false;
        }
    }
}
