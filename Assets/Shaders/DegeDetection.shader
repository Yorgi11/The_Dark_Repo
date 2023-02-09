// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom/Rim Lighting" {
    Properties{
        _MainTex("Albedo", 2D) = "white" {}
        _RimColor("Rim Color", Color) = (1, 1, 1, 1)
        _RimPower("Rim Power", Range(100, 1000)) = 300
    }

        SubShader{
            Tags {"RenderType" = "Opaque"}
            LOD 200

            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma shader_feature __ _NORMALMAP
                #include "UnityCG.cginc"

                struct appdata {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    float3 normal : TEXCOORD1;
                    float3 viewDir : TEXCOORD2;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                float3 _RimColor;
                float _RimPower;

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    float3 worldNormal = normalize(mul(float4(v.normal, 0), unity_ObjectToWorld).xyz);
                    float3 worldReflection = reflect(-UnityWorldSpaceViewDir(v.vertex), worldNormal);
                    o.normal = mul((float3x3)unity_WorldToObject, worldNormal);
                    o.viewDir = mul((float3x3)unity_WorldToObject, worldReflection).xyz;
                    return o;
                }

                float4 frag(v2f i) : SV_Target {
                    float3 rim = 1.0 - saturate(dot(i.normal, normalize(i.viewDir)));
                    rim = pow(rim, _RimPower * 100);
                    float4 col = tex2D(_MainTex, i.uv);
                    return float4(col * (1.0 - rim) + _RimColor.rgb * rim, 1.0);
                }
                ENDCG
            }
        }
            FallBack "Diffuse"
}
