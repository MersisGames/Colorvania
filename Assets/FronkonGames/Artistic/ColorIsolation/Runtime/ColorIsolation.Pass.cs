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
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
#if UNITY_6000_0_OR_NEWER
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;
#endif

namespace FronkonGames.Artistic.ColorIsolation
{
  ///------------------------------------------------------------------------------------------------------------------
  /// <summary> Render Pass. </summary>
  /// <remarks> Only available for Universal Render Pipeline. </remarks>
  ///------------------------------------------------------------------------------------------------------------------
  public sealed partial class ColorIsolation
  {
    [DisallowMultipleRendererFeature]
    private sealed class RenderPass : ScriptableRenderPass
    {
      // Internal use only.
      internal Material material { get; set; }

      private readonly Settings settings;

#if UNITY_6000_0_OR_NEWER
#else
      private RenderTargetIdentifier colorBuffer;
      private RenderTextureDescriptor renderTextureDescriptor;

      private readonly int renderTextureHandle0 = Shader.PropertyToID($"{Constants.Asset.AssemblyName}.RTH0");

      private const string CommandBufferName = Constants.Asset.AssemblyName;

      private ProfilingScope profilingScope;
      private readonly ProfilingSampler profilingSamples = new(Constants.Asset.AssemblyName);
#endif

      private static class ShaderIDs
      {
        internal static readonly int Intensity = Shader.PropertyToID("_Intensity");

        internal static readonly int IsolatedColor = Shader.PropertyToID("_IsolatedColor");
        internal static readonly int IsolatedThreshold = Shader.PropertyToID("_IsolatedThreshold");
        internal static readonly int FactorLAB = Shader.PropertyToID("_FactorLAB");

        internal static readonly int SelectedTint = Shader.PropertyToID("_SelectedTint");
        internal static readonly int SelectedColorBlend = Shader.PropertyToID("_SelectedColorBlend");
        internal static readonly int SelectedColorBlendStrength = Shader.PropertyToID("_SelectedColorBlendStrength");
        internal static readonly int SelectedSaturation = Shader.PropertyToID("_SelectedSaturation");
        internal static readonly int SelectedBrightness = Shader.PropertyToID("_SelectedBrightness");
        internal static readonly int SelectedContrast = Shader.PropertyToID("_SelectedContrast");
        internal static readonly int SelectedGamma = Shader.PropertyToID("_SelectedGamma");
        internal static readonly int SelectedHue = Shader.PropertyToID("_SelectedHue");
        internal static readonly int SelectedInvert = Shader.PropertyToID("_SelectedInvert");

        internal static readonly int UnselectedTint = Shader.PropertyToID("_UnselectedTint");
        internal static readonly int UnselectedColorBlend = Shader.PropertyToID("_UnselectedColorBlend");
        internal static readonly int UnselectedColorBlendStrength = Shader.PropertyToID("_UnselectedColorBlendStrength");
        internal static readonly int UnselectedSaturation = Shader.PropertyToID("_UnselectedSaturation");
        internal static readonly int UnselectedBrightness = Shader.PropertyToID("_UnselectedBrightness");
        internal static readonly int UnselectedContrast = Shader.PropertyToID("_UnselectedContrast");
        internal static readonly int UnselectedGamma = Shader.PropertyToID("_UnselectedGamma");
        internal static readonly int UnselectedHue = Shader.PropertyToID("_UnselectedHue");
        internal static readonly int UnselectedInvert = Shader.PropertyToID("_UnselectedInvert");

        internal static readonly int Brightness = Shader.PropertyToID("_Brightness");
        internal static readonly int Contrast = Shader.PropertyToID("_Contrast");
        internal static readonly int Gamma = Shader.PropertyToID("_Gamma");
        internal static readonly int Hue = Shader.PropertyToID("_Hue");
        internal static readonly int Saturation = Shader.PropertyToID("_Saturation");
      }

      private static class Keywords
      {
        internal static readonly string DebugView = "DEBUG_VIEW";
      }

      /// <summary> Render pass constructor. </summary>
      public RenderPass(Settings settings) : base()
      {
        this.settings = settings;
#if UNITY_6000_0_OR_NEWER
        profilingSampler = new ProfilingSampler(Constants.Asset.AssemblyName);
#endif
      }

      /// <summary> Destroy the render pass. </summary>
      ~RenderPass() => material = null;

