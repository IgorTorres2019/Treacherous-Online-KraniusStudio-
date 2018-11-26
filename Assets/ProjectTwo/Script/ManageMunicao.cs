using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManageMunicao : MonoBehaviour
{
    [SerializeField]
    private Image _imageArma;
    public Image ImageArma
    {
        get { return _imageArma; }
    }

    [SerializeField]
    private Text _text_Municao;
    public Text Text_Municao
    {
        get { return _text_Municao; }
    }

    [SerializeField]
    private Text _text_Pente;
    public Text Text_Pente
    {
        get { return _text_Pente; }
    }

    [SerializeField]
    private Sprite[] ArmasEquip = new Sprite[0];
}
