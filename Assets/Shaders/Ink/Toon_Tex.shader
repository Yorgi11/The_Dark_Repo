Shader "Custom/Tex_Toon"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _ShadowColor("ShadowColor", Color) = (0,0,0,1)
        _MainTex("Main Texture", 2D) = "white" {}
        _RampTex("Ramp Texture", 2D) = "white" {}
        _OutColor("Outline Color", Color) = (1,1,1,1)
        _Thickness("Thickness", Range(0.002, 1)) = 0.005
    }

        //SubShader
        //{
        //    Pass
        //    {
        //        Tags {"LightMode" = "ForwardBase"}

        //        CGPROGRAM
        //        #pragma vertex vert
        //        #pragma fragment frag
        //        #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight

        //        #include "UnityCG.cginc"
        //        #include "UnityLightingCommon.cginc"
        //        #include "Lighting.cginc" 
        //        #include "AutoLight.cginc"
        //        struct appdata
        //        {
        //            float4 vertex : POSITION;
        //            float3 normal : Normal;
        //            float2 texcord : TEXCOORD0;
        //        };

        //        struct v2f
        //        {
        //            float2 uv : TEXCOORD0;
        //            float4 diff : COLOR0;
        //            float4 pos : SV_POSITION;
        //            SHADOW_COORDS(1)
        //        };

        //        sampler2D _MainTex, _RampTex;
        //        float4 _Color;

        //        v2f vert(appdata v)
        //        {
        //            v2f o;
        //            o.pos = UnityObjectToClipPos(v.vertex);
        //            o.uv = v.texcord;
        //            half3 worldNormal = UnityObjectToWorldNormal(v.normal);
        //            half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
        //            o.diff = (nl * 0.5 + 0.5) * _LightColor0;
        //            TRANSFER_SHADOW(o)
        //            return o;
        //        }

        //        fixed4 frag(v2f i) : SV_Target
        //        {
        //            float3 ramp = tex2D(_RampTex, i.diff).rgb * _Color;
        //            fixed shadow = SHADOW_ATTENUATION(i);
        //            ramp *= shadow;
        //            return float4(ramp,1);
        //        }
        //        ENDCG
        //    }
        //    // outlinePass 1
        //    Pass
        //    {
        //        Cull Front
        //        CGPROGRAM
        //            #pragma vertex vert
        //            #pragma fragment frag

        //            #include "UnityCG.cginc"

        //            struct appdata {
        //                float4 vertex : POSITION;
        //                float3 normal : NORMAL;
        //            };

        //            struct v2f {
        //                float4 pos : SV_POSITION;
        //                fixed4 color : COLOR0;
        //            };

        //            float _Thickness;
        //            float4 _OutColor;

        //            v2f vert(appdata v) {
        //                v2f o;
        //                o.pos = UnityObjectToClipPos(v.vertex);

        //                float3 norm = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
        //                float2 offset = TransformViewToProjection(norm.xy);

        //                o.pos.xy += offset * o.pos.z * _Thickness;
        //                o.color = _OutColor;
        //                return o;
        //            }

        //            fixed4 frag(v2f i) : SV_Target
        //            {
        //               return i.color;
        //            }
        //        ENDCG
        //    }
        //    // outlinePass 2
        //    Pass
        //    {
        //        Cull Front
        //        CGPROGRAM
        //            #pragma vertex vert
        //            #pragma fragment frag

        //            #include "UnityCG.cginc"

        //            struct appdata {
        //                float4 vertex : POSITION;
        //                float3 normal : NORMAL;
        //            };

        //            struct v2f {
        //                float4 pos : SV_POSITION;
        //                fixed4 color : COLOR;
        //            };

        //            float _Thickness;
        //            float4 _OutColor;

        //            v2f vert(appdata v) {
        //                v2f o;
        //                o.pos = UnityObjectToClipPos(v.vertex);

        //                float3 norm = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
        //                float2 offset = TransformViewToProjection(norm.xy);

        //                o.pos.xy += offset * o.pos.z * _Thickness * 0.35;
        //                o.color = _OutColor;
        //                return o;
        //            }

        //            fixed4 frag(v2f i) : SV_Target
        //            {
        //               return i.color;
        //            }
        //        ENDCG
        //    }
        //    Pass
        //    {
        //        Tags {"LightMode" = "ShadowCaster"}
        //        CGPROGRAM
        //        #pragma vertex vert
        //        #pragma fragment frag
        //        #pragma multi_compile_shadowcaster
        //        #include "UnityCG.cginc"
        //        struct appdata {
        //            float4 vertex : POSITION;
        //            float3 normal : NORMAL;
        //            float4 texcoord : TEXCOORD0;
        //        };
        //        struct v2f {
        //            V2F_SHADOW_CASTER;
        //        };
        //        v2f vert(appdata v)
        //        {
        //            v2f o;
        //            TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
        //            return o;
        //        }
        //        float4 frag(v2f i) : SV_Target
        //        {
        //            SHADOW_CASTER_FRAGMENT(i)
        //        }
        //        ENDCG
        //    }
        //}
        SubShader{
            // Toon
            Tags{ "RenderType" = "Opaque" "Queue" = "Geometry"}

                CGPROGRAM
                #pragma surface surf ToonRamp fullforwardshadows
                #pragma target 3.0

                float4 _Color, _ShadowColor;
                sampler2D _RampTex;
                sampler2D _MainTex;

                struct Input
                {
                    float2 uv_MainTex;
                };

                float4 LightingToonRamp(SurfaceOutput s, fixed3 lightDir, half3 viewDir, float atten) {
                    float diff = dot(s.Normal, lightDir);
                    float h = diff * 0.5 + 0.5;
                    float2 rh = h;
                    float3 ramp = tex2D(_RampTex, rh).rgb;
                    
                    float lightChange = fwidth(diff);
                    float lightIntensity = smoothstep(0, lightChange, diff);

                    #ifdef USING_DIRECTIONAL_LIGHT
                    float attenuationChange = fwidth(atten) * 0.5;
                    float shadow = smoothstep(0.5 - attenuationChange, 0.5 + attenuationChange, atten);
                    #else
                    float attenuationChange = fwidth(atten);
                    float shadow = smoothstep(0, attenuationChange, atten);
                    #endif
                    lightIntensity = lightIntensity * shadow;

                    float3 shadcol = s.Albedo * _ShadowColor;

                    float4 c;
                    c.rgb = lerp(shadcol, s.Albedo, lightIntensity) * _LightColor0.rgb * ramp;
                    c.a = s.Alpha;
                    return c;
                }

                void surf(Input IN, inout SurfaceOutput o)
                {
                    //o.Albedo = _Color.rgb;
                    o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _Color.rgb;
                }
                ENDCG
        }
        FallBack "Standard"
}