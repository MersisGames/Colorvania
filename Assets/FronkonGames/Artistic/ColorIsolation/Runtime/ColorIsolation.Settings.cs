////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// Copyright (c) Martin Bustos @FronkonGames <fronkongames@gmail.com>. All rights reserved.
//
// THIS FILE CAN NOT BE HOSTED IN PUBLIC REPOSITORIES.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace FronkonGames.Artistic.ColorIsolation
{
  ///------------------------------------------------------------------------------------------------------------------
  /// <summary> Settings. </summary>
  /// <remarks> Only available for Universal Render Pipeline. </remarks>
  ///------------------------------------------------------------------------------------------------------------------
  public sealed partial class ColorIsolation
  {
    /// <summary> Settings. </summary>
    [Serializable]
    public sealed class Settings
    {
      public Settings() => ResetDefaultValues();

      /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      #region Common settings.

      /// <summary> Controls the intensity of the effect [0, 1]. Default 1. </summary>
      /// <remarks> An effect with Intensity equal to 0 will not be executed. </remarks>
      public float intensity = 1.0f;
      #endregion
      /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

      /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      #region Color Isolation settings.

      /// <summary> Color to be isolated. Default Red. </summary>
      public Color isolatedColor = Color.red;

      /// <summary> Insulation accuracy, the less, the more accurate the color that is insulated [0, 1]. Default 0.1. </summary>
      public float isolatedThreshold = 0.1f;

      /// <summary> Factors modifying the importance of each channel of the CIELAB color space [0, 1]. Default 1. </summary>
      /// <remarks>
      /// Channel X: L* perceptual lightness.
      /// Channels Y and Z: the four unique colors of human vision: red, green, blue, and yellow.
      /// </remarks>
      public Vector3 factorLAB = Vector3.one;

      /// <summary> Color operation used to blend the isolated zone with the original color. Default Solid. </summary>
      public ColorBlends isolatedColorBlend = ColorBlends.Solid;

      /// <summary> Strength of color blend operation [0, 1]. Default 1. </summary>
      public float isolatedColorBlendStrength = 1.0f;

      /// <summary> Isolated zone tint. Default White. </summary>
      public Color isolatedTint = Color.white;

      /// <summary> Isolated color saturation [0, 2]. Default 1. </summary>
      public float isolatedSaturation = 1.0f;

      /// <summary> Isolated brightness [-1, 1]. Default 0. </summary>
      public float isolatedBrightness = 0.0f;

      /// <summary> Isolated contrast [0, 10]. Default 1. </summary>
      public float isolatedContrast = 1.0f;

      /// <summary> Isolated Gamma [0.1, 10]. Default 1. </summary>
      public float isolatedGamma = 1.0f;

      /// <summary> Isolated hue rotation [0, 1]. Default 0. </summary>
      public float isolatedHue = 0.0f;

      /// <summary> Isolated color invert [0, 1]. Default 0. </summary>
      public float isolatedInvert = 0.0f;

      /// <summary> Color operation used to blend the not isolated zone with the original color. Default Solid. </summary>
      public ColorBlends notIsolatedColorBlend = ColorBlends.Solid;

      /// <summary> Strength of color blend operation [0, 1]. Default 1. </summary>
      public float notIsolatedColorBlendStrength = 1.0f;

      /// <summary> Not isolated zone tint. Default White. </summary>
      public Color notIsolatedTint = Color.white;

      /// <summary> Not isolated color saturation [0, 2]. Default 1. </summary>
      public float notIsolatedSaturation = 1.0f;

      /// <summary> Not isolated brightness [-1, 1]. Default 0. </summary>
      public float notIsolatedBrightness = 0.0f;

      /// <summary> Not isolated contrast [0, 10]. Default 1. </summary>
      public float notIsolatedContrast = 1.0f;

      /// <summary> Not isolated Gamma [0.1, 5.0]. Default 1. </summary>
      public float notIsolatedGamma = 1.0f;

      /// <summary> Not isolated hue rotation [0, 1]. Default 0. </summary>
      public float notIsolatedHue = 0.0f;

      /// <summary> Not isolated color invert [0, 1]. Default 0. </summary>
      public float notIsolatedInvert = 0.0f;

      #endregion
      /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

      /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      #region Color settings.

      /// <summary> Brightness [-1.0, 1.0]. Default 0. </summary>
      public float brightness = 0.0f;

      /// <summary> Contrast [0.0, 10.0]. Default 1. </summary>
      public float contrast = 1.0f;

      /// <summary> Gamma [0.1, 10.0]. Default 1. </summary>
      public float gamma = 1.0f;

      /// <summary> The color wheel [0.0, 1.0]. Default 0. </summary>
      public float hue = 0.0f;

      /// <summary> Intensity of a colors [0.0, 2.0]. Default 1. </summary>
      public float saturation = 1.0f;
      #endregion
      /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

      /////////////////////////////////////////////////////////////////////////////////////////////////////////////////
      #region Advanced settings.
      /// <summary> Does it affect the Scene View? </summary>
      public bool affectSceneView = false;

#if !UNITY_6000_0_OR_NEWER
      /// <summary> Enable render pass profiling. </summary>
      public bool enableProfiling = false;

      /// <summary> Filter mode. Default Bilinear. </summary>
      public FilterMode filterMode = FilterMode.Bilinear;
#endif
      /// <summary> Render pass injection. Default BeforeRenderingPostProcessing. </summary>
      public RenderPassEvent whenToInsert = RenderPassEvent.BeforeRenderingPostProcessing;
      #endregion
      /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

      /// <summary> Reset to default values. </summary>
      public void ResetDefaultValues()
      {
        intensity = 1.0f;

        isolatedColor = Color.red;
        isolatedColorBlend = ColorBlends.Solid;
        factorLAB = Vector3.one;
        isolatedColorBlendStrength = 1.0f;
        isolatedThreshold = 0.1f;
        isolatedTint = Color.white;
        isolatedSaturation = 1.0f;
        isolatedBrightness = 0.0f;
        isolatedContrast = 1.0f;
        isolatedGamma = 1.0f;
        isolatedHue = 0.0f;
        isolatedInvert = 0.0f;
        notIsolatedTint = Color.white;
        notIsolatedColorBlend = ColorBlends.Solid;
        notIsolatedColorBlendStrength = 1.0f;
        notIsolatedSaturation = 0.0f;
        notIsolatedBrightness = 0.0f;
        notIsolatedContrast = 1.0f;
        notIsolatedGamma = 1.0f;
        notIsolatedHue = 0.0f;
        notIsolatedInvert = 0.0f;

        brightness = 0.0f;
        contrast = 1.0f;
        gamma = 1.0f;
        hue = 0.0f;
        saturation = 1.0f;

        affectSceneView = false;
#if !UNITY_6000_0_OR_NEWER
        enableProfiling = false;
        filterMode = FilterMode.Bilinear;
#endif
        whenToInsert = RenderPassEvent.BeforeRenderingPostProcessing;
      }
    }
  }
}
