Shader "Custom/Tex_Toon" {
    Properties{
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Main Texture", 2D) = "white"{}
        _TexFactor("Texture Factor", float) = 1
        _RampTex("Ramp Texture", 2D) = "white"{}
    }
        SubShader{
            // Toon
            Tags{ "RenderType" = "Opaque" "Queue" = "Geometry"}

                CGPROGRAM
                #pragma surface surf ToonRamp

                float _TexFactor;
                float4 _Color;
                sampler2D _RampTex;
                sampler2D _MainTex;

                struct Input
                {
                    float2 uv_MainTex;
                };

                float4 LightingToonRamp(SurfaceOutput s, fixed3 lightDir, fixed atten) {
                    float diff = dot(s.Normal, lightDir);
                    float h = diff * 0.5 + 0.5;
                    float2 rh = h;
                    float3 ramp = tex2D(_RampTex, rh).rgb;

                    float4 c;
                    c.rgb = s.Albedo * _LightColor0.rgb * ramp;
                    c.a = s.Alpha;
                    return c;
                }

                void surf(Input IN, inout SurfaceOutput o)
                {
                    //o.Albedo = _Color.rgb;
                    o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _TexFactor * _Color.rgb;
                }
                ENDCG
        }
        FallBack "Standard"
}