using UnityEngine;
using UnityEngine.UI;

public class ManageChat : MonoBehaviour
{
    public static ManageChat instance;

    [SerializeField]
    private Scrollbar scrollbar;
    [SerializeField]
    RectTransform layoutMsg;
    public InputField txt_mensagem;

    [SerializeField]
    private GameObject _mensageChatPrefab;
    private GameObject MensageChatPrefab
    {
        get { return _mensageChatPrefab; }
    }

    private void Awake()
    {
        instance = this;
       
    }
    private void Update()
    {
        atualizachat(); // atualiza o método .
    }
    // controla a quantidade de mensagens existentes para nao acumular alem de 15 mensagens.
    private void atualizachat()
    {
        int Nmsg = layoutMsg.transform.childCount; // verifica a quantidade existente de mensagem.
        if(Nmsg > 15)
        {
            Destroy(layoutMsg.transform.GetChild(0).gameObject); // destroi o primeiro gameobject da mensagem enviada.
        }
    }

    // seta a mensagem escrita no prefab e o instancia no chat.
    public void ApplyMensagem(string Name, string msg)
    {
        _mensageChatPrefab.GetComponent<MsgPrefab>().EnviarMensagem(Name, msg); // set a mensagem.
        GameObject _prefabMsg = Instantiate(_mensageChatPrefab); // instancia a mensagem .
        _prefabMsg.transform.SetParent(layoutMsg, false); // seta como parente do containe de mensagens.
        Manager.instance.Paineis[4].gameObject.SetActive(true);
        scrollbar.value = 0; // desce o scrol da mensagem pra visualizar as novas mensagens recebidas.
    }
}
