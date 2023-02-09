Shader "Custom/CartoonInk" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _Thickness("Thickness", Float) = 0.2
        _Color("Color", Color) = (1,1,1,1)
    }

        SubShader{
            Tags {"RenderType" = "Opaque"}
            LOD 200

            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float _Thickness;
                float4 _Color;

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                float4 frag(v2f i) : SV_Target {
                    float4 col = tex2D(_MainTex, i.uv);
                    float d = _Thickness / 100.0;
                    float2 uv_shift = i.uv + float2(d, d);
                    float4 col2 = tex2D(_MainTex, uv_shift);
                    float4 result = lerp(col, _Color, col2.a - col.a);
                    return result;
                }
                ENDCG
            }
        }
            FallBack "Diffuse"
}
