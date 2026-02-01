using UnityEngine;
using UnityEngine.Events;

public enum ColorMode
{
    Red,
    Blue,
    Yellow
}

/// <summary>
/// Singleton manager that controls the global color mode.
/// Objects with ColorObject script will react to color mode changes.
/// </summary>
public class ColorManager : MonoBehaviour
{
    private static ColorManager m_instance;

    public static ColorManager instance
    {
        get
        {
            if (m_instance == null)
            {
#if UNITY_6000_0_OR_NEWER
                m_instance = FindFirstObjectByType<ColorManager>(FindObjectsInactive.Include);
#else
                m_instance = FindObjectOfType<ColorManager>(true);
#endif
            }
            return m_instance;
        }
    }

    [Header("Current Color Mode")]
    [Tooltip("The currently active color mode")]
    [SerializeField] private ColorMode currentColorMode = ColorMode.Red;

    /// <summary>
    /// Event called when the color mode changes.
    /// Parameters: (newColorMode)
    /// </summary>
    public UnityEvent<ColorMode> OnColorModeChanged;

    /// <summary>
    /// Gets the current color mode.
    /// </summary>
    public ColorMode CurrentColorMode => currentColorMode;

    private void Awake()
    {
        if (m_instance != null && m_instance != this)
        {
            Destroy(gameObject);
            return;
        }
        m_instance = this;
    }

    private void Start()
    {
        // Notify all objects of the initial color mode
        NotifyColorModeChanged();
    }

    /// <summary>
    /// Sets the color mode and notifies all ColorObject instances.
    /// </summary>
    /// <param name="mode">The color mode to set</param>
    public void SetColorMode(ColorMode mode)
    {
        if (currentColorMode != mode)
        {
            currentColorMode = mode;
            NotifyColorModeChanged();
        }
    }

    /// <summary>
    /// Notifies all ColorObject instances of the current color mode.
    /// </summary>
    private void NotifyColorModeChanged()
    {
        OnColorModeChanged?.Invoke(currentColorMode);

        // Also directly notify all ColorObject instances
        ColorObject[] colorObjects = FindObjectsByType<ColorObject>(FindObjectsSortMode.None);
        foreach (ColorObject colorObj in colorObjects)
        {
            colorObj.OnColorModeChanged(currentColorMode);
        }
    }

    /// <summary>
    /// Sets color mode to Red.
    /// </summary>
    public void SetRedMode()
    {
        SetColorMode(ColorMode.Red);
    }

    /// <summary>
    /// Sets color mode to Blue.
    /// </summary>
    public void SetBlueMode()
    {
        SetColorMode(ColorMode.Blue);
    }

    /// <summary>
    /// Sets color mode to Yellow.
    /// </summary>
    public void SetYellowMode()
    {
        SetColorMode(ColorMode.Yellow);
    }

    // Validation in editor
    private void OnValidate()
    {
        if (Application.isPlaying && m_instance == this)
        {
            NotifyColorModeChanged();
        }
    }
}
