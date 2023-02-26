Shader "Custom/Tex_Toon_Shadow" {
    Properties{
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Main Texture", 2D) = "white"{}
        _TexFactor("Texture Factor", Range(0.0, 1.0)) = 1
        _RampTex("Ramp Texture", 2D) = "white"{}
    }
        SubShader{
            Pass{
                Tags{ "RenderType" = "Opaque" "Queue" = "Geometry" }
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                // shadow
                #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight

                #include "UnityCG.cginc"
                #include "UnityLightingCommon.cginc"
                // shadow
                #include "Lighting.cginc" 
                #include "AutoLight.cginc"

                struct appdata {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    float3 normal : NORMAL;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    float3 worldNormal : TEXCOORD1;
                    float3 worldPos : TEXCOORD2;
                    half4 vertex : SV_POSITION;
                    // shadow
                    SHADOW_COORDS(1)
                    //
                };

                float _TexFactor;
                float4 _Color;
                sampler2D _RampTex;
                sampler2D _MainTex;

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    o.worldNormal = UnityObjectToWorldNormal(v.normal);
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                    // shadow
                    TRANSFER_SHADOW(o)
                    //
                    return o;
                }

                float4 frag(v2f i) : SV_Target {
                    float4 tex = tex2D(_MainTex, i.uv) * _TexFactor;
                    float4 c;
                    c.rgb = tex.rgb * _Color.rgb;
                    c.a = tex.a * _Color.a;

                    float3 worldNormal = normalize(i.worldNormal);
                    float3 worldLightDir = normalize(_WorldSpaceLightPos0.xyz - i.worldPos);
                    half3 ramp = tex2D(_RampTex, float2(dot(worldNormal, worldLightDir) * 0.5 + 0.5, 0.5)).rgb;
                    // shadow
                    fixed shadow = SHADOW_ATTENUATION(i);
                    //
                    c.rgb *= ramp * shadow;

                    return c;
                }
                ENDCG
            }
            /*Pass
            {
                Tags {"LightMode" = "ShadowCaster"}
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_shadowcaster
                #include "UnityCG.cginc"
                struct appdata {
                    float4 vertex : POSITION;
                    float3 normal : NORMAL;
                    float4 texcoord : TEXCOORD0;
                };
                struct v2f {
                    V2F_SHADOW_CASTER;
                };
                v2f vert(appdata v)
                {
                    v2f o;
                    TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                    return o;
                }
                float4 frag(v2f i) : SV_Target
                {
                    SHADOW_CASTER_FRAGMENT(i)
                }
                ENDCG
            }*/
        }
        FallBack "Diffuse"
}
