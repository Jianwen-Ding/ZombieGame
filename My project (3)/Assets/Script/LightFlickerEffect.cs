using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Written by Steve Streeting 2017
// License: CC0 Public Domain http://creativecommons.org/publicdomain/zero/1.0/

/// <summary>
/// Component which will flicker a linked light while active by changing its
/// intensity between the min and max values given. The flickering can be
/// sharp or smoothed depending on the value of the smoothing parameter.
///
/// Just activate / deactivate this component as usual to pause / resume flicker
/// </summary>

public class LightFlickerEffect : MonoBehaviour {
    [Tooltip("External light to flicker; you can leave this null if you attach script to a light")]
    public new Light light;
    [Tooltip("Minimum random light intensity")]
    public float minIntensity = .5f;
    [Tooltip("Maximum random light intensity")]
    public float maxIntensity = 2f;
    [Tooltip("How much to smooth out the randomness; lower values = sparks, higher = lantern")]
    [Range(1, 50)]
    public int smoothing = 5;

    public float flickerSpeed = 1f;

    float tempFlickerSpeed;
    float timer = 0f;

    // Continuous average calculation via FIFO queue
    // Saves us iterating every time we update, we just change by the delta
    Queue<float> smoothQueue;
    float lastSum = 0;


    /// <summary>
    /// Reset the randomness and start again. You usually don't need to call
    /// this, deactivating/reactivating is usually fine but if you want a strict
    /// restart you can do.
    /// </summary>
    public void Reset() {
        smoothQueue.Clear();
        lastSum = 0;
    }

    void Start() {
         smoothQueue = new Queue<float>(smoothing);
         // External or internal light?
         if (light == null) {
            light = GetComponent<Light>();
         }
         tempFlickerSpeed = flickerSpeed;
    }

    void Update() {
        if (light == null)
            return;

        // pop off an item if too big
        while (smoothQueue.Count >= smoothing) {
            lastSum -= smoothQueue.Dequeue();
        }
        timer += Time.deltaTime;

        if (timer > tempFlickerSpeed){
            // Generate random new item, calculate new average
            float newVal = Random.Range(minIntensity, maxIntensity);
            tempFlickerSpeed = Random.Range(0, flickerSpeed);
            smoothQueue.Enqueue(newVal);
            lastSum += newVal;

            // Calculate new smoothed average
            light.intensity = lastSum / (float)smoothQueue.Count;
            timer = 0f;
        }
    }

}
