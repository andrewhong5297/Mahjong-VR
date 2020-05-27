using System;
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;
using Com.MyCompany.MyGame;

public class GameManager : MonoBehaviourPunCallbacks
{
    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;
    public GameObject SettingsUIPrefab;
    public GameObject UpdateUIPrefab; //authority? 

    //spawn positions
    public GameObject EastPos;
    public GameObject SouthPos;
    public GameObject WestPos;
    public GameObject NorthPos;

    public static GameManager Instance;

    public void Start()
    {
        Instance = this;
        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            if (PlayerManager.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate

                //count number of players so we can instantiate in a seperate location?
                if (PhotonNetwork.CurrentRoom.PlayerCount == 0) //east
                {
                    PhotonNetwork.Instantiate(this.playerPrefab.name, EastPos.transform.position, Quaternion.identity, 0);
                    Instantiate(UpdateUIPrefab, new Vector3(0.027f, -.237f, -1.071f), Quaternion.identity);
                    UpdateUIPrefab.SetActive(true);
                }
                if (PhotonNetwork.CurrentRoom.PlayerCount == 1) //south
                {
                    PhotonNetwork.Instantiate(this.playerPrefab.name, EastPos.transform.position, Quaternion.identity, 0);
                    Instantiate(UpdateUIPrefab, new Vector3(0.027f, -.237f, -1.071f), Quaternion.identity);
                    UpdateUIPrefab.SetActive(true);
                }
                if (PhotonNetwork.CurrentRoom.PlayerCount == 2) //west
                {
                    PhotonNetwork.Instantiate(this.playerPrefab.name, EastPos.transform.position, Quaternion.identity, 0);
                    Instantiate(UpdateUIPrefab, new Vector3(0.027f, -.237f, -1.071f), Quaternion.identity);
                }
                if (PhotonNetwork.CurrentRoom.PlayerCount == 3) //north
                {
                    PhotonNetwork.Instantiate(this.playerPrefab.name, EastPos.transform.position, Quaternion.identity, 0);
                    Instantiate(UpdateUIPrefab, new Vector3(0.027f, -.237f, -1.071f), Quaternion.identity);
                }
                Instantiate(SettingsUIPrefab, new Vector3(0, -0, -0), Quaternion.identity);
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }
    }

    #region Photon Callbacks

    ///<summary>
    ///Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting


        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


            LoadArena();
        }
    }


    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom


            LoadArena();
        }
    }
    #endregion

    #region Public Methods

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    #endregion

    #region Private Methods

    void LoadArena()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to load a level but we are not the master client");
        }
        Debug.LogFormat("PhotonNetwork : Loading Mahjong VR Room");
        PhotonNetwork.LoadLevel("Mahjong VR Controllers");
    }

    #endregion

}
