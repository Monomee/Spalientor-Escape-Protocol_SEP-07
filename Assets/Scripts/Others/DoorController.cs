using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Animator doorAnimator;
    const string CHARACTER_NEARBY = "character_nearby";
    Transform[] bot;
    Transform player;
    private void Start()
    {
        bot = GameObject.FindGameObjectsWithTag("bot")
            .Select(bot => bot.transform)
            .ToArray();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (bot.Any(b => Vector3.Distance(b.position, transform.position) < 2.5f) ||
            Vector3.Distance(player.position, transform.position) < 2.5f)
        {
            doorAnimator.SetBool(CHARACTER_NEARBY, true);
        }
        else
        {
            doorAnimator.SetBool(CHARACTER_NEARBY, false);
        }
    }
}
