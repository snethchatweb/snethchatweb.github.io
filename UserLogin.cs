using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Database;
using Firebase.Extensions;

[System.Serializable]
public class UserAccount
{
    public BasicInfo basicInfo;
    public ProfileInfo profileInfo;
    public HistoryEpInfo[] historyEpInfo;
    public ClanBasicInfo clanBasicInfo;
    public CaptainBasicInfo captainBasicInfo;
    public PetInfo petInfo;
    public SocialInfo socialInfo;
    public DiamondCostRes diamondCostRes;
    public CreditScoreInfo creditScoreInfo;
    public EquippedAch[] equippedAch;
}

[System.Serializable]
public class BasicInfo
{
    public string accountId;
    public int accountType;
    public string nickname;
    public string region;
    public int level;
    public int exp;
    public int bannerId;
    public int headPic;
    public int rank;
    public int rankingPoints;
    public int badgeCnt;
    public int badgeId;
    public int seasonId;
    public int liked;
    public bool showRank;
    public string lastLoginAt;
    public int csRank;
    public int csRankingPoints;
    public int[] weaponSkinShows;
    public int maxRank;
    public int csMaxRank;
    public AccountPrefers accountPrefers;
    public string createAt;
    public int title;
    public ExternalIconInfo externalIconInfo;
    public string releaseVersion;
    public bool showBrRank;
    public bool showCsRank;
    public SocialHighLightsWithBasicInfo socialHighLightsWithBasicInfo;
}

[System.Serializable]
public class AccountPrefers
{
    public bool hideRegion;
    public string accountName;
    public int character;
    public int offlineTime;
}

[System.Serializable]
public class ExternalIconInfo
{
    public string status;
    public string showType;
}

[System.Serializable]
public class SocialHighLightsWithBasicInfo
{
}

[System.Serializable]
public class ProfileInfo
{
    public int avatarId;
    public int[] clothes;
    public int[] equipedSkills;
    public int pvePrimaryWeapon;
    public int endTime;
    public int[] clothesTailorEffects;
}

[System.Serializable]
public class HistoryEpInfo
{
    public int epEventId;
    public int epBadge;
    public int badgeCnt;
    public string bpIcon;
    public int maxLevel;
    public string eventName;
}

[System.Serializable]
public class ClanBasicInfo
{
    public string clanId;
    public string clanName;
    public string captainId;
    public int clanLevel;
    public int capacity;
    public int memberNum;
}

[System.Serializable]
public class CaptainBasicInfo
{
    public string accountId;
    public int accountType;
    public string nickname;
    public string region;
    public int level;
    public int exp;
    public int bannerId;
    public int headPic;
    public int rank;
    public int rankingPoints;
    public int badgeCnt;
    public int badgeId;
    public int seasonId;
    public int liked;
    public bool showRank;
    public string lastLoginAt;
    public int csRank;
    public int csRankingPoints;
    public int[] weaponSkinShows;
    public int maxRank;
    public int csMaxRank;
    public AccountPrefers accountPrefers;
    public string createAt;
    public int title;
    public ExternalIconInfo externalIconInfo;
    public string releaseVersion;
    public bool showBrRank;
    public bool showCsRank;
    public SocialHighLightsWithBasicInfo socialHighLightsWithBasicInfo;
}

[System.Serializable]
public class PetInfo
{
    public int id;
    public string name;
    public int level;
    public int exp;
    public bool isSelected;
    public int skinId;
    public int selectedSkillId;
}

[System.Serializable]
public class SocialInfo
{
    public string accountId;
    public string gender;
    public string language;
    public string modePrefer;
    public string signature;
    public string rankShow;
}

[System.Serializable]
public class DiamondCostRes
{
    public int diamondCost;
}

[System.Serializable]
public class CreditScoreInfo
{
    public int creditScore;
    public string rewardState;
    public int periodicSummaryLikeCnt;
    public int periodicSummaryIllegalCnt;
    public string periodicSummaryStartTime;
    public string periodicSummaryEndTime;
}

[System.Serializable]
public class EquippedAch
{
    public int achId;
    public int level;
}

