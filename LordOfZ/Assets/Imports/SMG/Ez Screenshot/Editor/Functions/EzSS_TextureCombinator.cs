using UnityEngine;

namespace SMG.EzScreenshot
{
    public static class EzSS_TextureCombinator
    {
        private enum Combination
        {
            simple,
            shadow,
            gameViewAndMockupScreen,
            mockupAndMockupScreen
        }

        public static Texture2D Simple(Texture2D bgTexture, Texture2D fgTexture, bool alphaBlended)
        {
            // Calculate the offsetbased on the size difference of the bgTexture and fgTexture
            Vector2 _offset = new Vector2((((bgTexture.width - fgTexture.width)) / 2), (((bgTexture.height - fgTexture.height)) / 2));
            Texture2D _result = new Texture2D(bgTexture.width, bgTexture.height);
            return Combine(bgTexture, fgTexture, _result, _offset, Combination.simple, alphaBlended);
        }

        public static Texture2D Simple(Texture2D bgTexture, Texture2D fgTexture, bool alphaBlended, Vector2 mockupOffset)
        {
            // Calculate the offsetbased on the size difference of the bgTexture and fgTexture
            Vector2 _offset = new Vector2((((bgTexture.width - fgTexture.width)) / 2), (((bgTexture.height - fgTexture.height)) / 2));
            _offset = new Vector2(_offset.x + mockupOffset.x, _offset.y + mockupOffset.y);
            Texture2D _result = new Texture2D(bgTexture.width, bgTexture.height);
            return Combine(bgTexture, fgTexture, _result, _offset, Combination.simple, alphaBlended);
        }

        public static Texture2D Shadow(Texture2D bgTexture, Texture2D fgTexture, EzSS_Shadow shadow)
        {
            Texture2D _result = new Texture2D(bgTexture.width, bgTexture.height);
            Vector2 _offset = Vector2.zero;
            Vector2 _shadowOffset = Vector2.zero;
            // Calculate the offsetbased on the size difference of the bgTexture and fgTexture
            _offset = new Vector2(((bgTexture.width - fgTexture.width)) / 2, ((bgTexture.height - fgTexture.height)) / 2);
            // Calculate the shadow offset based on the shadow direction
            _shadowOffset = new Vector2((shadow.direction.x / 100) * fgTexture.width, (shadow.direction.y / 100) * fgTexture.height);
            // Add to _offset the shadow offset
            _offset = new Vector2(_offset.x + _shadowOffset.x, _offset.y + _shadowOffset.y);
            return Combine(bgTexture, fgTexture, _result, _offset, Combination.shadow, false);
        }

        public static Texture2D Shadow(Texture2D bgTexture, Texture2D fgTexture, EzSS_Shadow shadow, Vector2 mockupOffset)
        {
            Texture2D _result = new Texture2D(bgTexture.width, bgTexture.height);
            Vector2 _offset = Vector2.zero;
            Vector2 _shadowOffset = Vector2.zero;
            // Calculate the offsetbased on the size difference of the bgTexture and fgTexture
            _offset = new Vector2(((bgTexture.width - fgTexture.width)) / 2, ((bgTexture.height - fgTexture.height)) / 2);
            // Calculate the shadow offset based on the shadow direction
            _shadowOffset = new Vector2((shadow.direction.x / 100) * fgTexture.width, (shadow.direction.y / 100) * fgTexture.height);
            // Add to _offset the shadow offset
            _offset = new Vector2((_offset.x + _shadowOffset.x) + mockupOffset.x, (_offset.y + _shadowOffset.y) + mockupOffset.y);
            return Combine(bgTexture, fgTexture, _result, _offset, Combination.shadow, false);
        }

