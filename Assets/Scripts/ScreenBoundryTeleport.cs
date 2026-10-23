    using UnityEngine;
    using Unity.U2D;

    public class ScreenBoundaryTeleport : MonoBehaviour
    {
       public Camera mainCamera;
       public float padding = 0.5f; // Extra space from edge
       public bool checkLeft = true;
       public bool checkRight = true;
       public bool checkTop = true;
       public bool checkBottom = true;
       public GameObject teleportTarget;

       void Start()
       {
           // Auto-assign camera if not set
           if (mainCamera == null)
           {
               mainCamera = Camera.main;
           }
       }

       void Update()
       {
            if (mainCamera == null) return;
            // Get camera bounds
            float cameraHeight = mainCamera.orthographicSize * 2f;
            float cameraWidth = cameraHeight * mainCamera.aspect;
            Vector3 cameraPos = mainCamera.transform.position;

            float leftBoundary = cameraPos.x - (cameraWidth / 2f);
            float rightBoundary = cameraPos.x + (cameraWidth / 2f);
            float topBoundary = cameraPos.y + (cameraHeight / 2f);
            float bottomBoundary = cameraPos.y - (cameraHeight / 2f);

            // Get this GameObject's bounds
            //SpriteRenderer spriteRenderer = GetComponent<SpriteShape>();
            UnityEngine.U2D.SpriteShapeRenderer spriteRenderer = GetComponent<UnityEngine.U2D.SpriteShapeRenderer>();
            Collider2D collider = GetComponent<Collider2D>();

            float objectWidth = 1f;
            float objectHeight = 1f;
            Vector3 targetPosition = transform.position;

            if (spriteRenderer != null)
            {
               objectWidth = spriteRenderer.bounds.size.x;
               objectHeight = spriteRenderer.bounds.size.y;
               targetPosition = collider.transform.position;
            }
            else if (collider != null)
            {
               objectWidth = collider.bounds.size.x;
               objectHeight = collider.bounds.size.y;
               targetPosition = spriteRenderer.transform.position;
            }

            Vector3 currentPos = teleportTarget != null ? teleportTarget.transform.position : targetPosition;
            Vector3 newPos = currentPos;

            // Check left boundary
            if (checkLeft && currentPos.x < leftBoundary + objectWidth/2 + padding)
            {
               newPos.x = leftBoundary + objectWidth/2 + padding;
            }

            // Check right boundary
            if (checkRight && currentPos.x > rightBoundary - objectWidth/2 - padding)
            {
               newPos.x = rightBoundary - objectWidth/2 - padding;
            }

            // Check bottom boundary
            if (checkBottom && currentPos.y < bottomBoundary + objectHeight/2 + padding)
            {
               newPos.y = bottomBoundary + objectHeight/2 + padding;
            }

            // Check top boundary
            if (checkTop && currentPos.y > topBoundary - objectHeight/2 - padding)
            {
               newPos.y = topBoundary - objectHeight/2 - padding;
            }

            // Teleport if position changed
            if (newPos != currentPos)
            {
                if (teleportTarget != null)
                {
                    teleportTarget.transform.position = newPos;     
                }
               
            }
       }
    }