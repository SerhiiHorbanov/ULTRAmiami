shader_type canvas_item;
uniform sampler2D SCREEN_TEXTURE : hint_screen_texture,filter_linear_mipmap; 

uniform vec2 size;

uniform vec2 r_offset;
uniform vec2 b_offset;
uniform vec2 g_offset;

uniform float lines_intensity;
uniform float lines_speed;

void fragment()
{
	vec2 uv = SCREEN_UV;
	uv -= mod(uv, size);
	
	float r = textureLod(SCREEN_TEXTURE, uv + r_offset, 0.0).r;
	float g = textureLod(SCREEN_TEXTURE, uv + g_offset, 0.0).g;
	float b = textureLod(SCREEN_TEXTURE, uv + b_offset, 0.0).b;
	
	vec2 line = vec2(sin(TIME * lines_speed) * 0.5 + 0.5, uv.y);
	vec3 glitch = texture(SCREEN_TEXTURE, line).rgb * lines_intensity;
	
	COLOR = vec4(r, g, b, 1) + vec4(glitch * lines_intensity, 1);
	//vec4 behind_color = texture(SCREEN_TEXTURE, SCREEN_UV);
	//float brightness = behind_color.r + behind_color.g + behind_color.b * 0.3f;
	//COLOR = vec4(brightness, brightness, brightness, 1);
}