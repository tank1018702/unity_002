using ch.sycoforge.Decal;
using ch.sycoforge.Decal.Projectors.Geometry;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeDecalCombiner
{
    /// <summary>
    /// Combines the specified decals to a mesh. 
    /// </summary>
    /// <param name="decals">The decals to comnine.</param>
    /// <returns>Returns the root <c>GameObject</c> of the combines mesh or <c>null</c> when the method fails.</returns>
    /// <remarks>The decals need to have an Atlas as Source</remarks>
    public static List<GameObject> Combine(IList<EasyDecal> decals)
    {
        
        Dictionary<DecalTextureAtlas, List<EasyDecal>> mappings = new Dictionary<DecalTextureAtlas, List<EasyDecal>>();

        foreach (EasyDecal decal in decals)
        {
            if (decal.Source == SourceMode.Atlas && decal.Projector != null)
            {
                if (!mappings.ContainsKey(decal.Atlas))
                {
                    mappings.Add(decal.Atlas, new List<EasyDecal>());
                }

                mappings[decal.Atlas].Add(decal);
            }
        }

        return Combine(mappings);
    }

    private static List<GameObject> Combine(Dictionary<DecalTextureAtlas, List<EasyDecal>> mappings)
    {
        List<GameObject> roots = new List<GameObject>();

        if (mappings.Count > 0)
        {
            foreach (DecalTextureAtlas atlas in mappings.Keys)
            {
                IList<EasyDecal> decals = mappings[atlas];

                foreach (EasyDecal decal in decals)
                {
                    GameObject combined = Combine(decals, atlas);

                    if(combined != null)
                    {
                        roots.Add(combined);
                    }
                }
            }
        }

        return roots;
    }

    private static GameObject Combine(IList<EasyDecal> decals, DecalTextureAtlas atlas)
    {
        if (decals.Count > 0)
        {
            DynamicMesh mesh = new DynamicMesh(DecalBase.DecalRoot, RecreationMode.Always);

            GameObject root = new GameObject(string.Format("Combined Decals Root [{0}]", atlas.name));
            MeshFilter filter = root.AddComponent<MeshFilter>();
            MeshRenderer renderer = root.AddComponent<MeshRenderer>();

            foreach (EasyDecal decal in decals)
            {
                if (decal.Source == SourceMode.Atlas && decal.Projector != null)
                {
                    mesh.Add(decal.Projector.Mesh, decal.LocalToWorldMatrix, root.transform.worldToLocalMatrix);

                    decal.gameObject.SetActive(false);
                }
            }

            renderer.material = atlas.Material;
            filter.sharedMesh = mesh.ConvertToMesh(null);

            //root.transform.parent = DecalBase.DecalRoot.transform;
        }

        return null;
    }
}
