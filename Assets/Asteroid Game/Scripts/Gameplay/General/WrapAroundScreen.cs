using UnityEngine;

// Wrap GameObject it to the opposite side of the screen
public class WrapAroundScreen : MonoBehaviour
{
    private void Update()
    {
        if (!IsInsideScreen())
        {
            WrapAround();
        }
    }

    private bool IsInsideScreen()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        return viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1;
    }

    //Wrap position based on the oposite side of the screen
    private void WrapAround()
    {
        // Determine the screen's boundaries
        float screenLeft = Camera.main.ViewportToWorldPoint(Vector3.zero).x;
        float screenRight = Camera.main.ViewportToWorldPoint(Vector3.one).x;
        float screenBottom = Camera.main.ViewportToWorldPoint(Vector3.zero).y;
        float screenTop = Camera.main.ViewportToWorldPoint(Vector3.one).y;

        // Calculate the new position for the object based on the side it hits
        Vector3 newPosition = transform.position;

        if (transform.position.x < screenLeft)
        {
            newPosition.x = screenRight;
        }
        else if (transform.position.x > screenRight)
        {
            newPosition.x = screenLeft;
        }

        if (transform.position.y > screenTop)
        {
            newPosition.y = screenBottom;
        }
        else if (transform.position.y < screenBottom)
        {
            newPosition.y = screenTop;
        }

        transform.position = newPosition;
    }
}