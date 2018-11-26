using UnityEngine;
using UnityEngine.UI;

public class MsgPrefab : MonoBehaviour
{
    [SerializeField]
    private Text _msgRecebida;
    private Text MsgRecebida
    {
        get { return _msgRecebida; }
    }

    public void EnviarMensagem(string Name,string Msg)
    {
        if(Msg == "/Color7600")
        {
            _msgRecebida.color = Color.blue;
            Msg = "Ok";
        }
        if(Msg == "/Color7601")
        {
            _msgRecebida.color = Color.green;
            Msg = "Ok";
        }
        if (Msg == "/Color7602")
        {
            _msgRecebida.color = Color.white;
            Msg = "Ok";
        }
        if (Msg == "/Color7603")
        {
            _msgRecebida.color = Color.black;
            Msg = "Ok";
        }
        if (Msg == "/Color7604")
        {
            _msgRecebida.color = Color.gray;
            Msg = "Ok";
        }
        if (Msg == "/Color7605")
        {
            _msgRecebida.color = Color.red;
            Msg = "Ok";
        }
        if (Msg == "/Color7606")
        {
            _msgRecebida.color = Color.yellow;
            Msg = "Ok";
        }
        _msgRecebida.text = Name + ": " + Msg;
    }


}
