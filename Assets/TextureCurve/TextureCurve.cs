using UnityEngine;

public class TextureCurve : ScriptableObject
{
    [SerializeField]
    private AnimationCurve _red = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField]
    private AnimationCurve _green = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField]
    private AnimationCurve _blue = AnimationCurve.Linear(0, 0, 1, 1);
    [SerializeField]
    private AnimationCurve _alpha = AnimationCurve.Linear(0, 0, 1, 1);

    Texture2D _texture;
    public Texture2D Texture {
        get { return _texture; }
    }

    [SerializeField] int _resolution = 512;
    public int Resolution {
        get { return _resolution; }
    }

    [SerializeField] TextureWrapMode _wrapMode = TextureWrapMode.Clamp;
    [SerializeField] FilterMode _filterMode = FilterMode.Bilinear;

    public void OnEnable()
    {
        if(_texture == null)
            _texture = new Texture2D(_resolution, 1, TextureFormat.ARGB32, false, true);
    }

    public void Bake()
    {
        if (_texture == null)
            return;

        if (_texture.width != _resolution)
            _texture.Resize(_resolution, 1);

        _texture.wrapMode = _wrapMode;
        _texture.filterMode = _filterMode;

        Color[] colors = new Color[_resolution];
        for(int i = 0; i < _resolution; ++i)
        {
            var t = (float)i / _resolution;

            colors[i].r = _red.Evaluate(t);
            colors[i].g = _green.Evaluate(t);
            colors[i].b = _blue.Evaluate(t);
            colors[i].a = _alpha.Evaluate(t);
        }

        _texture.SetPixels(colors);
        _texture.Apply(false);
    }
}
