void CalculateMainLight_float(float3 BaseColor, out float3 C) {
    // Get the HDRP light data
    DirectionalLightData mainLight = _DirectionalLightDatas[0];

    // Calculate the light direction
    float3 Direction = -mainLight.forward;

    // Calculate the light color
    float3 Color = mainLight.color;

    float2 DiffuseVec = (DotProduct(Direction, WorldNormal), 0);

    SamplerState SS;
    SS.Filter = Linear;
    SS.Wrap = Clamp;

    float r = SampleTex2DLOD(ToonRamp, Diffusevec, SS, 0).r;

    c = Color * r * BaseColor;
}