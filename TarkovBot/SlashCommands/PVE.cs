
using System.Globalization;
using NetCord;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;
using TarkovBot.Database.Controllers;
using TarkovBot.Database.DTOs;

namespace TarkovBot.SlashCommands;

public class PVE:ApplicationCommandModule<ApplicationCommandContext>
{
    private readonly TeamKillsController _teamKillsController;

    public PVE(TeamKillsController teamKillsController)
    {
        _teamKillsController = teamKillsController;
    }

    [SlashCommand("teamkills", "PVE Teamkill Leaderboard")]
    public async Task TeamkillLeaderboard()
    {
        await Context.Interaction.SendResponseAsync(InteractionCallback.DeferredMessage());
        
        try
        {
            var results = await _teamKillsController.GetTeamKillsAsync(Context.Guild.Id);
            results = results.OrderByDescending(x => x.Kills.Count).ToList();
            if (!results.Any())
            {
                await Context.Interaction.ModifyResponseAsync(options =>
                {
                    options.Content = "Leaderboard is empty.";
                });
                return;
            }            
            
            
            var embeds = new List<EmbedProperties>();
            var titleEmbed = new EmbedProperties()
            {
                Title = "💀 PVE Teamkill Leaderboard 💀",
                Color = new Color(71, 76, 72),
                Timestamp = DateTime.Now,
            };
            embeds.Add(titleEmbed);
            
            var leaderboardCounter = 1;
            foreach (var res in results)
            {
                var killer = await Context.Client.Rest.GetUserAsync(res.KillerId);
                
                var medals = new Dictionary<int, string>
                {
                    { 1, ":first_place:" },
                    { 2, ":second_place:" },
                    { 3, ":third_place:" }
                };
                titleEmbed.Description += $"{(leaderboardCounter>3?leaderboardCounter+".": medals[leaderboardCounter])} {killer.GlobalName} - {res.Kills.Count} {(res.Kills.Count == 1 ? "kill" : "kills")}\n";
                leaderboardCounter++;
                
                var embed = new EmbedProperties()
                {
                    Author = new EmbedAuthorProperties()
                    {
                        Name = killer.GlobalName,
                        IconUrl = killer.GetAvatarUrl()?.ToString()??null,
                    },
                    Color = killer.AccentColor?? new Color(255, 255, 255),
                };
                
                var counter = 1;
                foreach (var kill in res.Kills)
                {
                    var killed = await Context.Client.Rest.GetUserAsync(kill.KilledId);
                    embed.AddFields(new EmbedFieldProperties()
                    {
                        Name = $"{counter++}. Killed {killed.GlobalName}",
                        Value =
                            $"{kill.Description}\n{kill.Date?.ToString("dd-MM-yyyy")}"
                    });
                }
                embeds.Add(embed);
            }
            await Context.Interaction.ModifyResponseAsync(options =>
            {
                options.Embeds = embeds;
            });
        }
        catch (Exception e)
        {
            await Context.Interaction.ModifyResponseAsync(options =>
            {
                options.Content = $"Couldn't get teamkill leaderboard {e.Message}";
            });
        }
    }

    [SlashCommand("addteamkill", "Add Teamkill to the Leaderboard")]
    public async Task AddTeamKill(
        [SlashCommandParameter(Name = "killer")] User killer,
        [SlashCommandParameter(Name = "killed")] User killed,
        [SlashCommandParameter(Name = "description",Description = "Short, optional description")] string? description = null,
        [SlashCommandParameter(Name = "date",Description = "Optional date of the incident ('DD-MM-YYYY').")] string? date = null
        )
    {
        await Context.Interaction.SendResponseAsync(InteractionCallback.DeferredMessage());
        DateTime? parsedDate = null;
        try
        {
            if (DateTime.TryParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime tempParsedDate))
            {
                parsedDate = tempParsedDate;
            }
        }
        catch(Exception e)
        {
            await Context.Interaction.ModifyResponseAsync(options =>
            {
                options.Content = "Cannot parse date,try again.";
            });
        }

        try
        {
            var newKill = new AddTeamKill()
            {
                KillerId = killer.Id,
                KilledId = killed.Id,
                Description = description,
                Date = parsedDate
            };
            await _teamKillsController.AddTeamKillAsync(Context.Guild.Id, newKill);
            await Context.Interaction.ModifyResponseAsync(options =>
            {
                options.Content = $"Successfully added {killer} teamkill to the leaderboard";
            });
        }
        catch (Exception e)
        {
            await Context.Interaction.ModifyResponseAsync(options =>
            {
                options.Content = "Couldn't add teamkill to the leaderboard.";
            });
        }
        
    }
}