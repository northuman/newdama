using UnityEngine;

public static class CustomCursor
{
    private const string CursorResourcePath =
        "StoryResources/AIGeneratedImages/cursor_game";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Apply()
    {
        Texture2D cursorTexture = Resources.Load<Texture2D>(CursorResourcePath);

        if (cursorTexture == null)
        {
            Debug.LogWarning(
                $"[CustomCursor] No se encontro el cursor en Resources/{CursorResourcePath}.");
            return;
        }

        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }
}
