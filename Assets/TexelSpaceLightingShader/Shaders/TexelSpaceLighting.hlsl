#ifndef CIBERMAN_TSL_INCLUDED
#define CIBERMAN_TSL_INCLUDED

float2 GetDeltaST(float2 originalUV)
{
    // Gets the UV coordinate of the center of the texel (using the BaseMap texel size as reference)
    float2 baseMapTexelCenter = floor(originalUV * _BaseMap_TexelSize.zw) / _BaseMap_TexelSize.zw + (_BaseMap_TexelSize.xy / 2.0);

    // Calculate how much the texture UV coords need to
    // shift to be at the center of the nearest texel.
    float2 uvToConvert = (baseMapTexelCenter - originalUV);
 
    // Calculate how much the texture coords vary over fragment space.
    // This essentially defines a 2x2 matrix that gets
    // texture space (UV) deltas from fragment space (ST) deltas
    // Note: I call fragment space (S,T) to disambiguate.
    float2 dUVdS = ddx(originalUV);
    float2 dUVdT = ddy(originalUV);
    float2x2 m = float2x2(dUVdT[1], -dUVdT[0], -dUVdS[1], dUVdS[0]);
 
    // Invert the fragment from texture matrix
    m = m * (1.0f / determinant(m));

    // Convert the UV delta to a fragment space delta
    return mul(m, uvToConvert);
}

// If you are writting your own shader, you can use this macro to "snap" any value to the nearest texel
// You first need to call GetDeltaST with the original UV texture coordinate of your albedo texture and then
// call this macro with that value and the vec2 value you want to snap (for example the UV of the shadowmap or any custom 
// value like a custom "fire intensity" that is calculated in your vertex shader)
#define SNAP_TO_TEXELS(stDelta, valueToSnap) (valueToSnap + ddx(valueToSnap) * stDelta.x + ddy(valueToSnap) * stDelta.y);

#endif