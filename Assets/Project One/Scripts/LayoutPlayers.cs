using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LayoutPlayers : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerListingPrefab;
    private GameObject PlayerListingPrefab
    {
        get { return _playerListingPrefab; }
    }

    private List<ListPlayers> _listPlayers = new List<ListPlayers>();
    private List<ListPlayers> ListPlayers
    {
        get { return _listPlayers; }
    }

    private void OnJoinedRoom()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        PhotonPlayer[] photonPlayer = PhotonNetwork.playerList;
        for (int i = 0; i < photonPlayer.Length; i++)
        {
            PlayerJoinedRoom(photonPlayer[i]);
        }
    }

    private void OnPhotonPlayerConnected(PhotonPlayer photonPlayer)
    {
        PlayerJoinedRoom(photonPlayer);
    }

    private void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer)
    {
        PlayerLeftRoom(photonPlayer);
    }

    private void PlayerJoinedRoom(PhotonPlayer photonPlayer)
    {
        if (photonPlayer == null)
            return;
        PlayerLeftRoom(photonPlayer);

        GameObject listPlayersObj = Instantiate(PlayerListingPrefab);
        listPlayersObj.transform.SetParent(transform, false);

        ListPlayers listPlayers = listPlayersObj.GetComponent<ListPlayers>();
        listPlayers.ApplyPhotonPlayer(photonPlayer);

        ListPlayers.Add(listPlayers);
    }

    private void PlayerLeftRoom(PhotonPlayer photonPlayer)
    {
        int index = ListPlayers.FindIndex(x => x.PhotonPayer == photonPlayer);
        if (index != -1)
        {
            Destroy(ListPlayers[index].gameObject);
            ListPlayers.RemoveAt(index);
        }
    }

    public void setaPontos(string Name,int Ponto)
    {
        Transform OndeVaiPontos = GameObject.Find(Name).transform;
        OndeVaiPontos.GetComponent<ListPlayers>().SetaPontosNoPrefab(Ponto);
    }

    public void Setamorte(string Name)
    {
        Transform Morte = GameObject.Find(Name).transform;
        Morte.GetComponent<ListPlayers>().SetaMorte();
    }
}
