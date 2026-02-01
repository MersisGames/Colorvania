// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE
// WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
// COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR
// OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
Shader "Hidden/Fronkon Games/Artistic/Color Isolation URP"
{
  Properties
  {
    _MainTex("Main Texture", 2D) = "white" {}
  }

  SubShader
  {
    Tags
    {
      "RenderType" = "Opaque"
      "RenderPipeline" = "UniversalPipeline"
    }
    LOD 100
    ZTest Always ZWrite Off Cull Off

    Pass
    {
      Name "Fronkon Games Artistic Color Isolation"

      HLSLPROGRAM
      #pragma vertex ArtisticVert
      #pragma fragment ArtisticFrag
      #pragma fragmentoption ARB_precision_hint_fastest
      #pragma exclude_renderers d3d9 d3d11_9x ps3 flash

      #include "Artistic.hlsl"
      #include "ColorBlend.hlsl"

      half3 _IsolatedColor;
      float _IsolatedThreshold;
      half3 _FactorLAB;

      half3 _SelectedTint;
      int _SelectedColorBlend;
      float _SelectedColorBlendStrength;
      float _SelectedSaturation;
      float _SelectedBrightness;
      float _SelectedContrast;
      float _SelectedGamma;
      float _SelectedHue;
      float _SelectedInvert;

      half3 _UnselectedTint;
      int _UnselectedColorBlend;
      float _UnselectedColorBlendStrength;
      float _UnselectedSaturation;
      float _UnselectedBrightness;
      float _UnselectedContrast;
      float _UnselectedGamma;
      float _UnselectedHue;
      float _UnselectedInvert;

      inline float RGBChannel(float channel)
      {
        return 100.0 * (channel > 0.4045 ? pow(abs((channel + 0.055) / 1.055), 2.4) : channel / 12.92);
      }

      inline float XYZChannel(float channel)
      {
        return channel > 0.008856 ? pow(abs(channel), 1.0 / 3.0) : (7.787 * channel) + (16.0 / 116.0);
      }

      float3 RGB2LAB(float3 color)
      {
        float r = RGBChannel(color.x);
        float g = RGBChannel(color.y);
        float b = RGBChannel(color.z);

        float x = r * 0.4124 + g * 0.3576 + b * 0.1805;
        float y = r * 0.2126 + g * 0.7152 + b * 0.0722;
        float z = r * 0.0193 + g * 0.1192 + b * 0.9505;

        x = XYZChannel(x / 95.0470);
        y = XYZChannel(y / 100.0);
        z = XYZChannel(z / 108.883);

        return saturate(float3(_FactorLAB.x * round((116.0 * y) - 16.0),  // L
                               _FactorLAB.y * round(500.0 * (x - y)),     // a
                               _FactorLAB.z * round(200.0 * (y - z))));   // b
      }

      half4 ArtisticFrag(ArtisticVaryings input) : SV_Target
      {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
        const float2 uv = UnityStereoTransformScreenSpaceTex(input.texcoord).xy;
        const half4 color = SAMPLE_MAIN(uv);
        half4 pixel = color;

        const float3 lab1 = RGB2LAB(pixel.rgb);
        const float3 lab2 = RGB2LAB(_IsolatedColor);
        const float distance = sqrt(pow(abs(lab1.x - lab2).x, 2.0) + pow(abs(lab1.y - lab2.y), 2.0) + pow(abs(lab1.z - lab2.z), 2.0));

        const float isolation = length(abs(distance)) < _IsolatedThreshold ? 1.0 : 0.0;

        half3 selected = ColorAdjust(pixel.rgb, _SelectedContrast, _SelectedBrightness, _SelectedHue, _SelectedGamma, _SelectedSaturation);
        selected = lerp(selected, (half3)1.0 - selected, _SelectedInvert);
        selected = lerp(selected, ColorBlend(_SelectedColorBlend, color.rgb, selected), _SelectedColorBlendStrength);
        selected *= _SelectedTint;

        half3 unselected = ColorAdjust(pixel.rgb, _UnselectedContrast, _UnselectedBrightness, _UnselectedHue, _UnselectedGamma, _UnselectedSaturation);
        unselected = lerp(unselected, ColorBlend(_UnselectedColorBlend, color.rgb, unselected), _UnselectedColorBlendStrength);
        unselected = lerp(unselected, (half3)1.0 - unselected, _UnselectedInvert);
        unselected *= _UnselectedTint;

        pixel.rgb = lerp(unselected, selected, isolation);

        // Color adjust.
        pixel.rgb = ColorAdjust(pixel.rgb, _Contrast, _Brightness, _Hue, _Gamma, _Saturation);

        return lerp(color, pixel, _Intensity);
      }
      ENDHLSL
    }
  }
  
  FallBack "Diffuse"
}
