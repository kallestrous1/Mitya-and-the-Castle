using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SwooshMeshTrail : MonoBehaviour
{
    [Header("Lifetime / Sampling")]
    [SerializeField] private float lifetime = 1f;          // how long the ribbon persists
    [SerializeField] private float minSampleDistance = 0.01f; // avoid oversampling jitter
    [SerializeField] private int maxSamples = 48;             // cap for performance

    [Header("Shape")]
    [SerializeField] private AnimationCurve widthOverLife = AnimationCurve.EaseInOut(0, 1, 1, 0);
    [SerializeField] private float baseWidth = 1.0f;          // overall thickness multiplier

    [Header("Visual")]
    [SerializeField] private Gradient colorOverLife;
    [SerializeField] private bool worldSpace = true;          // usually true for weapons in motion

    private struct Sample
    {
        public Vector3 a;     // base
        public Vector3 b;     // tip
        public float t;       // time
    }

    private readonly List<Sample> samples = new();
    private Mesh mesh;

    // Reused buffers
    private readonly List<Vector3> verts = new();
    private readonly List<int> tris = new();
    private readonly List<Vector2> uvs = new();
    private readonly List<Color> colors = new();

    private bool emitting;

    void Awake()
    {
        mesh = new Mesh { name = "SwooshTrailMesh" };
        mesh.MarkDynamic();
        GetComponent<MeshFilter>().sharedMesh = mesh;

        // A reasonable default if you didn’t set one in inspector
        if (colorOverLife.colorKeys == null || colorOverLife.colorKeys.Length == 0)
        {
            colorOverLife = new Gradient();
            colorOverLife.SetKeys(
                new[] { new GradientColorKey(Color.white, 0f), new GradientColorKey(Color.white, 1f) },
                new[] { new GradientAlphaKey(255f, 0f), new GradientAlphaKey(255f, 1f) }
            );
        }

        StopAndClear();
    }

    public void StartSwoosh()
    {
        if(emitting) return;
        Debug.Log("Starting swoosh");
        emitting = true;
        // Important: clear so you don't connect from last position
        samples.Clear();
        ClearMesh();
    }

    public void StopSwoosh()
    {
        if (!emitting) return;
        emitting = false;
        // do not clear; allow fade
    }

    public void StopAndClear()
    {
        emitting = false;
        samples.Clear();
        ClearMesh();
    }

    public void GenerateSwoosh(Vector3 bladeBaseWorld, Vector3 bladeTipWorld)
    {
        if (!emitting) return;

        Vector3 aLocal = transform.InverseTransformPoint(bladeBaseWorld);
        Vector3 bLocal = transform.InverseTransformPoint(bladeTipWorld);

        // Sampling gate (keep, but use locals)
        if (samples.Count > 0)
        {
            var last = samples[^1];
            float dA = (aLocal - last.a).sqrMagnitude;
            float dB = (bLocal - last.b).sqrMagnitude;
            float minSqr = minSampleDistance * minSampleDistance;

            if (samples.Count >= 2 && dA < minSqr && dB < minSqr)
            {
                PruneOldSamples();
                RebuildMesh();
                return;
            }
        }

        samples.Add(new Sample { a = aLocal, b = bLocal, t = Time.time });

        if (samples.Count > maxSamples) samples.RemoveAt(0);

        PruneOldSamples();
        RebuildMesh();
    }

    // Call this when not emitting but you still want fade-out updates
    public void TickFadeOnly()
    {
        if (samples.Count == 0) return;
        PruneOldSamples();
        RebuildMesh();
    }

    private void PruneOldSamples()
    {
        float cutoff = Time.time - lifetime;
        // remove oldest first
        while (samples.Count > 0 && samples[0].t < cutoff)
            samples.RemoveAt(0);

        if (samples.Count < 2)
            ClearMesh();
    }

    private void RebuildMesh()
    {
        if (samples.Count < 2)
            return;

        verts.Clear();
        tris.Clear();
        uvs.Clear();
        colors.Clear();

        // Build quad strip: for each sample i we add 2 vertices (base, tip)
        // Triangles connect (i-1) to i.
        int n = samples.Count;

        float now = Time.time;
        for (int i = 0; i < n; i++)
        {
            float age01 = Mathf.InverseLerp(now, now - lifetime, samples[i].t); // 0 = newest, 1 = oldest
            // age01 is reversed due to inverse lerp, so clamp and flip:
            age01 = Mathf.Clamp01(age01);

            float width01 = widthOverLife.Evaluate(age01);
            float w = baseWidth * width01;

            
            Color c = colorOverLife.Evaluate(age01);

            Vector3 mid = (samples[i].a + samples[i].b) * 0.5f;

            float t = age01;

            float shrink = Mathf.Lerp(1f, 0.2f, t);

            Vector3 a = Vector3.Lerp(mid, samples[i].a, shrink);
            Vector3 b = Vector3.Lerp(mid, samples[i].b, shrink);

            Vector3 dir = (b - a).normalized;
            Vector3 normal = new Vector3(-dir.y, dir.x, 0f); // 2D perpendicular

            float noise = Mathf.PerlinNoise(i * 0.15f, Time.time * 0.5f);

            noise = noise * 2f - 1f;

            // strength tweak
            float distortionStrength = 0.5f;

            a += normal * noise * distortionStrength;
            b += normal * noise * distortionStrength;

            verts.Add(a);
            verts.Add(b);

            float v = i / (float)(n - 1);
            uvs.Add(new Vector2(0f, v));
            uvs.Add(new Vector2(1f, v));

            float trailAlpha = Mathf.Lerp(1f, 0f, age01);

            Color cBase = c;
            cBase.a = 0.05f * trailAlpha;   // transparent near handle

            Color cTip = c;
            cTip.a = 1.0f * trailAlpha;     // opaque near blade tip

            colors.Add(cBase);
            colors.Add(cTip);
        }

        // Triangles
        for (int i = 1; i < n; i++)
        {
            int i0 = (i - 1) * 2;
            int i1 = (i - 1) * 2 + 1;
            int i2 = i * 2;
            int i3 = i * 2 + 1;

            // quad: (i0,i2,i1) and (i2,i3,i1)
            tris.Add(i0);
            tris.Add(i2);
            tris.Add(i1);

            tris.Add(i2);
            tris.Add(i3);
            tris.Add(i1);
        }


        mesh.Clear(false);
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        mesh.SetUVs(0, uvs);
        mesh.SetColors(colors);
        mesh.RecalculateBounds();
    }

    private void ClearMesh()
    {
        if (mesh == null) return;
        mesh.Clear(false);
    }
}