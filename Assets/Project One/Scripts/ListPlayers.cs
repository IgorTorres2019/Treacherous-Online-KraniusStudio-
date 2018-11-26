using UnityEngine;
using UnityEngine.UI;

public class ListPlayers : MonoBehaviour
{
    public PhotonPlayer PhotonPayer { get; private set; }

    public int Pontos = 0,morte = 0;

    [SerializeField]
    private Text _playerName;
    private Text PlayerName
    {
        get { return _playerName; }
    }

    [SerializeField]
    private Text _pontosDeKill;
    private Text PontosDeKill
    {
        get { return _pontosDeKill; }
    }

    [SerializeField]
    private Text _dead;
    private Text Dead
    {
        get { return _dead; }
    }

    [SerializeField]
    private Image _clan;
    private Image Clan
    {
        get { return _clan; }
    }

    public void ApplyPhotonPlayer(PhotonPlayer photonPlayer)
    {
        PhotonPayer = photonPlayer;
        PlayerName.text = photonPlayer.NickName;
        name = photonPlayer.NickName;
    }

    public void SetaPontosNoPrefab(int pontos)
    {
        Pontos += pontos;
        _pontosDeKill.text = Pontos.ToString();
    }

    public void SetaMorte()
    {
        morte += 1;
        _dead.text = morte.ToString();
    }
}
