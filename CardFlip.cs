using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardFlip : MonoBehaviour
{
   public float flipDuration = 0.5f;   // Time for the flip and rotation animation
    public float liftHeight = 0.5f;     // How high the card lifts (upward, Y-axis)
    public float liftDistance = 0.5f;   // How far the card lifts sideways (X-axis)
    public float liftDuration = 0.5f;   // How quickly the card moves upward and sideways

    public float shakeIntensity = 0.1f;  // How much the card shakes
    public int shakeVibrato = 10;        // The vibrato of the shake effect (how many times it shakes)
    public float shakeDuration = 0.5f;   // Duration of shaking

    private bool isFlipped = false;      // Track if the card is flipped

    void OnMouseDown()
    {
        // Start the shake, lift, and rotate sequence
        ShakeLiftAndRotateCard();
    }

    void ShakeLiftAndRotateCard()
    {
        // Step 1: Apply shaking before rotating and lifting
        transform.DOShakePosition(shakeDuration, shakeIntensity, shakeVibrato)
            .OnComplete(() =>
            {
                // Step 2: Create a sequence to handle lifting and rotation at the same time
                Sequence cardSequence = DOTween.Sequence();

                // Step 3: Add the diagonal lift (upward and sideways) to the sequence
                Vector3 liftPosition = new Vector3(transform.position.x + liftDistance, transform.position.y + liftHeight, transform.position.z);
                cardSequence.Append(transform.DOMove(liftPosition, liftDuration));

                // Step 4: Add the rotation animation while the card is lifting
                if (!isFlipped)
                {
                    // Rotate 180 degrees around the Y-axis while the card is lifting
                    cardSequence.Join(transform.DORotate(new Vector3(0, 180, 0), flipDuration).SetEase(Ease.InOutQuad));
                }
                else
                {
                    // Rotate back to 0 degrees if the card is flipped
                    cardSequence.Join(transform.DORotate(new Vector3(0, 0, 0), flipDuration).SetEase(Ease.InOutQuad));
                }

                // Step 5: Add the card moving back to its original position after the rotation
                cardSequence.Append(transform.DOMove(transform.position, liftDuration));

                // Step 6: Toggle the flipped state when the sequence finishes
                cardSequence.OnComplete(() => isFlipped = !isFlipped);
            });
    }
}
