using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomListing : MonoBehaviourPunCallbacks {

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        Debug.Log("AAAAAA");
    }
}
