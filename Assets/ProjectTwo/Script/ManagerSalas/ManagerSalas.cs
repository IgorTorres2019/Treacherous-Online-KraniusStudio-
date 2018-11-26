using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerSalas : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabdeSalas;
    private GameObject PrefabdeSalas
    {
        get { return prefabdeSalas; }
    }

    private List<ListadeSalas> listadeSalasButton = new List<ListadeSalas>();
    private List<ListadeSalas> ListadeSalasButton
    {
        get { return listadeSalasButton; }
    }

    private void OnReceivedRoomListUpdate()
    {
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();

        foreach (RoomInfo room in rooms)
        {
            RoomReceived(room);
        }

        RemoveOldRooms();
    }

    private void RoomReceived(RoomInfo room)
    {
        int index = ListadeSalasButton.FindIndex(x => x.RoomName == room.Name);

        if (index == -1)
        {
            if (room.IsVisible && room.PlayerCount < room.MaxPlayers)
            {
                GameObject prefabdeSalaObj = Instantiate(prefabdeSalas);
                prefabdeSalaObj.transform.SetParent(transform, false);

                ListadeSalas listadeSals = prefabdeSalaObj.GetComponent<ListadeSalas>();
                ListadeSalasButton.Add(listadeSals);

                index = (ListadeSalasButton.Count - 1);

            }
        }
        if (index != -1)
        {
            ListadeSalas listadeSalas = ListadeSalasButton[index];
            listadeSalas.SetRoonNameText(room.Name);
            listadeSalas.Updated = true;
        }
    }

    private void RemoveOldRooms()
    {
        List<ListadeSalas> removeListadeSalas = new List<ListadeSalas>();
        foreach (ListadeSalas listadesalas in listadeSalasButton)
        {
            if (!listadesalas.Updated)
                removeListadeSalas.Add(listadesalas);
            else
                listadesalas.Updated = false;
        }
        foreach (ListadeSalas listadeSalas in removeListadeSalas)
        {
            GameObject listadeSalasobj = listadeSalas.gameObject;
            ListadeSalasButton.Remove(listadeSalas);
            Destroy(listadeSalasobj);
        }
    }
}
