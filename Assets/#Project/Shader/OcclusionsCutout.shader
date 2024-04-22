Shader"Meta/Depth/BiRP/OcclusionCutout"
{
    Properties
    {
        _EnvironmentDepthBias ("Environment Depth Bias", Float) = 0.0
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" }
        LOD 100

        Blend Zero SrcAlpha
        ZTest LEqual

        Pass
        {
            Stencil {
                Ref 1
                Comp always
                Pass replace
            }
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "Packages/com.meta.xr.depthapi/Runtime/BiRP/EnvironmentOcclusionBiRP.cginc"

            // DepthAPI Environment Occlusion
            #pragma multi_compile _ HARD_OCCLUSION SOFT_OCCLUSION

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;

                META_DEPTH_VERTEX_OUTPUT(0)

                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            float _EnvironmentDepthBias;

            v2f vert (appdata v) {
                v2f o;

                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                o.vertex = UnityObjectToClipPos(v.vertex);

                META_DEPTH_INITIALIZE_VERTEX_OUTPUT(o, v.vertex);

                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i)

#if defined(HARD_OCCLUSION) || defined(SOFT_OCCLUSION)
                float occlusionValue = META_DEPTH_GET_OCCLUSION_VALUE(i, _EnvironmentDepthBias);
#else
                float occlusionValue = 1.0f;
#endif

                return fixed4(0, 0, 0, occlusionValue);
            }
            ENDCG
        }
    }
}
