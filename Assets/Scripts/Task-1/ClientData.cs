using System.Collections.Generic;

[System.Serializable]
public class ClientDataList
{
    public List<ClientData> clients;
    public Dictionary<int, ClientDetails> data;
    public string label;
}

[System.Serializable]
public class ClientData
{
    public bool isManager;
    public int id;
    public string label;
    public ClientDetails clientDetails;
}

[System.Serializable]
public class ClientDetails
{
    public string address;
    public string name;
    public int points;
}
