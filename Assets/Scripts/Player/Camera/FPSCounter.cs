using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    public TMP_Text Text;
    public TMP_Text FPS_Text;

    private Dictionary<int, string> CachedNumberStrings = new();
    private int[] _frameRateSamples;
    private int _cacheNumbersAmount = 300;
    private int _averageFromAmount = 30;
    private int _averageCounter = 0;
    private int _currentAveraged;

    void Awake()
    {
        // Cache strings and create array
        {
            for (int i = 0; i < _cacheNumbersAmount; i++)
            {
                CachedNumberStrings[i] = i.ToString();
            }
            _frameRateSamples = new int[_averageFromAmount];
        }
    }
    void Update()
    {
        // Sample
        {
            var currentFrame = (int)Math.Round(1f / Time.smoothDeltaTime); // If your game modifies Time.timeScale, use unscaledDeltaTime and smooth manually (or not).
            _frameRateSamples[_averageCounter] = currentFrame;
        }

        // Average
        {
            var average = 0f;

            foreach (var frameRate in _frameRateSamples)
            {
                average += frameRate;
            }

            _currentAveraged = (int)Math.Round(average / _averageFromAmount);
            _averageCounter = (_averageCounter + 1) % _averageFromAmount;
        }

        // Assign to Private Text value
        {
            Text.text = _currentAveraged < _cacheNumbersAmount && _currentAveraged > 0
                ? CachedNumberStrings[_currentAveraged]
                : _currentAveraged < 0
                    ? "< 0"
                    : _currentAveraged > _cacheNumbersAmount
                        ? $"> {_cacheNumbersAmount}"
                        : "-1";
        }
        // Assign to UI with additional text indicator
        {
            string result = "";
            if (Text.text.Length == 1) result = "  " + Text.text;
            else if (Text.text.Length == 2) result = " " + Text.text;
            else result = Text.text;
            FPS_Text.text = "FPS:" + result;
        }

    }
}