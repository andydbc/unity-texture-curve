/*
MIT License

Copyright (c) 2018 Andy Duboc

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
namespace TextureCurve
{
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
        public Texture2D Texture
        {
            get { return _texture; }
        }

        [SerializeField] int _resolution = 512;
        public int Resolution
        {
            get { return _resolution; }
        }

        [SerializeField] TextureWrapMode _wrapMode = TextureWrapMode.Clamp;
        [SerializeField] FilterMode _filterMode = FilterMode.Bilinear;

        public void OnEnable()
        {
            if (_texture == null)
                _texture = new Texture2D(_resolution, 1, TextureFormat.ARGB32, false, true);
        }

        public void Bake()
        {
            if (_texture == null)
                return;

            if (_texture.width != _resolution)
                _texture.Reinitialize(_resolution, 1);

            _texture.wrapMode = _wrapMode;
            _texture.filterMode = _filterMode;

            Color[] colors = new Color[_resolution];
            for (int i = 0; i < _resolution; ++i)
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
}
