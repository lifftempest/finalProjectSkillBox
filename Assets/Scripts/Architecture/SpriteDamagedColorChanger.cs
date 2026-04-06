using System.Collections;
using UnityEngine;

public class SpriteDamagedColorChanger
{
    private static Color _damagedSpriteColor = new Color(3, 0.5f, 0.5f, 1);
    private static Color _standartColor = new Color(1, 1, 1, 1);

    public static IEnumerator FlashSprite(SpriteRenderer spriteRenderer)
    {
        float duration = 0.25f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float currentTime = elapsed / duration;
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, _damagedSpriteColor, currentTime);
            yield return null;
        }
        spriteRenderer.color = _damagedSpriteColor;
        elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float currentTime = elapsed / duration;
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, _standartColor, currentTime);
            yield return null;
        }
        spriteRenderer.color = _standartColor;
    }
}
