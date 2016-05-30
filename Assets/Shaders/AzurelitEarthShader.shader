// Shader created with Shader Forge v1.26 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.26;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:3,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:True,hqlp:False,rprd:True,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:1,dpts:2,wrdp:True,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:True,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:2865,x:32850,y:32822,varname:node_2865,prsc:2|emission-528-OUT;n:type:ShaderForge.SFN_Multiply,id:6343,x:32241,y:32704,varname:node_6343,prsc:2|A-1153-RGB,B-3243-OUT;n:type:ShaderForge.SFN_LightVector,id:8924,x:31513,y:32922,varname:node_8924,prsc:2;n:type:ShaderForge.SFN_NormalVector,id:1179,x:31513,y:33061,prsc:2,pt:False;n:type:ShaderForge.SFN_Dot,id:6453,x:31686,y:32985,varname:node_6453,prsc:2,dt:0|A-8924-OUT,B-1179-OUT;n:type:ShaderForge.SFN_Divide,id:3243,x:31883,y:32985,varname:node_3243,prsc:2|A-6453-OUT,B-9620-OUT;n:type:ShaderForge.SFN_Vector1,id:9620,x:31686,y:33139,varname:node_9620,prsc:2,v1:0.8;n:type:ShaderForge.SFN_OneMinus,id:4160,x:32061,y:32985,varname:node_4160,prsc:2|IN-3243-OUT;n:type:ShaderForge.SFN_Add,id:528,x:32462,y:32821,varname:node_528,prsc:2|A-6343-OUT,B-4950-OUT;n:type:ShaderForge.SFN_Multiply,id:4950,x:32243,y:32966,varname:node_4950,prsc:2|A-8963-RGB,B-4160-OUT;n:type:ShaderForge.SFN_Tex2d,id:1153,x:31883,y:32621,ptovrint:False,ptlb:Day,ptin:_Day,varname:node_1153,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:f109871435d48e44c9f3930941084948,ntxv:0,isnm:False|MIP-9769-OUT;n:type:ShaderForge.SFN_Tex2d,id:8963,x:31883,y:32812,ptovrint:False,ptlb:Night,ptin:_Night,varname:node_8963,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:bff009cdcfd17d24094ea7aac559b65e,ntxv:0,isnm:False|MIP-9769-OUT;n:type:ShaderForge.SFN_Vector1,id:9769,x:31638,y:32708,varname:node_9769,prsc:2,v1:1;proporder:1153-8963;pass:END;sub:END;*/

Shader "Azurelit/EarthDayNightShader" {
    Properties {
        _Day ("Day", 2D) = "white" {}
        _Night ("Night", 2D) = "white" {}
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
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 2.0
            #pragma glsl
            uniform sampler2D _Day; uniform float4 _Day_ST;
            uniform sampler2D _Night; uniform float4 _Night_ST;
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
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
////// Lighting:
////// Emissive:
                float node_9769 = 1.0;
                float4 _Day_var = tex2Dlod(_Day,float4(TRANSFORM_TEX(i.uv0, _Day),0.0,node_9769));
                float node_3243 = (dot(lightDirection,i.normalDir)/0.8);
                float4 _Night_var = tex2Dlod(_Night,float4(TRANSFORM_TEX(i.uv0, _Night),0.0,node_9769));
                float3 emissive = ((_Day_var.rgb*node_3243)+(_Night_var.rgb*(1.0 - node_3243)));
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
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
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 2.0
            #pragma glsl
            uniform sampler2D _Day; uniform float4 _Day_ST;
            uniform sampler2D _Night; uniform float4 _Night_ST;
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
                UNITY_FOG_COORDS(5)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
////// Lighting:
                float3 finalColor = 0;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "Meta"
            Tags {
                "LightMode"="Meta"
            }
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_META 1
            #define _GLOSSYENV 1
            #include "UnityCG.cginc"
            #include "UnityPBSLighting.cginc"
            #include "UnityStandardBRDF.cginc"
            #include "UnityMetaPass.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_fog
            #pragma exclude_renderers metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 2.0
            #pragma glsl
            uniform sampler2D _Day; uniform float4 _Day_ST;
            uniform sampler2D _Night; uniform float4 _Night_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                float2 texcoord2 : TEXCOORD2;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST );
                return o;
            }
            float4 frag(VertexOutput i) : SV_Target {
                i.normalDir = normalize(i.normalDir);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                UnityMetaInput o;
                UNITY_INITIALIZE_OUTPUT( UnityMetaInput, o );
                
                float node_9769 = 1.0;
                float4 _Day_var = tex2Dlod(_Day,float4(TRANSFORM_TEX(i.uv0, _Day),0.0,node_9769));
                float node_3243 = (dot(lightDirection,i.normalDir)/0.8);
                float4 _Night_var = tex2Dlod(_Night,float4(TRANSFORM_TEX(i.uv0, _Night),0.0,node_9769));
                o.Emission = ((_Day_var.rgb*node_3243)+(_Night_var.rgb*(1.0 - node_3243)));
                
                float3 diffColor = float3(0,0,0);
                float specularMonochrome;
                float3 specColor;
                diffColor = DiffuseAndSpecularFromMetallic( diffColor, 0, specColor, specularMonochrome );
                o.Albedo = diffColor;
                
                return UnityMetaFragment( o );
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
