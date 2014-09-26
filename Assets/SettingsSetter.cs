using UnityEngine;
using UnityEditor;

public class TexturePostProcessor : AssetPostprocessor
{

	void OnPostprocessTexture()
	{
		var importer = assetImporter as TextureImporter;
		importer.anisoLevel = 0;
		importer.filterMode = FilterMode.Point;
	}

}