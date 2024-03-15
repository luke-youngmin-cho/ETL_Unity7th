using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using DiceGame.Data;
using TMPro;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    private ChatClient _chatClient;
    private Dictionary<string, TMP_Text> _chatLogs = new Dictionary<string, TMP_Text>();

    private void Start()
    {
        _chatClient = new ChatClient(this);
        _chatClient.Connect("16490a5d-55c6-4cab-be49-a2a91c8d0603",
                            PhotonNetwork.AppVersion,
                            new Photon.Chat.AuthenticationValues(LoginInformation.profile.nickname));
    }

    private void Update()
    {
        _chatClient?.Service();
    }

    public void PublishMessage(string message)
    {
        _chatClient.PublishMessage("General", message);
    }

    public void OnConnected()
    {
        _chatClient.Subscribe(new string[] { "General" });
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < senders.Length; i++)
        {
            _chatLogs[channelName].text += $"{senders[i]} : {messages[i]}\n";
        }
    }

    public void DebugReturn(DebugLevel level, string message)
    {
    }

    public void OnChatStateChange(ChatState state)
    {
    }

    public void OnDisconnected()
    {
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
    }

    public void OnUnsubscribed(string[] channels)
    {
    }

    public void OnUserSubscribed(string channel, string user)
    {
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
    }
}
