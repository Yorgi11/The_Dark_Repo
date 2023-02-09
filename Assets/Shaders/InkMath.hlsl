float3 CalculateMainLight_float(float Thickness, float3 baseColor, float3 HDsceneColor, out float3 Color) {

    float3 halfvec = (0.5, 0.5, 0.5, 1);

    float2 v1 = (-1, 1, 1);
    float2 v2 = (1, -1, 1);
    float2 v3 = (1, 1, 1);
    float2 v4 = (-1, -1, 1);
    float2 Screen = (Width, Height);

    v1 *= Thickness;
    v1 /= Screen;
    v1 += ScreenPos;

    v2 *= Thickness;
    v2 /= Screen;
    v2 += ScreenPos;

    v3 *= Thickness;
    v3 /= Screen;
    v3 += ScreenPos;

    v4 *= Thickness;
    v4 /= Screen;
    v4 += ScreenPos;

    // Normal Ink
    float3 hd1 = SamplrHD(v1);
    float3 hd2 = SamplrHD(v2);
    float3 hd3 = SamplrHD(v3);
    float3 hd4 = SamplrHD(v4);

    hd1 *= halfvec;
    hd1 += halfvec;

    hd2 *= halfvec;
    hd2 += halfvec;

    hd3 *= halfvec;
    hd3 += halfvec;

    hd4 *= halfvec;
    hd4 += halfvec;

    float3 t1 = hd1 - hd2;
    if (t1 < 0) t1 *= -1;

    float3 t2 = hd3 - hd4;
    if (t2 < 0) t2 *= -1;

    float3 comb = t1 + t2;
    float r = comb.x;
    float g = comb.y;
    float b = comb.z;

    float temp1 = max(r, g);
    float temp2 = max(temp1, b);

    float normalInk = smooth(0.2, 0.7, temp2);

    // Depth Ink
    float depth1 = scnedpth(v1);
    float depth2 = scnedpth(v2);
    float depth3 = scnedpth(v3);
    float depth4 = scnedpth(v4);

    float a1 = depth1 - depth2;
    if (a1 < 0) a1 *= -1;

    float a2 = depth3 - depth4;
    if (a2 < 0) a2 *= -1;

    float k1 = max(depth1, depth2);
    float k2 = max(depth3, k1);
    float k3 = max(depth4, k2);

    float af = a1 + a2;
    af /= k3;
    af = sat(af);
    af = smooth(0.1, 0.2, af);

    float L1 = max(normalInk, af);

    Color = Lrp(HDsceneColor, baseColor, L1);
}

float3 SamplrHD(float4 vec) {
    //NormalWorld * vec
}

float scneDpth(float4 uv) {
    //samplin = Linear01
}

float sat(float val){
    //saturate algo
    //make between 0 and 1
}

float3 Lrp(float3 A, float3 B, float3 T) {
    // lerp from a to b by t
}

float max(float val1, float val2) {
    if (val1 > val2) return val1;
    else return val2;
}

float smooth(float eg1, float eg2, float val) {

}