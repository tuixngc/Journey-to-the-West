using UnityEngine;
using UnityEngine.Rendering;
    
// The CreateAssetMenu attribute lets you create instances of this class in the Unity Editor.
[CreateAssetMenu(menuName = "Rendering/ExampleRenderPipelineAsset")]
public class ExampleRenderPipelineAsset : RenderPipelineAsset
{
    // This data can be defined in the Inspector for each Render Pipeline Asset
    public Color exampleColor;
    public string exampleString;
    
        // Unity calls this method before rendering the first frame.
       // If a setting on the Render Pipeline Asset changes, Unity destroys the current Render Pipeline Instance and calls this method again before rendering the next frame.
    protected override RenderPipeline CreatePipeline() {
        // Instantiate the Render Pipeline that this custom SRP uses for rendering, and pass a reference to this Render Pipeline Asset.
        // The Render Pipeline Instance can then access the configuration data defined above.
        return new ExampleRenderPipelineInstance(this);
    }
}
