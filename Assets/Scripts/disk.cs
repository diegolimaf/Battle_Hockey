using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class disk : MonoBehaviour
{
    float moveSpeed = 15.0f;
    private Vector2 diskDirection = Vector2.left;

    private float goalMontrealHeight, goalMontrealWidth, goalMontrealMaxX, goalMontrealMinX, goalMontrealMaxY, goalMontrealMinY, goalTorontoHeight, goalTorontoWidth, goalTorontoMaxX, goalTorontoMinX, goalTorontoMaxY, goalTorontoMinY, diskWidth, diskHeight;

    private GameObject goalMontreal, goalToronto, gameControl, montrealDefense, torontoDefense;

    private float bounceAngle, vx, vy;
    private float maxAngle = 45.0f;

    private float topWall = 10.45f;
    private float bottonWall = -10.45f;
    float toShrink = 1;

    int hits = 0;

    SpriteRenderer montrealRenderer, torontoRenderer;

    public Text hitsText;

    AudioSource audios;
    public AudioClip audioGoal, audioWall;

    bool collidedMontreal, collidedToronto, collidedWall, collidedDefenseToronto, collidedDefenseMontreal;

    void Start()
    {
        audios = GetComponent<AudioSource>();

        if (moveSpeed < 0)
            moveSpeed = -1 * moveSpeed;

        goalMontreal = GameObject.Find("GoalMontreal");
        goalToronto = GameObject.Find("GoalToronto");
        gameControl = GameObject.Find("Game");
        torontoDefense = GameObject.Find("torontoLine");
        montrealDefense = GameObject.Find("montrealLine");

        montrealRenderer = montrealDefense.GetComponent<SpriteRenderer>();
        torontoRenderer = torontoDefense.GetComponent<SpriteRenderer>();

        diskHeight = transform.GetComponent<Renderer>().bounds.size.y;
        diskWidth = transform.GetComponent<Renderer>().bounds.size.x;

        goalMontrealWidth = goalMontreal.transform.GetComponent<Renderer>().bounds.size.x;
        goalTorontoWidth = goalToronto.transform.GetComponent<Renderer>().bounds.size.x;
        goalMontrealHeight = goalMontreal.transform.GetComponent<Renderer>().bounds.size.y;
        goalTorontoHeight = goalToronto.transform.GetComponent<Renderer>().bounds.size.y;

        goalMontrealMaxX = goalMontreal.transform.localPosition.x - goalMontrealWidth / 2;
        goalMontrealMinX = goalMontreal.transform.localPosition.x + goalMontrealWidth / 2;

        goalTorontoMaxX = goalToronto.transform.localPosition.x + goalTorontoWidth / 2;
        goalTorontoMinX = goalToronto.transform.localPosition.x - goalTorontoWidth / 2;

        bounceAngle = GetBounceAngle();

        vx = moveSpeed * Mathf.Cos(bounceAngle);
        vy = moveSpeed * -Mathf.Sin(bounceAngle);

    }
    void Update()
    {
        Move();
    }
    bool Collision()
    {
        goalMontrealHeight = goalMontreal.transform.GetComponent<Renderer>().bounds.size.y;
        goalTorontoHeight = goalToronto.transform.GetComponent<Renderer>().bounds.size.y;

        goalMontrealMaxY = goalMontreal.transform.localPosition.y + goalMontrealHeight / 2;
        goalMontrealMinY = goalMontreal.transform.localPosition.y - goalMontrealHeight / 2;

        goalTorontoMaxY = goalToronto.transform.localPosition.y + goalTorontoHeight / 2;
        goalTorontoMinY = goalToronto.transform.localPosition.y - goalTorontoHeight / 2;

        // Check if disk hits left paddle
        if (transform.localPosition.x - diskWidth / 2 < goalTorontoMaxX && transform.localPosition.x > goalTorontoMinX)
        {
            if (transform.localPosition.y - diskHeight / 2 < goalTorontoMaxY && transform.localPosition.y + diskHeight / 2 > goalTorontoMinY)
            {
                diskDirection = Vector2.right;
                transform.localPosition = new Vector3(goalTorontoMaxX + diskWidth / 2, transform.localPosition.y, transform.localPosition.z);
                collidedToronto = true;
                audios.PlayOneShot(audioGoal);
                return true;
            }
        }
        // Check if disk hits right paddle
        if (transform.localPosition.x + diskWidth / 2 > goalMontrealMaxX && transform.localPosition.x < goalMontrealMinX)
        {
            if (transform.localPosition.y - diskHeight / 2 < goalMontrealMaxY && transform.localPosition.y + diskHeight / 2 > goalMontrealMinY)
            {
                diskDirection = Vector2.left;
                transform.localPosition = new Vector3(goalMontrealMaxX - diskWidth / 2, transform.localPosition.y, transform.localPosition.z);
                collidedMontreal = true;
                audios.PlayOneShot(audioGoal);
                return true;
            }
        }
        // check if disk hits top wall
        if (transform.localPosition.y > topWall)
        {
            transform.position = new Vector3(transform.localPosition.x, topWall, transform.localPosition.z);
            collidedWall = true;
            audios.PlayOneShot(audioWall);
            return true;
        }
        // check if disk hits bottom wall
        if (transform.localPosition.y < bottonWall)
        {
            transform.position = new Vector3(transform.localPosition.x, bottonWall, transform.localPosition.z);
            collidedWall = true;
            audios.PlayOneShot(audioWall);
            return true;
        }
        // check if disk hits defense line left
        if ((transform.localPosition.x < -18.6) && torontoRenderer.enabled)
        {
            diskDirection = Vector2.right;
            transform.position = new Vector3(-18.6f, transform.localPosition.y, transform.localPosition.z);
            collidedDefenseToronto = true;
            return true;
        }
        // check if disk hits defense line right
        if ((transform.localPosition.x > 18.6) && montrealRenderer.enabled)
        {
            diskDirection = Vector2.left;
            transform.position = new Vector3(18.6f, transform.localPosition.y, transform.localPosition.z);
            collidedDefenseMontreal = true;
            return true;
        }
        // check if goal left
        if (transform.localPosition.x < -21)
        {
            gameControl.GetComponent<gameControl>().MontrealPoint();
            toShrink = 1;
            goalMontreal.transform.localScale = new Vector3(toShrink, 0.3f, 1f);
            goalToronto.transform.localScale = new Vector3(toShrink, 0.3f, 1f);
            goalMontreal.GetComponent<rightPlayer>().UpdateHeight();
            goalToronto.GetComponent<leftPlayer>().UpdateHeight();
        }
        // check if goal right
        if (transform.localPosition.x > 21)
        {
            gameControl.GetComponent<gameControl>().TorontoPoint();
            toShrink = 1;
            goalMontreal.transform.localScale = new Vector3(toShrink, 0.3f, 1f);
            goalToronto.transform.localScale = new Vector3(toShrink, 0.3f, 1f);
            goalMontreal.GetComponent<rightPlayer>().UpdateHeight();
            goalToronto.GetComponent<leftPlayer>().UpdateHeight();
        }
        return false;
    }
    void Move()
    {
        if (!Collision())
        {
            vx = moveSpeed * Mathf.Cos(bounceAngle);
            if (moveSpeed > 0)
                vy = moveSpeed * -Mathf.Sin(bounceAngle);
            else
                vy = moveSpeed * Mathf.Sin(bounceAngle);

            transform.localPosition += new Vector3(diskDirection.x * vx * Time.deltaTime, vy * Time.deltaTime, 0);
        }
        else
        {
            if (moveSpeed < 0)
                moveSpeed = -1 * moveSpeed;

            if (collidedToronto)
            {
                collidedToronto = false;
                AddHits();
                float relativeY = goalToronto.transform.localPosition.y - transform.localPosition.y;
                float normalizedY = relativeY / (goalTorontoHeight / 2);

                bounceAngle = normalizedY * (maxAngle * Mathf.Deg2Rad);
            }
            else if (collidedMontreal)
            {
                collidedMontreal = false;
                AddHits();
                float relativeY = goalMontreal.transform.localPosition.y - transform.localPosition.y;
                float normalizedY = relativeY / (goalMontrealHeight / 2);

                bounceAngle = normalizedY * (maxAngle * Mathf.Deg2Rad);
            }
            else if (collidedWall)
            {
                collidedWall = false;
                bounceAngle = -bounceAngle;
            }
            else if (collidedDefenseToronto)
            {
                collidedDefenseToronto = false;
                torontoRenderer.enabled = false;
                bounceAngle = 0;
            }
            else if (collidedDefenseMontreal)
            {
                collidedDefenseMontreal = false;
                montrealRenderer.enabled = false;
                bounceAngle = 0;
            }
        }
    }

    float GetBounceAngle(float minDegree = 150, float maxDegree = 210)
    {
        float minRadian = minDegree * Mathf.PI / 180;
        float maxRadian = maxDegree * Mathf.PI / 180;

        return Random.Range(minRadian, maxRadian);
    }
    void AddHits()
    {
        hits++;
        if (hits % 5 == 0)
        {
            moveSpeed += 2f;
        }
        if (hits % 10 == 0)
        {
            toShrink *= 0.85f;
            goalMontreal.transform.localScale = new Vector3(toShrink, 0.3f, 1f);
            goalToronto.transform.localScale = new Vector3(toShrink, 0.3f, 1f);
            goalMontreal.GetComponent<rightPlayer>().UpdateHeight();
            goalToronto.GetComponent<leftPlayer>().UpdateHeight();
        }
        if (hits % 20 == 0)
        {
            goalToronto.GetComponent<leftPlayer>().IncreaseSpeed();
            goalMontreal.GetComponent<rightPlayer>().IncreaseSpeed();
        }
        hitsText.text = "HITs:   " + hits.ToString();
    }
}
