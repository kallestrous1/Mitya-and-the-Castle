using UnityEngine;

public class Boss : Enemy
{
    public bool fightStarted = false;
    public GameObject[] BossWalls;

    public void StartFight()
    {
        fightStarted = true;
        foreach(GameObject wall in BossWalls)
        {
            wall.SetActive(true);
        }
    }

    public void EndFight()
    {
        foreach (GameObject wall in BossWalls)
        {
            wall.SetActive(false);
        }
    }

/*    public override void UpdateTurnDirection()
    {
        if (target != null)
        {
            facingRight = target.transform.position.x > this.transform.position.x;
            if (facingRight && !flipped)
            {
                flipped = true;
                // this.transform.Rotate(0f, 180f, 0f);
                float xScale = this.transform.parent.localScale.x;
                this.transform.parent.localScale = new Vector3(xScale * -1f, 1f, 1f);
            }
            else if (!facingRight && flipped)
            {
                flipped = false;
                float xScale = this.transform.parent.localScale.x;
                this.transform.parent.localScale = new Vector3(xScale * -1f, 1f, 1f);

                // this.transform.Rotate(0f, -180f, 0f);
            }

        }
    }*/
}