      private void UpdateMaterial()
      {
        material.shaderKeywords = null;
        material.SetFloat(ShaderIDs.Intensity, settings.intensity);

        material.SetColor(ShaderIDs.IsolatedColor, settings.isolatedColor);
        material.SetFloat(ShaderIDs.IsolatedThreshold, settings.isolatedThreshold);
        material.SetVector(ShaderIDs.FactorLAB, settings.factorLAB);

        material.SetColor(ShaderIDs.SelectedTint, settings.isolatedTint);
        material.SetInt(ShaderIDs.SelectedColorBlend, (int)settings.isolatedColorBlend);
        material.SetFloat(ShaderIDs.SelectedColorBlendStrength, settings.isolatedColorBlendStrength);
        material.SetFloat(ShaderIDs.SelectedSaturation, settings.isolatedSaturation);
        material.SetFloat(ShaderIDs.SelectedBrightness, settings.isolatedBrightness);
        material.SetFloat(ShaderIDs.SelectedContrast, settings.isolatedContrast);
        material.SetFloat(ShaderIDs.SelectedGamma, 1.0f / settings.isolatedGamma);
        material.SetFloat(ShaderIDs.SelectedHue, settings.isolatedHue);
        material.SetFloat(ShaderIDs.SelectedInvert, settings.isolatedInvert);

        material.SetColor(ShaderIDs.UnselectedTint, settings.notIsolatedTint);
        material.SetInt(ShaderIDs.UnselectedColorBlend, (int)settings.notIsolatedColorBlend);
        material.SetFloat(ShaderIDs.UnselectedColorBlendStrength, settings.notIsolatedColorBlendStrength);
        material.SetFloat(ShaderIDs.UnselectedSaturation, settings.notIsolatedSaturation);
        material.SetFloat(ShaderIDs.UnselectedBrightness, settings.notIsolatedBrightness);
        material.SetFloat(ShaderIDs.UnselectedContrast, settings.notIsolatedContrast);
        material.SetFloat(ShaderIDs.UnselectedGamma, 1.0f / settings.notIsolatedGamma);
        material.SetFloat(ShaderIDs.UnselectedHue, settings.notIsolatedHue);
        material.SetFloat(ShaderIDs.UnselectedInvert, settings.notIsolatedInvert);

        material.SetFloat(ShaderIDs.Brightness, settings.brightness);
        material.SetFloat(ShaderIDs.Contrast, settings.contrast);
        material.SetFloat(ShaderIDs.Gamma, 1.0f / settings.gamma);
        material.SetFloat(ShaderIDs.Hue, settings.hue);
        material.SetFloat(ShaderIDs.Saturation, settings.saturation);
      }

#if UNITY_6000_0_OR_NEWER
      /// <inheritdoc/>
      public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
      {
        if (material == null || settings.intensity == 0.0f)
          return;

        UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
        if (resourceData.isActiveTargetBackBuffer == true)
          return;

        UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();
        if (cameraData.camera.cameraType == CameraType.SceneView && settings.affectSceneView == false || cameraData.postProcessEnabled == false)
          return;

        TextureHandle source = resourceData.activeColorTexture;
        TextureHandle destination = renderGraph.CreateTexture(source.GetDescriptor(renderGraph));

        UpdateMaterial();

        RenderGraphUtils.BlitMaterialParameters pass = new(source, destination, material, 0);
        renderGraph.AddBlitPass(pass, $"{Constants.Asset.AssemblyName}.Pass");

        resourceData.cameraColor = destination;
      }
#elif UNITY_2022_3_OR_NEWER
      /// <inheritdoc/>
      public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
      {
        renderTextureDescriptor = renderingData.cameraData.cameraTargetDescriptor;
        renderTextureDescriptor.depthBufferBits = 0;

        colorBuffer = renderingData.cameraData.renderer.cameraColorTargetHandle;
        cmd.GetTemporaryRT(renderTextureHandle0, renderTextureDescriptor, settings.filterMode);
      }

      /// <inheritdoc/>
      public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
      {
        if (material == null ||
            renderingData.postProcessingEnabled == false ||
            settings.intensity == 0.0f ||
            settings.affectSceneView == false && renderingData.cameraData.isSceneViewCamera == true)
          return;

        CommandBuffer cmd = CommandBufferPool.Get(CommandBufferName);

        if (settings.enableProfiling == true)
          profilingScope = new ProfilingScope(cmd, profilingSamples);

        UpdateMaterial();

        cmd.Blit(colorBuffer, renderTextureHandle0, material);
        cmd.Blit(renderTextureHandle0, colorBuffer);

        cmd.ReleaseTemporaryRT(renderTextureHandle0);

        if (settings.enableProfiling == true)
          profilingScope.Dispose();

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
      }

      public override void OnCameraCleanup(CommandBuffer cmd) => cmd.ReleaseTemporaryRT(renderTextureHandle0);
#else
      #error Unsupported Unity version. Please update to a newer version of Unity.
#endif
    }
  }
}
