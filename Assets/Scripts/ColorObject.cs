using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Component for objects that react to color mode changes.
/// When the active color mode matches this object's color, it becomes active (colored, interactable, etc.).
/// When it doesn't match, it becomes inactive (desaturated, non-interactable, etc.).
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class ColorObject : MonoBehaviour
{
    [Header("Color Settings")]
    [Tooltip("The color this object represents (Red, Blue, or Yellow)")]
    public ColorMode objectColor = ColorMode.Red;

    [Header("Material Settings")]
    [Tooltip("The material instance to modify. If null, will use SpriteRenderer's material.")]
    public Material materialInstance;

    [Header("Behavior Settings")]
    [Tooltip("If true, will disable/enable colliders based on color mode match")]
    public bool ToggleCollider = true;

    [Tooltip("If true, inverts collider behavior - collider is DISABLED when color matches (hole/platform disappears), ENABLED when color doesn't match (acts as platform)")]
    public bool InvertColliderToggle = false;

    [Tooltip("If true, will disable/enable the GameObject when color doesn't match")]
    public bool toggleGameObject = false;

    [Header("Events")]
    [Tooltip("Called when this object becomes active (color mode matches)")]
    public UnityEvent OnBecameActive;

    [Tooltip("Called when this object becomes inactive (color mode doesn't match)")]
    public UnityEvent OnBecameInactive;

    private const string DESATURATE_PROPERTY = "_Desaturate";
    private SpriteRenderer spriteRenderer;
    private Collider[] colliders;
    private bool isActive = true;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Get or create material instance
        if (materialInstance == null && spriteRenderer != null)
        {
            materialInstance = spriteRenderer.material;
        }
    }

    private void Start()
    {
        // Subscribe to ColorManager if it exists
        if (ColorManager.instance != null)
        {
            ColorManager.instance.OnColorModeChanged.AddListener(OnColorModeChanged);
        }
        else
        {
            Debug.LogWarning($"ColorObject on '{gameObject.name}' could not find ColorManager instance.");
        }
        
        // Apply initial state - do this after subscription to ensure we get the latest state
        if (ColorManager.instance != null)
        {
            OnColorModeChanged(ColorManager.instance.CurrentColorMode);
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
    /// Called when the color mode changes. Updates this object's state accordingly.
    /// </summary>
    /// <param name="activeColorMode">The currently active color mode</param>
    public void OnColorModeChanged(ColorMode activeColorMode)
    {
        bool shouldBeActive = (activeColorMode == objectColor);
        
        Debug.Log($"[ColorObject] {gameObject.name}: Color mode changed to {activeColorMode}. Object color: {objectColor}. Should be active: {shouldBeActive}");
        
        // Always update state to ensure colliders and material are in sync
        isActive = shouldBeActive;
        UpdateObjectState(shouldBeActive);
    }

    /// <summary>
    /// Updates the object's state based on whether it should be active.
    /// </summary>
    /// <param name="active">True if this object should be active</param>
    private void UpdateObjectState(bool active)
    {
        // Update material desaturation
        SetDesaturate(!active);

        // Update colliders - refresh the array each time to catch dynamically added colliders
        if (ToggleCollider)
        {
            // Get 3D colliders on this object and all children
            colliders = GetComponentsInChildren<Collider>(true);
            if (colliders != null && colliders.Length > 0)
            {
                // Determine collider state: invert if InvertColliderToggle is enabled
                bool shouldEnableCollider = InvertColliderToggle ? !active : active;
                
                foreach (Collider col in colliders)
                {
                    if (col != null)
                    {
                        bool wasEnabled = col.enabled;
                        col.enabled = shouldEnableCollider;
                        if (wasEnabled != shouldEnableCollider)
                        {
                            string behaviorType = InvertColliderToggle ? "HOLE" : "NORMAL";
                            Debug.Log($"[ColorObject] {gameObject.name}: Collider '{col.name}' ({col.GetType().Name}) {(shouldEnableCollider ? "ENABLED" : "DISABLED")} (Behavior: {behaviorType}, Color: {objectColor}, Active Mode: {ColorManager.instance?.CurrentColorMode}, Object Active: {active})");
                        }
                    }
                }
            }
            else
            {
                Debug.LogWarning($"[ColorObject] {gameObject.name}: ToggleCollider is enabled but no colliders found on this object or its children.");
            }
        }

        // Update GameObject active state
        if (toggleGameObject)
        {
            gameObject.SetActive(active);
        }

        // Invoke events
        if (active)
        {
            OnBecameActive?.Invoke();
        }
        else
        {
            OnBecameInactive?.Invoke();
        }
    }

    /// <summary>
    /// Sets the desaturate property on the material.
    /// </summary>
    /// <param name="desaturate">True to desaturate, false to show color</param>
    private void SetDesaturate(bool desaturate)
    {
        if (materialInstance != null)
        {
            if (materialInstance.HasProperty(DESATURATE_PROPERTY))
            {
                materialInstance.SetFloat(DESATURATE_PROPERTY, desaturate ? 1.0f : 0.0f);
            }
            else
            {
                Debug.LogWarning($"Material '{materialInstance.name}' on '{gameObject.name}' does not have the '{DESATURATE_PROPERTY}' property. Make sure it uses the Sprite-Unlit-Desaturate shader.");
            }
        }
    }

    /// <summary>
    /// Manually refresh this object's state based on the current color mode.
    /// Useful if you need to update after changing settings in the inspector.
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
