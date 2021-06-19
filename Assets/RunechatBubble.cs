using MLAPI;
using MLAPI.NetworkVariable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunechatBubble : NetworkBehaviour
{
    public TMPro.TextMeshPro text;

    public NetworkVariable<string> chatText = new NetworkVariable<string>();

    // Start is called before the first frame update
    void Start()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            Destroy(gameObject, 4f);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = chatText.Value;
        text.alpha -= Time.deltaTime * 0.5f;
        transform.position = new Vector3(transform.position.x, transform.position.y + Time.deltaTime * 0.5f);
    }
}
