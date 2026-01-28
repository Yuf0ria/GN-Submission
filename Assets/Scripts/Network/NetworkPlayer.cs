using Fusion;
using UnityEngine;
using TMPro;

public class NetworkPlayer : NetworkBehaviour
{
    [SerializeField] private MeshRenderer m_MeshRenderer;
    [SerializeField] public TMP_Text Inputedname;
    //static
    public static NetworkPlayer Local { get; set; }
    [Header("Network Properties")]
    [Networked] public Vector3 NetworkedPosition { get; set; }
    [Networked] public Color PlayerColor { get; set; }
    [Networked] public NetworkString<_32> PlayerName { get; set; }
    
    public override void FixedUpdateNetwork()
    {
        if (!HasStateAuthority) return;
        if(!GetInput(out NetworkInputData input)) return;

        this.transform.position +=
            new Vector3(input.InputVector.normalized.x, 0, input.InputVector.normalized.y)
            * Runner.DeltaTime;

        NetworkedPosition = this.transform.position;
    }
    #region Fusion Callbacks
    public override void Spawned()
    {
        if (HasInputAuthority)//client
        {
            Local = this;
            //Name
            RPC_SetPLayerName(PlayerPrefs.GetString("player"));
        }
        if (HasStateAuthority)//Server
        {
            PlayerColor = Random.ColorHSV();//renders color to the player
        }
    }

    public override void Render()
    {
        this.transform.position = NetworkedPosition;
        if (m_MeshRenderer != null && m_MeshRenderer.material.color != PlayerColor) 
        {
            m_MeshRenderer.material.color = PlayerColor;
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]

    private void RPC_SetPLayerColor(Color color)
    {
        if (HasStateAuthority)
        {
            this.PlayerColor = color;
        }
    }

    public void RPC_SetPLayerName(string PlayerName, RpcInfo info = default ){
        if(HasInputAuthority){
            this.name = PlayerName;
        }
    }
    #endregion

    #region Unity Callbacks
    private void Update()
    {
        if (!HasInputAuthority) return; //sends information
        if(Input.GetKeyDown(KeyCode.Q))
        {
            var randColor = Random.ColorHSV();
            RPC_SetPLayerColor(randColor);
        }
    }
    #endregion
}
