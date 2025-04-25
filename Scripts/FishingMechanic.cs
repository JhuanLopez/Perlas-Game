using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("Fishing State")]
    private bool isFishing = false;
    private bool minigameActive = false;
    private float catchProgress = 0f;
    
    [Header("Fish Movement")]
    [SerializeField] private float fishMovementSpeed = 2f;
    [SerializeField] private float fishMovementRange = 100f; // Range in UI units
    private float fishPosition;
    private float fishTarget;
    
    [Header("Player Control")]
    [SerializeField] private float barMovementSpeed = 150f;
    [SerializeField] private float catchingRange = 30f; // Distance needed to catch fish
    private float barPosition;
    
    [Header("Catch Mechanics")]
    [SerializeField] private float catchSpeed = 0.5f;
    [SerializeField] private float escapeSpeed = 0.3f;
    [SerializeField] private float requiredProgress = 1f;
    
    [Header("UI References")]
    [SerializeField] private RectTransform progressBar;
    [SerializeField] private RectTransform fishIndicator;
    [SerializeField] private RectTransform playerBar;
    [SerializeField] private GameObject minigamePanel;
    void Start()
    {
         minigamePanel.SetActive(false);
        ResetPositions();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isFishing)
        {
            StartFishing();
        }
        
        if (minigameActive)
        {
            UpdateFishingMinigame();
        }
    }
    void StartFishing()
    {
        isFishing = true;
        Debug.Log("Casting line...");
        Invoke("StartMinigame", Random.Range(1f, 3f)); // Random time before fish bites
    }

    void StartMinigame()
    {
        minigameActive = true;
        minigamePanel.SetActive(true);
        ResetPositions();
        Debug.Log("Fish is biting!");
    }

    void ResetPositions()
    {
        catchProgress = 0f;
        fishPosition = 0f;
        fishTarget = 0f;
        barPosition = 0f;
        
        // Reset UI positions
        if (fishIndicator != null)
            fishIndicator.anchoredPosition = new Vector2(0, fishPosition);
        if (playerBar != null)
            playerBar.anchoredPosition = new Vector2(0, barPosition);
        if (progressBar != null)
            progressBar.localScale = new Vector3(catchProgress, 1, 1);
    }

    void UpdateFishingMinigame()
    {
        // Update fish movement
        UpdateFishPosition();
        
        // Update player bar position
        UpdatePlayerPosition();
        
        // Check if fish is caught
        CheckCatchProgress();
        
        // Update UI positions
        UpdateUIPositions();
    }

    void UpdateFishPosition()
    {
        // Create a smooth moving target for the fish
        fishTarget = Mathf.Sin(Time.time * fishMovementSpeed) * fishMovementRange;
        fishPosition = Mathf.Lerp(fishPosition, fishTarget, Time.deltaTime * 3f);
    }

    void UpdatePlayerPosition()
    {
        // Move bar up with up arrow, down with down arrow or when no input
        if (Input.GetKey(KeyCode.UpArrow))
        {
            barPosition += barMovementSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            barPosition -= barMovementSpeed * Time.deltaTime;
        }
        else
        {
            barPosition -= barMovementSpeed * 0.5f * Time.deltaTime; // Natural fall when no input
        }
        
        // Clamp bar position to panel bounds
        barPosition = Mathf.Clamp(barPosition, -fishMovementRange, fishMovementRange);
    }

    void CheckCatchProgress()
    {
        // Check if player bar is close enough to fish
        float distance = Mathf.Abs(fishPosition - barPosition);
        bool isInCatchRange = distance < catchingRange;
        
        // Update catch progress
        if (isInCatchRange)
        {
            catchProgress += catchSpeed * Time.deltaTime;
            if (catchProgress >= requiredProgress)
            {
                CatchFish();
            }
        }
        else
        {
            catchProgress -= escapeSpeed * Time.deltaTime;
        }
        
        catchProgress = Mathf.Clamp(catchProgress, 0f, requiredProgress);
    }

    void UpdateUIPositions()
    {
        if (fishIndicator != null)
            fishIndicator.anchoredPosition = new Vector2(0, fishPosition);
        
        if (playerBar != null)
            playerBar.anchoredPosition = new Vector2(0, barPosition);
        
        if (progressBar != null)
            progressBar.localScale = new Vector3(catchProgress, 1, 1);
    }

    void CatchFish()
    {
        Debug.Log("Fish caught!");
        minigameActive = false;
        isFishing = false;
        minigamePanel.SetActive(false);
    }
}
