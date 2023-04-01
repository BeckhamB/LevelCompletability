using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMovement : MonoBehaviour
{
    public float speedMultiplier = 1;
    static Animator anim;
    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }
    public void SetPath(Queue<Tile> path)
    {
        StopAllCoroutines();
        StartCoroutine(MoveAlongPath(path));
        
    }

    private IEnumerator MoveAlongPath(Queue<Tile> path)
    {
        yield return new WaitForSeconds(1f);
        Vector3 lastPosition = transform.position;
        while (path.Count > 0)
        {
            Tile nextTile = path.Dequeue();
            Vector3 newNextTile = new(nextTile.transform.position.x, nextTile.transform.position.y + 2f, nextTile.transform.position.z);
            float lerpValue = 0;
            transform.LookAt(newNextTile, Vector3.up);
            while (lerpValue < 1f)
            {
                lerpValue += Time.deltaTime * speedMultiplier;
                transform.position = Vector3.Lerp(lastPosition, newNextTile, lerpValue);
                if(anim != null)
                {
                    anim.SetBool("Walk Forward", true);
                }
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f / speedMultiplier);
            lastPosition = new Vector3(nextTile.transform.position.x, nextTile.transform.position.y + 2f, nextTile.transform.position.z);
        }
    }
}
