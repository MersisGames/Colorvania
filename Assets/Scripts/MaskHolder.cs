using UnityEngine;

/// <summary>
/// Manages three child GameObjects that are enabled/disabled based on the ColorManager's color mode.
/// Only the child matching the active color mode will be enabled.
/// </summary>
public class MaskHolder : MonoBehaviour
{
    [Header("Child References")]
    [Tooltip("The child GameObject to show when Red color mode is active")]
    public GameObject redChild;

    [Tooltip("The child GameObject to show when Blue color mode is active")]
    public GameObject blueChild;

    [Tooltip("The child GameObject to show when Yellow color mode is active")]
    public GameObject yellowChild;

    [Header("Auto-Assign Settings")]
    [Tooltip("If true, will automatically find children by name if references are not set")]
    public bool autoFindChildren = true;

    private void Start()
    {
        // Auto-find children if enabled and references are missing
        if (autoFindChildren)
        {
            if (redChild == null)
                redChild = FindChildByName("Red");
            if (blueChild == null)
                blueChild = FindChildByName("Blue");
            if (yellowChild == null)
                yellowChild = FindChildByName("Yellow");
        }

        // Subscribe to ColorManager if it exists
        if (ColorManager.instance != null)
        {
            ColorManager.instance.OnColorModeChanged.AddListener(OnColorModeChanged);
            // Apply initial state
            OnColorModeChanged(ColorManager.instance.CurrentColorMode);
        }
        else
        {
            Debug.LogWarning($"[MaskHolder] {gameObject.name}: ColorManager not found!");
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from ColorManager
        if (ColorManager.instance != null)
        {
            ColorManager.instance.OnColorModeChanged.RemoveListener(OnColorModeChanged);
        }
    }

    /// <summary>
    /// Called when the color mode changes. Updates which child is active.
    /// </summary>
    /// <param name="colorMode">The new active color mode</param>
    private void OnColorModeChanged(ColorMode colorMode)
    {
        // Disable all children first
        SetChildActive(redChild, false);
        SetChildActive(blueChild, false);
        SetChildActive(yellowChild, false);

        // Enable the child matching the color mode
        switch (colorMode)
        {
            case ColorMode.Red:
                SetChildActive(redChild, true);
                Debug.Log($"[MaskHolder] {gameObject.name}: Red child activated");
                break;

            case ColorMode.Blue:
                SetChildActive(blueChild, true);
                Debug.Log($"[MaskHolder] {gameObject.name}: Blue child activated");
                break;

            case ColorMode.Yellow:
                SetChildActive(yellowChild, true);
                Debug.Log($"[MaskHolder] {gameObject.name}: Yellow child activated");
                break;
        }
    }

    /// <summary>
    /// Sets a child GameObject active/inactive safely.
    /// </summary>
    /// <param name="child">The child GameObject to modify</param>
    /// <param name="active">True to enable, false to disable</param>
    private void SetChildActive(GameObject child, bool active)
    {
        if (child != null)
        {
            child.SetActive(active);
        }
    }

    /// <summary>
    /// Finds a child GameObject by name (case-insensitive).
    /// </summary>
    /// <param name="name">The name to search for</param>
    /// <returns>The found GameObject, or null if not found</returns>
    private GameObject FindChildByName(string name)
    {
        foreach (Transform child in transform)
        {
            if (child.name.Equals(name, System.StringComparison.OrdinalIgnoreCase))
            {
                return child.gameObject;
            }
        }
        return null;
    }

    /// <summary>
    /// Manually refresh the mask holder state based on current color mode.
    /// </summary>
    public void RefreshState()
    {
        if (ColorManager.instance != null)
        {
            OnColorModeChanged(ColorManager.instance.CurrentColorMode);
        }
    }

    // Validation in editor
    private void OnValidate()
    {
        if (Application.isPlaying && ColorManager.instance != null)
        {
            RefreshState();
        }
    }
}