        public static Texture2D GameViewAndMockupScreen(Texture2D bgTexture, Texture2D fgTexture, EzSS_Mockup mockup, EzSS_Resolutions resolutions)
        {
            Texture2D _result = new Texture2D(bgTexture.width, bgTexture.height);
            // Calculate the offset based on the size difference of the bgTexture and fgTexture
            Vector2 _offset = new Vector2((((bgTexture.width - fgTexture.width)) / 2), (((bgTexture.height - fgTexture.height)) / 2));
            float _offsetMultiplierX = 0;
            float _offsetMultiplierY = 0;
            // Add the mockup offset to the current offset
            switch (mockup.selectedOrientation)
            {
                case EzSS_Mockup.Orientations.portrait:
                    // Calculate the screen offset
                    if (mockup.selectedScreenOffset.x != 0)
                        _offsetMultiplierX = (mockup.selectedScreenOffset.x * resolutions.mockupWidth) / mockup.selectedTextureSize.x;
                    if (mockup.selectedScreenOffset.y != 0)
                        _offsetMultiplierY = (mockup.selectedScreenOffset.y * resolutions.mockupHeight) / mockup.selectedTextureSize.y;
                    // Add the screen offset to the texture offset
                    _offset = new Vector2(_offset.x + _offsetMultiplierX, _offset.y + _offsetMultiplierY);
                    break;

                case EzSS_Mockup.Orientations.landscapeLeft:
                    // Calculate the screen offset
                    if (mockup.selectedScreenOffset.x != 0)
                        _offsetMultiplierX = (mockup.selectedScreenOffset.x * resolutions.mockupHeight) / mockup.selectedTextureSize.x;
                    if (mockup.selectedScreenOffset.y != 0)
                        _offsetMultiplierY = (mockup.selectedScreenOffset.y * resolutions.mockupWidth) / mockup.selectedTextureSize.y;
                    // Add the screen offset to the texture offset
                    _offset = new Vector2(_offset.x - _offsetMultiplierY, _offset.y + _offsetMultiplierX);
                    break;

                case EzSS_Mockup.Orientations.landscapeRight:
                    // Calculate the screen offset
                    if (mockup.selectedScreenOffset.x != 0)
                        _offsetMultiplierX = (-mockup.selectedScreenOffset.x * resolutions.mockupHeight) / mockup.selectedTextureSize.x;
                    if (mockup.selectedScreenOffset.y != 0)
                        _offsetMultiplierY = (mockup.selectedScreenOffset.y * resolutions.mockupWidth) / mockup.selectedTextureSize.y;
                    // Add the screen offset to the texture offset
                    _offset = new Vector2(_offset.x + _offsetMultiplierY, _offset.y + _offsetMultiplierX);
                    break;
            }

            return Combine(bgTexture, fgTexture, _result, _offset, Combination.gameViewAndMockupScreen, false);
        }

        public static Texture2D MockupAndMockupScreen(Texture2D bgTexture, Texture2D fgTexture)
        {
            Texture2D _result = new Texture2D(bgTexture.width, bgTexture.height);
            Vector2 _offset = Vector2.zero;
            // Calculate the offsetbased on the size difference of the bgTexture and fgTexture
            _offset = new Vector2((((bgTexture.width - fgTexture.width)) / 2), (((bgTexture.height - fgTexture.height)) / 2));
            return Combine(bgTexture, fgTexture, _result, _offset, Combination.mockupAndMockupScreen, false);
        }

        private static Texture2D Combine(Texture2D bgTexture, Texture2D fgTexture, Texture2D result, Vector2 offset, Combination combination, bool alphaBlended)
        {
            for (int y = 0; y < bgTexture.height; y++)
            {
                for (int x = 0; x < bgTexture.width; x++)
                {
                    if (x >= offset.x && y >= offset.y && x < (fgTexture.width + offset.x) && y < (fgTexture.height + offset.y))
                    {
                        Color _bgColor = bgTexture.GetPixel(x, y);
                        Color _fgColor = fgTexture.GetPixel(x - (int)offset.x, y - (int)offset.y);
                        Color _finalColor = Color.clear;

                        switch (combination)
                        {
                            case Combination.simple:
                                if (alphaBlended)
                                    _finalColor = _bgColor * (1.0f - _fgColor.a) + _fgColor;
                                else
                                    _finalColor = Color.Lerp(_bgColor, _fgColor, _fgColor.a / 1.0f);
                                break;

                            case Combination.shadow:
                                _finalColor = new Color(
                                    (_fgColor.r * _fgColor.a) + (_bgColor.r * (1 - _fgColor.a)),
                                    (_fgColor.g * _fgColor.a) + (_bgColor.g * (1 - _fgColor.a)),
                                    (_fgColor.b * _fgColor.a) + (_bgColor.b * (1 - _fgColor.a)),
                                    _fgColor.a + (_bgColor.a * (1 - _fgColor.a)));
                                break;

                            case Combination.gameViewAndMockupScreen:
                                _finalColor = _bgColor.a > 0 ? Color.Lerp(_bgColor, _fgColor, 1) : Color.Lerp(_bgColor, _fgColor, 0);
                                break;

                            case Combination.mockupAndMockupScreen:
                                _finalColor = new Color(
                                    (_fgColor.r * _fgColor.a) + (_bgColor.r * (1 - _fgColor.a)),
                                    (_fgColor.g * _fgColor.a) + (_bgColor.g * (1 - _fgColor.a)),
                                    (_fgColor.b * _fgColor.a) + (_bgColor.b * (1 - _fgColor.a)),
                                    _fgColor.a + (_bgColor.a * (1 - _fgColor.a)));
                                break;
                        }

                        result.SetPixel(x, y, _finalColor);
                    }
                    else
                    {
                        result.SetPixel(x, y, bgTexture.GetPixel(x, y));
                    }
                }
            }
            // result.Apply();
            return result;
        }
    }
}