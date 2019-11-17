using System.Collections.Generic;
using UnityEngine;
using Project.Utility;
using System;

namespace Project.Networking
{
    public class NetworkClient : SocketIO.SocketIOComponent
    {
        [Header("Network Client")]
        [SerializeField]
        private Transform networkContainer;

        [SerializeField]
        private GameObject playerPrefab;

        public static string ClientID { get; private set; }

        private Dictionary<string, NetworkIdentity> serverObjects;


        // Start is called before the first frame update
        public override void Start()
        {
            base.Start();
            Initialize();
            SetupEvents();
        }

        // Update is called once per frame
        public override void Update()
        {
            base.Update();
        }

        private void Initialize()
        {
            serverObjects = new Dictionary<string, NetworkIdentity>();
        }
        private void SetupEvents()
        {
            On("open", (E) =>
            {
                Debug.Log("connection made");
            });

            On("register", (E) =>
            {
                ClientID = E.data["id"].ToString().RemoveQuotes();

                Debug.LogFormat("Our Client's ID ({0})", ClientID);
            });

            On("spawn", (E) =>
            {
                string id = E.data["id"].ToString().RemoveQuotes();

                GameObject go = Instantiate(playerPrefab, networkContainer);
                go.name = string.Format("Player({0})", id);
                NetworkIdentity ni = go.GetComponent<NetworkIdentity>();
                ni.SetControllerID(id);
                ni.SetSocketReference(this);
                serverObjects.Add(id, ni);
            });

            On("disconnected", (E) =>
            {
                string id = E.data["id"].ToString().RemoveQuotes();

                GameObject go = serverObjects[id].gameObject;
                Destroy(go);
                serverObjects.Remove(id);
            });

            On("updatePosition", (E) =>
            {
                Debug.Log("received packet");
                string id = E.data["id"].ToString().RemoveQuotes();
                float x = E.data["position"]["x"].f;
                float y = E.data["position"]["y"].f;
                float z = E.data["position"]["z"].f;
                NetworkIdentity ni = serverObjects[id];
                ni.transform.position = new Vector3(x, y, z);
                Debug.Log(ni.transform.position);
            });

        }
    }

    [System.Serializable]
    public class Player
    {
        public string id;
        public Position position;
    }

    [System.Serializable]
    public class Position : ISerializationCallbackReceiver
    {
        [NonSerialized] public float x;
        [NonSerialized] public float y;
        [NonSerialized] public float z;

        [SerializeField] public string _x;
        [SerializeField] public string _y;
        [SerializeField] public string _z;

        public void OnAfterDeserialize()
        {
            x = float.Parse(_x);
            y = float.Parse(_y);
            z = float.Parse(_z);
        }

        public void OnBeforeSerialize()
        {
            _x = x.ToString();
            _y = x.ToString();
            _z = x.ToString();
        }
    }
}
