Shader "Unlit/ReticleColor" {
     Properties {
         _Color("Color & Transparency", Color) = (0, 0, 0, 0)
     }
     SubShader {
         Lighting Off
         ZWrite Off
         ZTest Always
         Fog { Mode Off }
         Cull Back
         Blend SrcAlpha OneMinusSrcAlpha
         Tags {"Queue" = "Transparent"}
         Color[_Color]
         Pass {
         }
     } 
     FallBack "Unlit/Transparent"
 }