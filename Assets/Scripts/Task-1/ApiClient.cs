using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

public class ApiClient : MonoBehaviour
{
    private const string apiUrl = "https://qa2.sunbasedata.com/sunbase/portal/api/assignment.jsp?cmd=client_data";
    public static ApiClient instance;
    public GameObject clientListItemPrefab;
    public Transform contentContainer;
    public GameObject popup;
    public TextMeshProUGUI Header;
    public TextMeshProUGUI popupName;
    public TextMeshProUGUI popupPoints;
    public TextMeshProUGUI popupAddress;
    public TMP_Dropdown filterDropdown;

    private List<ClientData> clientDataList;

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        FetchDataFromAPI();
        filterDropdown.onValueChanged.AddListener(OnFilterDropdownChanged); 
    }

    public void FetchDataFromAPI()
    {
        StartCoroutine(GetDataFromAPI());
    }

    private IEnumerator GetDataFromAPI()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error fetching data from API: " + webRequest.error);
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                Debug.Log(jsonResponse);
                ProcessJSON(jsonResponse);
            }
        }
    }

    private void ProcessJSON(string json)
    {
        ClientDataList clientDataListContainer = JsonConvert.DeserializeObject<ClientDataList>(json);
        clientDataList = clientDataListContainer.clients;

        foreach (ClientData clientData in clientDataList)
        {
            if (clientDataListContainer.data.TryGetValue(clientData.id, out ClientDetails details))
            {
                clientData.clientDetails = details;
            }
            else
            {
                Debug.LogWarning($"Client data not found for ID: {clientData.id}");
                clientData.clientDetails = new ClientDetails
                {
                    name = "N/A",
                    points = 0,
                    address = "N/A"
                };
            }

            PopulateClient(clientData);
        }
    }


    private void ClearClientList()
    {
        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void PopulateClient(ClientData clientData)
    {
        GameObject clientItem = Instantiate(clientListItemPrefab, contentContainer);

        Client clientComponent = clientItem.GetComponent<Client>();

        if (clientData.clientDetails != null)
        {
            clientComponent.PopulateClientInfo(clientData);
        }
        else
        {
            clientComponent.PopulateClientInfo(new ClientData
            {
                label = clientData.label,
                clientDetails = new ClientDetails { name = "N/A", points = 0, address = "N/A" }
            });
        }

        Button button = clientItem.GetComponent<Button>();
        button.onClick.AddListener(() => OpenPopup(clientData));
    }


    private void OpenPopup(ClientData clientData)
    {
        Header.text = clientData.label;
        popupName.text = clientData.clientDetails.name;
        popupPoints.text = "Points: " + clientData.clientDetails.points.ToString();
        popupAddress.text = "Address: " + clientData.clientDetails.address;

        popup.SetActive(true);
        popup.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }

    private void OnFilterDropdownChanged(int value)
    {
        List<ClientData> filteredList;

        switch (value)
        {
            case 0: // All clients
                filteredList = clientDataList;
                break;
            case 1: // Managers only
                filteredList = clientDataList.FindAll(client => client.isManager);
                break;
            case 2: // Non managers
                filteredList = clientDataList.FindAll(client => !client.isManager);
                break;
            default:
                filteredList = clientDataList;
                break;
        }

        ClearClientList();
        PopulateClientList(filteredList);
    }

    private void PopulateClientList(List<ClientData> clients)
    {
        foreach (ClientData clientData in clients)
        {
            GameObject clientItem = Instantiate(clientListItemPrefab, contentContainer);

            Client clientComponent = clientItem.GetComponent<Client>();
            clientComponent.PopulateClientInfo(clientData);

            Button button = clientItem.GetComponent<Button>();
            button.onClick.AddListener(() => OpenPopup(clientData));
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

}
