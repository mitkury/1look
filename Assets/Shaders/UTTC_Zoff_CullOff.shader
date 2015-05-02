Shader "Custom/UTTC_Zoff_CullOff" {

Properties {
    _Color ("Main Color (A=Opacity)", Color) = (1,1,1,1)
    _MainTex ("Base (A=Opacity)", 2D) = ""
}

Category {

    Tags { 
         "Queue"="Transparent" 
         "IgnoreProjector"="True" 
         "RenderType"="Transparent" 
         "PreviewType"="Plane"
      }

      Cull Off
      Lighting Off
      ZWrite Off
      Fog { Mode Off }

      Blend SrcAlpha OneMinusSrcAlpha
 
    SubShader {Pass {
        SetTexture[_MainTex] {Combine texture * constant ConstantColor[_Color]}
    }}
}
}