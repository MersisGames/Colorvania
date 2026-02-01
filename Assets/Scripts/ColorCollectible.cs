using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Collectible item that cycles color modes when collected.
/// When collected, it hides, disables its collider, and sets the ColorManager's color mode.
/// Collecting one makes the others reappear.
/// </summary>
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(SpriteRenderer))]
public class ColorCollectible : MonoBehaviour
{
    [Header("Color Settings")]
    [Tooltip("The color this collectible represents")]
    public ColorMode collectibleColor = ColorMode.Red;

    [Header("References")]
    [Tooltip("The sprite renderer to hide when collected. If null, will use GetComponent.")]
    public SpriteRenderer spriteRenderer;

    [Tooltip("The collider used for collection. If null, will use GetComponent.")]
    public Collider triggerCollider;

    [Header("Events")]
    [Tooltip("Called when this collectible is collected")]
    public UnityEvent OnCollected;

    private bool isCollected = false;
    private static ColorCollectible[] allCollectibles;

    private void Awake()
    {
        // Get components if not assigned
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (triggerCollider == null)
            triggerCollider = GetComponent<Collider>();

        // Ensure collider is a trigger
        if (triggerCollider != null)
            triggerCollider.isTrigger = true;
    }

    private void Start()
    {
        // Cache all color collectibles in the scene
        RefreshCollectibleCache();
        
        // Ensure we start visible
        SetVisible(true);
    }

    private void OnTriggerStay(Collider other)
    {
        // Check if player collided
        if (isCollected)
            return;

        // Check if it's the player (using tag or component)
        bool isPlayer = other.CompareTag("Player") || other.GetComponent<PLAYERTWO.PlatformerProject.Player>() != null;
        
        if (isPlayer)
        {
            Collect();
        }
    }

    /// <summary>
    /// Collects this color collectible.
    /// </summary>
    public void Collect()
    {
        if (isCollected)
            return;

        isCollected = true;

        // Hide sprite
        SetVisible(false);

        // Disable collider
        if (triggerCollider != null)
        {
            triggerCollider.enabled = false;
        }

        // Set ColorManager color mode
        if (ColorManager.instance != null)
        {
            ColorManager.instance.SetColorMode(collectibleColor);
            Debug.Log($"[ColorCollectible] {gameObject.name}: Collected! Setting color mode to {collectibleColor}");
        }
        else
        {
            Debug.LogWarning($"[ColorCollectible] {gameObject.name}: ColorManager not found!");
        }

        // Make other collectibles reappear
        ReappearOtherCollectibles();

        // Invoke event
        OnCollected?.Invoke();
    }

    /// <summary>
    /// Makes all other color collectibles reappear.
    /// </summary>
    private void ReappearOtherCollectibles()
    {
        RefreshCollectibleCache();

        foreach (ColorCollectible collectible in allCollectibles)
        {
            if (collectible != null && collectible != this && collectible.isCollected)
            {
                collectible.SetVisible(true);
                collectible.isCollected = false;
                
                if (collectible.triggerCollider != null)
                {
                    collectible.triggerCollider.enabled = true;
                }

                Debug.Log($"[ColorCollectible] {collectible.gameObject.name}: Reappeared!");
            }
        }
    }

    /// <summary>
    /// Sets the visibility of this collectible.
    /// </summary>
    /// <param name="visible">True to show, false to hide</param>
    public void SetVisible(bool visible)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = visible;
        }
    }

    /// <summary>
    /// Refreshes the cache of all color collectibles in the scene.
    /// </summary>
    private static void RefreshCollectibleCache()
    {
#if UNITY_6000_0_OR_NEWER
        allCollectibles = FindObjectsByType<ColorCollectible>(FindObjectsSortMode.None);
#else
        allCollectibles = FindObjectsOfType<ColorCollectible>();
#endif
    }

    /// <summary>
    /// Resets this collectible to its initial state (visible and collectible).
    /// </summary>
    public void ResetCollectible()
    {
        isCollected = false;
        SetVisible(true);
        
        if (triggerCollider != null)
        {
            triggerCollider.enabled = true;
        }
    }
}
