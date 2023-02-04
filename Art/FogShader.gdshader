// NOTE: Shader automatically converted from Godot Engine 3.5.stable.mono's CanvasItemMaterial.

shader_type canvas_item;
render_mode blend_mix;

uniform vec4 fogColor : hint_color;
uniform sampler2D fogTex;


void fragment(){
	
	vec4 fogMap = vec4(1.0,1.0,1.0,COLOR.r);
	
	fogMap = fogMap * fogColor;
	
	float a = 1.0-texture(fogTex,UV).r;
	
	COLOR = vec4(fogMap.r,fogMap.g,fogMap.b,a);

}

