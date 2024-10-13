using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;

    // Update is called once per frame
    void Update()
    {
        ClickTarget();
    }

    /// <summary>
    /// pick target to attack
    /// </summary>
    private void ClickTarget()
    {
        // raycast để lấy target cho Player
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            //512 hardcode cho layer Enemy
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);
            if (hit.collider != null)
            {
                if (hit.collider.tag == "Enemy")
                {
                    player.MyTarget = hit.transform;
                }
            }
            else
            {
                player.MyTarget = null;
            }
        }


    }
}
