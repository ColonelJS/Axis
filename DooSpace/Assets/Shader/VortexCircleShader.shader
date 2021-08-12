// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/VortexCircleShader"
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
                uniform float _Deformation;
                  
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
                    float radius;
                    float deformationRadius;
                    float ratio;
                    float4 tex;
                    float2 offset;
                    float rad;
                    float deformation;
                    float4 color;

                    radius = 0.15;
                    deformationRadius = 1.2;
                    // Normalized pixel coordinates (from 0 to 1)

                    ratio = 2.22;

                    tex = tex2D(_MainTex, i.uv);
                    offset = i.uv.xy - _Position;

                    rad = length(offset / ratio);

                    deformation = 1.0 / pow(rad * pow(_Rad, 10), _Deformation) * 0.01 * 2.0;

                    offset = offset * (1.0-deformation);

                    offset += _Position;

                    //offset.y / ratio;

                    float4 offsetCol = tex2D(_MainTex, offset);

                    //color = distance(i.uv, _Position) > radius ? offsetCol : tex.rgba + float4(pow(radius, 1.0/distance(i.uv, _Position)* radius), 0.0, 0.0, 1.0);
                    color = distance(i.uv, _Position) > radius ? offsetCol : tex.rgba + float4(0.0, 0.0, 0.0, 0.0);
                    // Output to screen
                    return float4(color);
                }
                ENDCG

            }
     }
        Fallback off
}