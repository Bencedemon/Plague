using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Discord;

public class DiscordManager : MonoBehaviour
{
    private long applicationID = 1268189558659612725;
    [Space]
    public string details = "In Menu";
    public string state = "asd";
    [Space]
    public string largeImage = "inmenu";
    public string largeText = "Main Menu";

    [Space]
    public int partySize = 0;
    public int partyMax  = 4;

    private long time;

    public Discord.Discord discord;

    void Start()
    {
        // Log in with the Application ID
        discord = new Discord.Discord(applicationID, (System.UInt64)Discord.CreateFlags.NoRequireDiscord);

        time = System.DateTimeOffset.Now.ToUnixTimeMilliseconds();

        UpdateStatus();
    }

    void Update()
    {
        // Destroy the GameObject if Discord isn't running
        try
        {
            discord.RunCallbacks();
        }
        catch
        {
            Destroy(gameObject);
        }
    }

    void OnDisable(){
        discord.Dispose();
    }

    void LateUpdate() 
    {
        UpdateStatus();
    }

    void UpdateStatus()
    {
        // Update Status every frame
        try
        {
            var activityManager = discord.GetActivityManager();
            var activity = new Discord.Activity
            {
                Details = details,
                State = state,
                Assets = 
                {
                    LargeImage = largeImage,
                    LargeText = largeText
                },
                Timestamps =
                {
                    Start = time
                },
                /*Party = {
                    Id = "room123",
                    Size = {
                        CurrentSize = 0,
                        MaxSize = 5
                    }
                }*/
            };

            activityManager.UpdateActivity(activity, (res) =>
            {
                if (res != Discord.Result.Ok) Debug.LogWarning("Failed connecting to Discord!");
            });
        }
        catch
        {
            // If updating the status fails, Destroy the GameObject
            Destroy(gameObject);
        }
    }
}