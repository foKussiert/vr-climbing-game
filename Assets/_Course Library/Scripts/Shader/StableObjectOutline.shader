Shader "Custom/StableObjectOutline"
{
    Properties
    {
        _OutlineColor ("Outline Color", Color) = (1, 0.75, 0.05, 1)
        _OutlineThickness ("Outline Thickness", Float) = 0.05
    }

    SubShader
    {
        Tags
        {
            // Outline wird VOR normalen Geometry-Objekten gerendert
            "Queue"="Geometry-10"
            "RenderType"="Opaque"
        }

        Pass
        {
            // Nur Rückseiten der aufgeblasenen Hülle rendern
            Cull Front

            // Wichtig: Depth schreiben, damit das Rendering stabiler ist
            ZWrite On
            ZTest LEqual

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            fixed4 _OutlineColor;
            float _OutlineThickness;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;

                float3 normal = normalize(v.normal);
                float3 expandedPosition = v.vertex.xyz + normal * _OutlineThickness;

                o.pos = UnityObjectToClipPos(float4(expandedPosition, 1.0));
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }
    }
}