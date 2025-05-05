using UnityEditor;

public class OutlineShaderGUI : ShaderGUI
{
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        base.OnGUI(materialEditor, properties);

        MaterialProperty baseTex2 = FindProperty("_baseTex2", properties);
        MaterialProperty baseNorm2 = FindProperty("_baseNormal2", properties);
        MaterialProperty usesTwoMaterials = FindProperty("_usesTwoMaterials", properties);

        materialEditor.ShaderProperty(usesTwoMaterials, "Show Second Inputs");

        if (usesTwoMaterials.floatValue > 0.5f)
        {
            EditorGUI.indentLevel++;
            materialEditor.ShaderProperty(baseTex2, "Second Base Texture");
            materialEditor.ShaderProperty(baseNorm2, "Second Base Normal");
            EditorGUI.indentLevel--;
        }
    }
}
