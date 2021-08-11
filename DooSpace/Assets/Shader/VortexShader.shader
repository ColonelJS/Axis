// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Hidden/VortexShader"
{
    Properties{
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
                    float2 offset = i.uv - _Position; // We shift our pixel to the desired position
                    float2 ratio = {_Ratio,1}; // determines the aspect ratio
                    float rad = length(offset / ratio); // the distance from the conventional "center" of the screen.
                    float deformation = 1 / pow(rad * pow(_Distance,0.5),2) * _Rad * 2;

                    offset = offset * (1 - deformation);

                    offset += _Position;

                    half4 res = tex2D(_MainTex, offset);
                    //if (rad*_Distance<pow(2*_Rad/_Distance,0.5)*_Distance) {res.g+=0.2;} // verification of compliance with the Einstein radius
                    //if (rad*_Distance<_Rad){res.r=0;res.g=0;res.b=0;} // check radius BH
                    return res;
                }
                ENDCG

            }
    }

        Fallback off

}
