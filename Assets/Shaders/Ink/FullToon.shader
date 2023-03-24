Shader "Custom/FullToon"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _ShadowColor("ShadowColor", Color) = (1,1,1,1)
        _MainTex("Main Texture", 2D) = "white" {}
        _RampTex("Ramp Texture", 2D) = "white" {}
        _OutColor("Outline Color", Color) = (1,1,1,1)
        _Thickness("Thickness", Range(0.002, 1)) = 1
        _ShadowVal("shadVal", Range(-1, 1)) = 1
    }
        SubShader{
            // Toon
                Tags{ "RenderType" = "Opaque" "Queue" = "Geometry"}
                CGPROGRAM
                    #pragma surface surf ToonRamp addshadow fullforwardshadows
                    #pragma target 3.0

                    float4 _Color;
                    float3 _ShadowColor;
                    float _ShadowVal;
                    sampler2D _RampTex;
                    sampler2D _MainTex;

                    struct Input
                    {
                        float2 uv_MainTex;
                    };

                    float4 LightingToonRamp(SurfaceOutput s, float3 lightDir, half3 viewDir, float atten) {
                        float diff = dot(s.Normal, lightDir);

                        float h = diff * 0.5 + 0.5;
                        float2 rh = h;
                        float3 ramp = tex2D(_RampTex, rh).rgb;

                        float lightChange = fwidth(diff);
                        float lightIntensity = smoothstep(0, lightChange, diff);

                    #ifdef USING_DIRECTIONAL_LIGHT
                        float attenuationChange = fwidth(atten);
                        float shadow = smoothstep(0, 1 + attenuationChange, atten);
                    #else
                        float attenuationChange = fwidth(atten);
                        float shadow = smoothstep(0, attenuationChange, atten);
                    #endif
                        lightIntensity = lightIntensity * shadow;

                        float3 shadcol = s.Albedo * _ShadowColor;
    
                        float4 c;
                        c.rgb = lerp(shadcol, s.Albedo , lightIntensity) * _LightColor0.rgb * ramp;
                        c.a = s.Alpha;
                        return c;
                    }
                    
                    void surf(Input IN, inout SurfaceOutput o)
                    {
                        //o.Albedo = _Color.rgb;
                        o.Albedo = (tex2D(_MainTex, IN.uv_MainTex) * _Color).rgb;
                    }
                ENDCG
            Pass // outlinePass 1
            {
                Cull Front
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                    struct appdata {
                        float4 vertex : POSITION;
                        float3 normal : NORMAL;
                    };
                    struct v2f {
                        float4 pos : SV_POSITION;
                        fixed4 color : COLOR0;
                    };
                    float _Thickness;
                    float4 _OutColor;
                    v2f vert(appdata v) {
                        v2f o;
                        o.pos = UnityObjectToClipPos(v.vertex);
                        float3 norm = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
                        float2 offset = TransformViewToProjection(norm.xy);
                        o.pos.xy += offset * o.pos.z * _Thickness;
                        o.color = _OutColor;
                        return o;
                    }
                    fixed4 frag(v2f i) : SV_Target
                    {
                        return i.color;
                    }
                    ENDCG
            }
            Pass // outlinePass 2
            {
                Cull Front
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                    struct appdata {
                        float4 vertex : POSITION;
                        float3 normal : NORMAL;
                    };
                    struct v2f {
                        float4 pos : SV_POSITION;
                        fixed4 color : COLOR0;
                    };
                    float _Thickness;
                    float4 _OutColor;
                    v2f vert(appdata v) {
                        v2f o;
                        o.pos = UnityObjectToClipPos(v.vertex);
                        float3 norm = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, v.normal));
                        float2 offset = TransformViewToProjection(norm.xy);
                        o.pos.xy += offset * o.pos.z * _Thickness * 0.01; // adds thickness // FIND BETTER SOLUTION
                        o.color = _OutColor;
                        return o;
                    }
                    fixed4 frag(v2f i) : SV_Target
                    {
                        return i.color;
                    }
                ENDCG
            }
        }
        FallBack "Standard"
}