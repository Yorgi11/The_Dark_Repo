Shader "Custom/Tex_Toon_Shadow" {
    Properties{
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Main Texture", 2D) = "white"{}
        _TexFactor("Texture Factor", float) = 1
        _RampTex("Ramp Texture", 2D) = "white"{}
    }

        SubShader{
            // Toon
            Tags{ "RenderType" = "Opaque" "Queue" = "Geometry"}
            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                // shadow
                #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight

                #include "UnityCG.cginc"
                // shadow
                #include "UnityLightingCommon.cginc"
                #include "Lighting.cginc" 
                #include "AutoLight.cginc"

                float4 _Color;
                float _TexFactor;
                sampler2D _MainTex;
                sampler2D _RampTex;

                struct appdata {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                    float3 normal : NORMAL;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    float3 worldNormal : TEXCOORD1;
                    float3 worldPos : TEXCOORD2;
                    float4 pos : SV_POSITION;
                    // shadow
                    SHADOW_COORDS(3)
                    //
                };

                v2f vert(appdata v) {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    o.worldNormal = UnityObjectToWorldNormal(v.normal);
                    o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                    // shadow
                    TRANSFER_SHADOW(o)
                    //
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    float diff = dot(i.worldNormal, _WorldSpaceLightPos0.xyz);
                    float h = diff * 0.5 + 0.5;
                    float2 rh = h;
                    float3 ramp = tex2D(_RampTex, rh).rgb;

                    fixed4 col = tex2D(_MainTex, i.uv);
                    col.rgb = col.rgb * _TexFactor * _Color.rgb * _LightColor0.rgb * ramp;
                    // shadow
                    fixed shadow = SHADOW_ATTENUATION(i);
                    //
                    return col * shadow;
                }

                ENDCG
            }
            Pass
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
            }
        }
        FallBack "Diffuse"
}
