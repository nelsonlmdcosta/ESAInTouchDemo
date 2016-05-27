// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:1,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:False,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:4013,x:33189,y:32708,varname:node_4013,prsc:2|emission-2223-OUT;n:type:ShaderForge.SFN_Tex2d,id:6840,x:32264,y:32901,ptovrint:False,ptlb:Night Texture,ptin:_NightTexture,varname:node_6840,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:bff009cdcfd17d24094ea7aac559b65e,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:688,x:32264,y:32716,ptovrint:False,ptlb:Day Texture,ptin:_DayTexture,varname:node_688,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:f109871435d48e44c9f3930941084948,ntxv:0,isnm:False;n:type:ShaderForge.SFN_LightVector,id:5416,x:31974,y:33071,varname:node_5416,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:7613,x:31974,y:33210,prsc:2,pt:False;n:type:ShaderForge.SFN_Dot,id:6949,x:32264,y:33130,varname:node_6949,prsc:2,dt:0|A-5416-OUT,B-7613-OUT;n:type:ShaderForge.SFN_Multiply,id:260,x:32591,y:32748,varname:node_260,prsc:2|A-688-RGB,B-6949-OUT;n:type:ShaderForge.SFN_Multiply,id:4858,x:32733,y:33038,varname:node_4858,prsc:2|A-6840-RGB,B-9219-OUT;n:type:ShaderForge.SFN_OneMinus,id:9219,x:32522,y:33147,varname:node_9219,prsc:2|IN-6949-OUT;n:type:ShaderForge.SFN_Add,id:2223,x:32919,y:32855,varname:node_2223,prsc:2|A-260-OUT,B-4858-OUT;proporder:6840-688;pass:END;sub:END;*/

Shader "AzurelitShaders/EarthDayNightShader" {
    Properties {
        _NightTexture ("Night Texture", 2D) = "white" {}
        _DayTexture ("Day Texture", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _NightTexture; uniform float4 _NightTexture_ST;
            uniform sampler2D _DayTexture; uniform float4 _DayTexture_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
////// Lighting:
////// Emissive:
                float4 _DayTexture_var = tex2D(_DayTexture,TRANSFORM_TEX(i.uv0, _DayTexture));
                float node_6949 = dot(lightDirection,i.normalDir);
                float4 _NightTexture_var = tex2D(_NightTexture,TRANSFORM_TEX(i.uv0, _NightTexture));
                float3 emissive = ((_DayTexture_var.rgb*node_6949)+(_NightTexture_var.rgb*(1.0 - node_6949)));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma exclude_renderers xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform sampler2D _NightTexture; uniform float4 _NightTexture_ST;
            uniform sampler2D _DayTexture; uniform float4 _DayTexture_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                LIGHTING_COORDS(3,4)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
////// Lighting:
                float3 finalColor = 0;
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
