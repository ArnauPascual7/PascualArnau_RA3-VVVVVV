Shader "Apascual/OutlineShader_Sprite"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [MainTexture] _BaseMap("Base Map", 2D) = "white" {}
        [Space(10)]
        _OutlineColor("Outline Color", Color) = (1, 1, 1, 1)
        _OutlineWidth("Outline Width", Range(1.0, 10.0)) = 2.0
    }
    SubShader
    {
        Tags 
        { 
            "Queue" = "Transparent" 
            "RenderType" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }
        
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        
        Pass
        {
            Name "SpriteOutline"
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            
            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };
            
            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };
            
            TEXTURE2D(_BaseMap);
            SAMPLER(sampler_BaseMap);
            
            // Unity injecta automàticament aquesta textura del Sprite Renderer
            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            
            CBUFFER_START(UnityPerMaterial)
                half4 _BaseColor;
                float4 _BaseMap_ST;
                half4 _OutlineColor;
                float _OutlineWidth;
                float4 _BaseMap_TexelSize;
                float4 _MainTex_ST;
                float4 _MainTex_TexelSize;
            CBUFFER_END
            
            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                OUT.color = IN.color;
                return OUT;
            }
            
            half4 frag(Varyings IN) : SV_Target
            {
                // Utilitzem _MainTex que Unity injecta automàticament des del Sprite Renderer
                // Si no existeix, fem fallback a _BaseMap
                #ifdef UNITY_SPRITE_INSTANCING_ENABLED
                    half4 mainTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                #else
                    half4 mainTex = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
                #endif
                
                half4 color = mainTex * _BaseColor * IN.color;
                
                // Si el píxel ja és opac, retornem el color normal
                if (color.a > 0.5)
                {
                    return color;
                }
                
                // Si el píxel és transparent, comprovem si hi ha outline
                #ifdef UNITY_SPRITE_INSTANCING_ENABLED
                    float2 texelSize = _MainTex_TexelSize.xy * _OutlineWidth;
                #else
                    float2 texelSize = _BaseMap_TexelSize.xy * _OutlineWidth;
                #endif
                
                // Algorisme millorat: comprovem més direccions en cercle
                half maxAlpha = 0;
                const int samples = 16; // Més samples = outline més uniforme
                
                for (int i = 0; i < samples; i++)
                {
                    float angle = (i / float(samples)) * 6.28318530718; // 2*PI
                    float2 offset = float2(cos(angle), sin(angle)) * texelSize;
                    #ifdef UNITY_SPRITE_INSTANCING_ENABLED
                        half alpha = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv + offset).a;
                    #else
                        half alpha = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv + offset).a;
                    #endif
                    maxAlpha = max(maxAlpha, alpha);
                }
                
                // Si hem trobat alpha al voltant, dibuixem l'outline
                if (maxAlpha > 0.5)
                {
                    // Fem blend suau entre outline i transparent
                    half outlineAlpha = saturate(maxAlpha * _OutlineColor.a);
                    return half4(_OutlineColor.rgb, outlineAlpha);
                }
                
                return color;
            }
            ENDHLSL
        }
    }
}