public class FirebaseAuthManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_InputField emailField;
    public TMP_InputField passwordField;
    public TextMeshProUGUI usernameText;
    public GameObject banPanel;
    public GameObject lobbyCanvas;
    public GameObject usernamePanel;
    public TMP_InputField usernameField;
    public Button chooseUsernameButton;

    private DatabaseReference databaseReference;
    private string databaseURL = "https://free-fire-ce368-default-rtdb.firebaseio.com/";

    void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                databaseReference = FirebaseDatabase.GetInstance(app).RootReference;
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
            }
        });

        chooseUsernameButton.onClick.AddListener(CreateAccount);
    }

    public void OnLoginButtonClicked()
    {
        string email = emailField.text;
        string password = passwordField.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            Debug.LogError("Email or Password is empty");
            return;
        }

        StartCoroutine(CheckUserCredentials(email, password));
    }

    private IEnumerator CheckUserCredentials(string email, string password)
    {
        string url = $"{databaseURL}users/guest/{email.Replace(".", ",")}.json";

        var request = new UnityEngine.Networking.UnityWebRequest(url, "GET");
        request.downloadHandler = new UnityEngine.Networking.DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.result == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError || request.result == UnityEngine.Networking.UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            string json = request.downloadHandler.text;

            if (json == "null")
            {
                usernamePanel.SetActive(true);
            }
            else
            {
                UserAccount user = JsonUtility.FromJson<UserAccount>(json);

                if (user.basicInfo.accountId == password)
                {
                    usernameText.text = user.basicInfo.nickname;

                    if (user.creditScoreInfo.creditScore < 0)
                    {
                        banPanel.SetActive(true);
                    }
                    else
                    {
                        lobbyCanvas.SetActive(true);
                        PlayerPrefs.SetString("uid", user.basicInfo.accountId);
                        PlayerPrefs.SetString("userLogin", "true");
                        PlayerPrefs.SetString("plataforma", "guest");
                    }
                }
                else
                {
                    Debug.LogError("Password mismatch");
                }
            }
        }
    }

    private void CreateAccount()
    {
        string email = emailField.text;
        string password = passwordField.text;
        string username = usernameField.text;

        if (string.IsNullOrEmpty(username))
        {
            Debug.LogError("Username is empty");
            return;
        }

        UserAccount newUser = new UserAccount
        {
            basicInfo = new BasicInfo
            {
                accountId = password,
                accountType = 1,
                nickname = username,
                region = "UNKNOWN",
                level = 1,
                exp = 0,
                bannerId = 0,
                headPic = 0,
                rank = 0,
                rankingPoints = 0,
                badgeCnt = 0,
                badgeId = 0,
                seasonId = 0,
                liked = 0,
                showRank = true,
                lastLoginAt = ((int)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds).ToString(),
                csRank = 0,
                csRankingPoints = 0,
                weaponSkinShows = new int[0],
                maxRank = 0,
                csMaxRank = 0,
                accountPrefers = new AccountPrefers(),
                createAt = ((int)(System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1))).TotalSeconds).ToString(),
                title = 0,
                externalIconInfo = new ExternalIconInfo(),
                releaseVersion = "1.0",
                showBrRank = true,
                showCsRank = true,
                socialHighLightsWithBasicInfo = new SocialHighLightsWithBasicInfo()
            },
            profileInfo = new ProfileInfo(),
            historyEpInfo = new List<HistoryEpInfo>().ToArray(),
            clanBasicInfo = new ClanBasicInfo(),
            captainBasicInfo = new CaptainBasicInfo(),
            petInfo = new PetInfo(),
            socialInfo = new SocialInfo(),
            diamondCostRes = new DiamondCostRes(),
            creditScoreInfo = new CreditScoreInfo(),
            // Continuando a definição do newUser
equippedAch = new List<EquippedAch>().ToArray()
};

string json = JsonUtility.ToJson(newUser);

string url = $"{databaseURL}users/guest/{email.Replace(".", ",")}.json";

var request = new UnityEngine.Networking.UnityWebRequest(url, "PUT");
byte[] bodyRaw = new System.Text.UTF8Encoding().GetBytes(json);
request.uploadHandler = new UnityEngine.Networking.UploadHandlerRaw(bodyRaw);
request.downloadHandler = new UnityEngine.Networking.DownloadHandlerBuffer();
request.SetRequestHeader("Content-Type", "application/json");

StartCoroutine(SendNewUserRequest(request));
}

private IEnumerator SendNewUserRequest(UnityEngine.Networking.UnityWebRequest request)
{
yield return request.SendWebRequest();

if (request.result == UnityEngine.Networking.UnityWebRequest.Result.ConnectionError || request.result == UnityEngine.Networking.UnityWebRequest.Result.ProtocolError)
{
Debug.LogError(request.error);
}
else
{
Debug.Log("User created successfully");
usernamePanel.SetActive(false);
OnLoginButtonClicked();
}
}
}