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
using UnityEngine;
using UnityEditor;
using static FronkonGames.Artistic.ColorIsolation.Inspector;

namespace FronkonGames.Artistic.ColorIsolation.Editor
{
  /// <summary> Artistic Color Isolation inspector. </summary>
  [CustomPropertyDrawer(typeof(ColorIsolation.Settings))]
  public class ColorIsolationFeatureSettingsDrawer : Drawer
  {
    private ColorIsolation.Settings settings;

    protected override void ResetValues() => settings?.ResetDefaultValues();

    protected override void InspectorGUI()
    {
      settings ??= GetSettings<ColorIsolation.Settings>();

      /////////////////////////////////////////////////
      // Common.
      /////////////////////////////////////////////////
      settings.intensity = Slider("Intensity", "Controls the intensity of the effect [0, 1]. Default 0.", settings.intensity, 0.0f, 1.0f, 1.0f);

      /////////////////////////////////////////////////
      // Color Isolation.
      /////////////////////////////////////////////////
      Separator();

      settings.isolatedColor = ColorField("Isolated color", "Isolated color. Default Red.", settings.isolatedColor, Color.red);
      IndentLevel++;
      settings.isolatedThreshold = Slider("Threshold", "Insulation accuracy, the less, the more accurate the color that is insulated [0, 1]. Default 0.1.", settings.isolatedThreshold, 0.0f, 1.0f, 0.1f);
      IndentLevel++;
      settings.factorLAB.x = Slider("L*", "L* perceptual lightness [0, 1]. Default 1", settings.factorLAB.x, 0.0f, 1.0f, 1.0f);
      settings.factorLAB.y = Slider("a*", "Four unique colors of human vision: red, green, blue, and yellow [0, 1]. Default 1", settings.factorLAB.y, 0.0f, 1.0f, 1.0f);
      settings.factorLAB.z = Slider("b*", "Four unique colors of human vision: red, green, blue, and yellow [0, 1]. Default 1", settings.factorLAB.z, 0.0f, 1.0f, 1.0f);
      IndentLevel--;
      IndentLevel--;

      Label("Isolated zone");
      IndentLevel++;
      settings.isolatedTint = ColorField("Tint", "Isolated zone tint. Default White.", settings.isolatedTint, Color.white);
      settings.isolatedColorBlend = (ColorBlends)EnumPopup("Color blend", "Color operation used to blend the isolated zone with the original color. Default Solid.", settings.isolatedColorBlend, ColorBlends.Solid);
      IndentLevel++;
      settings.isolatedColorBlendStrength = Slider("Strength", "Strength of color blend operation [0, 1]. Default 1.", settings.isolatedColorBlendStrength, 0.0f, 1.0f, 1.0f);
      IndentLevel--;
      settings.isolatedBrightness = Slider("Brightness", "Isolated brightness [-1, 1]. Default 0.", settings.isolatedBrightness, -1.0f, 1.0f, 0.0f);
      settings.isolatedContrast = Slider("Contrast", "Isolated contrast [0, 10]. Default 1.", settings.isolatedContrast, 0.0f, 10.0f, 1.0f);
      settings.isolatedGamma = Slider("Gamma", "Isolated Gamma [0.1, 10]. Default 1.", settings.isolatedGamma, 0.01f, 10.0f, 1.0f);
      settings.isolatedHue = Slider("Hue", "Isolated hue rotation [0, 1]. Default 0.", settings.isolatedHue, 0.0f, 1.0f, 0.0f);
      settings.isolatedSaturation = Slider("Saturation", "Isolated color saturation [0, 2]. Default 1.", settings.isolatedSaturation, 0.0f, 2.0f, 1.0f);
      settings.isolatedInvert = Slider("Invert", "Isolated color invert [0, 1]. Default 0.", settings.isolatedInvert, 0.0f, 1.0f, 0.0f);
      IndentLevel--;

      Label("Not isolated zone");
      IndentLevel++;
      settings.notIsolatedTint = ColorField("Tint", "Not isolated zone tint. Default White.", settings.notIsolatedTint, Color.white);
      settings.notIsolatedColorBlend = (ColorBlends)EnumPopup("Color blend", "Color operation used to blend the not isolated zone with the original color. Default Solid.", settings.notIsolatedColorBlend, ColorBlends.Solid);
      IndentLevel++;
      settings.notIsolatedColorBlendStrength = Slider("Strength", "Strength of color blend operation [0, 1]. Default 1.", settings.notIsolatedColorBlendStrength, 0.0f, 1.0f, 1.0f);
      IndentLevel--;
      settings.notIsolatedBrightness = Slider("Brightness", "Not isolated brightness [-1, 1]. Default 0.", settings.notIsolatedBrightness, -1.0f, 1.0f, 0.0f);
      settings.notIsolatedContrast = Slider("Contrast", "Not isolated contrast [0, 10]. Default 1.", settings.notIsolatedContrast, 0.0f, 10.0f, 1.0f);
      settings.notIsolatedGamma = Slider("Gamma", "Not isolated Gamma [0.1, 10]. Default 1.", settings.notIsolatedGamma, 0.01f, 10.0f, 1.0f);
      settings.notIsolatedHue = Slider("Hue", "Not isolated hue rotation [0, 1]. Default 0.", settings.notIsolatedHue, 0.0f, 1.0f, 0.0f);
      settings.notIsolatedSaturation = Slider("Saturation", "Not isolated color saturation [0, 2]. Default 1.", settings.notIsolatedSaturation, 0.0f, 2.0f, 1.0f);
      settings.notIsolatedInvert = Slider("Invert", "Not isolated color invert [0, 1]. Default 0.", settings.notIsolatedInvert, 0.0f, 1.0f, 0.0f);
      IndentLevel--;

      /////////////////////////////////////////////////
      // Color.
      /////////////////////////////////////////////////
      Separator();

      if (Foldout("Color") == true)
      {
        IndentLevel++;

        settings.brightness = Slider("Brightness", "Brightness [-1.0, 1.0]. Default 0.", settings.brightness, -1.0f, 1.0f, 0.0f);
        settings.contrast = Slider("Contrast", "Contrast [0.0, 10.0]. Default 1.", settings.contrast, 0.0f, 10.0f, 1.0f);
        settings.gamma = Slider("Gamma", "Gamma [0.1, 10.0]. Default 1.", settings.gamma, 0.01f, 10.0f, 1.0f);
        settings.hue = Slider("Hue", "The color wheel [0.0, 1.0]. Default 0.", settings.hue, 0.0f, 1.0f, 0.0f);
        settings.saturation = Slider("Saturation", "Intensity of a colors [0.0, 2.0]. Default 1.", settings.saturation, 0.0f, 2.0f, 1.0f);

        IndentLevel--;
      }

      /////////////////////////////////////////////////
      // Advanced.
      /////////////////////////////////////////////////
      Separator();

      if (Foldout("Advanced") == true)
      {
        IndentLevel++;

#if !UNITY_6000_0_OR_NEWER
        settings.filterMode = (FilterMode)EnumPopup("Filter mode", "Filter mode. Default Bilinear.", settings.filterMode, FilterMode.Bilinear);
#endif
        settings.affectSceneView = Toggle("Affect the Scene View?", "Does it affect the Scene View?", settings.affectSceneView);
        settings.whenToInsert = (UnityEngine.Rendering.Universal.RenderPassEvent)EnumPopup("RenderPass event",
          "Render pass injection. Default BeforeRenderingPostProcessing.",
          settings.whenToInsert,
          UnityEngine.Rendering.Universal.RenderPassEvent.BeforeRenderingPostProcessing);
#if !UNITY_6000_0_OR_NEWER
        settings.enableProfiling = Toggle("Enable profiling", "Enable render pass profiling", settings.enableProfiling);
#endif

        IndentLevel--;
      }
    }
  }
}
