// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/VortexShader"
{
    Properties
    {
        _MainTex("Base (RGB)", 2D) = "white" {}
    }

     SubShader{
            Pass {
                ZTest Always Cull Off ZWrite Off
                Fog { Mode off }

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma fragmentoption ARB_precision_hint_fastest 
                #include "UnityCG.cginc"

                uniform sampler2D _MainTex;
                uniform float2 _Position;
                uniform float _Rad;
                uniform float _Ratio;
                uniform float _Distance;

                struct v2f {
                    float4 pos : POSITION;
                    float2 uv : TEXCOORD0;
                };

                v2f vert(appdata_img v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv = v.texcoord;
                    return o;
                }

                float4 frag(v2f i) : COLOR
                {
                    float2 center = float2(0.5, 0.25);
                    float radius = 0.1;
                    float deformationRadius = 0.005;
                    // Normalized pixel coordinates (from 0 to 1)
                    float2 uv = fragCoord / iResolution.xy;

                    float ratio = iResolution.y / iResolution.x;
                    center = iMouse.xy / iResolution.xy;

                    float2 pos = float2(uv.x, uv.y);

                    float4 tex = texture(iChannel0, uv);
                    float2 offset = uv.xy - center;

                    float rad = length(offset / ratio);

                    float deformation = 1.0 / pow(rad * pow(deformationRadius, 0.1), 2.0) * 0.01 * 2.0;

                    offset = offset * (1.0 - deformation);

                    offset += center;

                    //offset.y * ratio;

                    float4 color = distance(pos, center) > radius ? texture(iChannel0, offset) : _MainTex.rgba;

                    // Output to screen
                    fragColor = float4(color);
                }
                ENDCG

            }
     }
        Fallback off
}