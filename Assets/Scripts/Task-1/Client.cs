using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Client : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private TextMeshProUGUI pointsLabel;

    public void PopulateClientInfo(ClientData clientData)
    {
        nameLabel.text = clientData.label;
        if(clientData.clientDetails!=null) 
            pointsLabel.text = "Points: " + clientData.clientDetails.points.ToString();
    }
}
