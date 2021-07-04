using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Doozy.Engine;
using Newtonsoft.Json;

public class NetworkManager : MonoBehaviour
{
    private SignInResponse signInResponse;
    public List<CollectionListResponse> collectionList = new List<CollectionListResponse>();
    public List<CollectionElementResponse> elementsList = new List<CollectionElementResponse>();

    private string accessToken;
    private string refreshToken;

    private Texture texture;

    private const string loginAddress = "https://test.loy.am/oauth/token";
    private const string collectionListAddress = "https://test.loy.am/api/sets";
    private const string registrateAddress = "https://test.loy.am/api/users";
    private const string refreshAddress = "https://test.loy.am/oauth/token";
    private const string collectionElementsAddress = "https://test.loy.am/api/subjects";
    private const string favoriteElementAddressStart = "https://test.loy.am/api/subjects/";
    private const string favoriteElementAddressEnd = "/actions/favorite";
    private const string unFavoriteElementAddressEnd = "/actions/unfavorite";
    private const string clientSecret = "0IcbmorPNeuEcywxvaGQzznSd3pIl8BF12hT8eeExuZ2G9XYJH7YHeQh";

    [Header("System Components")]
    [SerializeField] private Database database;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private ErrorManager errorManager;
    [SerializeField] private SignInView signInView;
    [SerializeField] private SignUpView signUpView;


    private void Start()
    {
        StartCoroutine(LoadingDelay());
    }

    private IEnumerator LoadingDelay()
    {
        yield return new WaitForSeconds(1.0f);

        if (!PlayerPrefs.HasKey("AccessToken")) GameEventMessage.SendEvent("Show Sign In View");
        else
        {
            LoadTokens();
            StartCoroutine(RefreshToken());
        }
    }

    private void SaveTokens()
    {
        PlayerPrefs.SetString("AccessToken", accessToken);
        PlayerPrefs.SetString("RefreshToken", refreshToken);
        PlayerPrefs.Save();
    }

    private void LoadTokens()
    {
        accessToken = PlayerPrefs.GetString("AccessToken");
        refreshToken = PlayerPrefs.GetString("RefreshToken");
    }

    private IEnumerator RefreshToken()
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("grant_type", "refresh_token"));
        formData.Add(new MultipartFormDataSection("client_id", "loyam_test"));
        formData.Add(new MultipartFormDataSection("client_secret", clientSecret));
        formData.Add(new MultipartFormDataSection("refresh_token", refreshToken));

        UnityWebRequest www = UnityWebRequest.Post(refreshAddress, formData);

        www.SetRequestHeader("Accept", "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            signInResponse = JsonConvert.DeserializeObject<SignInResponse>(www.downloadHandler.text);

            accessToken = signInResponse.access_token;
            refreshToken = signInResponse.refresh_token;
            SaveTokens();

            StartCoroutine(GetCollectionList());
        }
        else GameEventMessage.SendEvent("Show Sign In View");

    }

    private IEnumerator SignIn()
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("grant_type", "password"));
        formData.Add(new MultipartFormDataSection("client_id", "loyam_test"));
        formData.Add(new MultipartFormDataSection("client_secret", clientSecret));
        formData.Add(new MultipartFormDataSection("username", signInView.username.text));
        formData.Add(new MultipartFormDataSection("password", signInView.password.text));

        UnityWebRequest www = UnityWebRequest.Post(loginAddress, formData);

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            if (www.responseCode == 400) errorManager.ShowError("Неправильное имя или пароль");
            else errorManager.ShowError(www.error);
        }
        else
        {
            signInResponse = JsonConvert.DeserializeObject<SignInResponse>(www.downloadHandler.text);

            accessToken = signInResponse.access_token;
            refreshToken = signInResponse.refresh_token;
            SaveTokens();

            StartCoroutine(GetCollectionList());
        }

    }

    private IEnumerator Registrate()
    {
        List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        formData.Add(new MultipartFormDataSection("client_id", "loyam_test"));
        formData.Add(new MultipartFormDataSection("client_secret", clientSecret));
        formData.Add(new MultipartFormDataSection("username", signUpView.username.text));
        formData.Add(new MultipartFormDataSection("password", signUpView.password.text));
        formData.Add(new MultipartFormDataSection("email", signUpView.email.text));

        UnityWebRequest www = UnityWebRequest.Post(registrateAddress, formData);

        www.SetRequestHeader("Accept", "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            if (www.responseCode == 422) errorManager.ShowError("Такое имя или адрес электронной почты уже существуют");
            else errorManager.ShowError(www.error);
        }
        else StartCoroutine(GetCollectionList());

    }

    private IEnumerator GetCollectionList()
    {
        UnityWebRequest www = UnityWebRequest.Get(collectionListAddress);
        www.SetRequestHeader("Authorization", "Bearer " + accessToken);
        www.SetRequestHeader("Accept", "application/json");

        yield return www.SendWebRequest();

        collectionList = JsonConvert.DeserializeObject<List<CollectionListResponse>>(www.downloadHandler.text);

        StartCoroutine(GetCollectionElements());
    }

    private IEnumerator GetCollectionElements()
    {
        UnityWebRequest www = UnityWebRequest.Get(collectionElementsAddress);
        www.SetRequestHeader("Authorization", "Bearer " + accessToken);
        www.SetRequestHeader("Accept", "application/json");

        yield return www.SendWebRequest();

        elementsList = JsonConvert.DeserializeObject<List<CollectionElementResponse>>(www.downloadHandler.text);

        database.BuildDatabase(collectionList, elementsList, this);
        spawnManager.BuildCollectionListView();
        GameEventMessage.SendEvent("Show Collection List View");
    }

    public void OnSignInButtonClick()
    {
        if (!signInView.CheckError()) StartCoroutine(SignIn());
    }

    public void OnSignUpButtonClick()
    {
        if (!signUpView.CheckError()) StartCoroutine(Registrate());
    }

    private IEnumerator FavCollectionElement(int elementIndex)
    {
        byte[] bodyData = null;
        UnityWebRequest www = UnityWebRequest.Put(favoriteElementAddressStart + elementIndex.ToString() + favoriteElementAddressEnd, bodyData);
        www.SetRequestHeader("Authorization", "Bearer " + accessToken);
        www.SetRequestHeader("Accept", "application/json");

        yield return www.SendWebRequest();
    }

    public void FavoriteElement(int _elementIndex)
    {
        StartCoroutine(FavCollectionElement(_elementIndex));
    }

    private IEnumerator UnFavCollectionElement(int elementIndex)
    {
        byte[] bodyData = null;
        UnityWebRequest www = UnityWebRequest.Put(favoriteElementAddressStart + elementIndex.ToString() + unFavoriteElementAddressEnd, bodyData);
        www.SetRequestHeader("Authorization", "Bearer " + accessToken);
        www.SetRequestHeader("Accept", "application/json");

        yield return www.SendWebRequest();
    }

    public void UnFavoriteElement(int _elementIndex)
    {
        StartCoroutine(UnFavCollectionElement(_elementIndex));
    }

    //avoid "put request with empty body" error 
    public static UnityWebRequest Put(string uri, byte[] bodyData)
    {
        return new UnityWebRequest(uri, "PUT", (DownloadHandler)new DownloadHandlerBuffer(), bodyData == null || bodyData.Length == 0 ? null : (UploadHandler)new UploadHandlerRaw(bodyData));
    }
}
