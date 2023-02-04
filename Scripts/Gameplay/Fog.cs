using Godot;
using System;

public class Fog : Node2D {

	[Export]
	public Color fogColor;

	public static Fog Instance;

	private Texture LightTexture = ResourceLoader.Load("res://Art/FogLight.png") as Texture;

	const int GRID_SIZE = 10;

	int display_width = 8000;
	int display_height = 11000;

	Image fogImage = new Image();
	ImageTexture fogTexture = new ImageTexture();
	Image lightImage;
	Vector2 light_offset;

	Sprite fog;

	public override void _Ready(){
		Instance = this;
		Init();
		UpdateFogImageTexture();

		Events.Instance.startedGame += ResetFog;
		Events.Instance.fogReset?.Invoke();
	}


	private void Init(){
		fog = GetNode(new NodePath("./Fog")) as Sprite;
		lightImage = LightTexture.GetData();
		light_offset = new Vector2(LightTexture.GetWidth()/2,LightTexture.GetHeight()/2);
		var fog_image_width = display_width/GRID_SIZE;
		var fog_image_height = display_height/GRID_SIZE;
		fogImage.Create(fog_image_width,fog_image_height,false,Image.Format.Rgbah);
		fogImage.Fill(new Color(0,0,0,1));
		lightImage.Convert(Image.Format.Rgbah);
		fog.Scale *= GRID_SIZE;
	}

	public void UpdateFog(Vector2 new_grid_position){
		new_grid_position /= GRID_SIZE;
		fogImage.Lock();
		lightImage.Lock();

		var light_rect = new Rect2(Vector2.Zero,new Vector2(lightImage.GetWidth(),lightImage.GetHeight()));
		var myOffset = new Vector2(display_width/2/GRID_SIZE,0);
		fogImage.BlendRect(lightImage,light_rect,new_grid_position + myOffset - light_offset);

		fogImage.Unlock();
		lightImage.Unlock();
		UpdateFogImageTexture();
	}

	public void ResetFog(){
		fogImage.Fill(new Color(0,0,0,1));
		UpdateFogImageTexture();
		Events.Instance.fogReset?.Invoke();
	}

	private void UpdateFogImageTexture(){
		fogTexture.CreateFromImage(fogImage);
		fog.Texture = fogTexture;
		GetShader().SetShaderParam("fogTex",fogTexture);
	}

	private ShaderMaterial GetShader(){
		return fog.Material as ShaderMaterial;
	}

	public override void _Input(InputEvent @event){
		//UpdateFog(GetGlobalMousePosition());
	}

}