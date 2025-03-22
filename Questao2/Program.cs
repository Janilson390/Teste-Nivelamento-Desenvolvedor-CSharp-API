using Questao2.Entities;

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class Program
{
    private static readonly HttpClient client = new HttpClient();

    public static async Task Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await GetTotalScoredGoals(teamName, year);
        Console.WriteLine($"Team {teamName} scored {totalGoals} goals in {year}");

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await GetTotalScoredGoals(teamName, year);
        Console.WriteLine($"Team {teamName} scored {totalGoals} goals in {year}");
    }

    public static async Task<int> GetTotalScoredGoals(string team, int year)
    {
        int totalGoals = 0;
        int page = 1;
        var response = await GetApiResponse($"https://jsonmock.hackerrank.com/api/football_matches?team1={team}&year={year}&page={page}");
        int paginas = response.Total_Pages;
        for (int i = 0; i < paginas; i++)
        {
            List<ApiResponse> lstApiResponse = new List<ApiResponse>();

            lstApiResponse.Add(await GetApiResponse($"https://jsonmock.hackerrank.com/api/football_matches?team1={team}&year={year}&page={i + 1}"));
            lstApiResponse.Add(await GetApiResponse($"https://jsonmock.hackerrank.com/api/football_matches?team2={team}&year={year}&page={i + 1}"));

            foreach (var apiResponse in lstApiResponse)
            {

                foreach (var match in apiResponse.Data)
                {
                    if (match.Team1 == team)
                    {
                        totalGoals += match.Team1Goals;
                    }
                    else if (match.Team2 == team)
                    {
                        totalGoals += match.Team2Goals;
                    }
                }
            }
        }
        return totalGoals;
    }

    public static async Task<ApiResponse> GetApiResponse(string url)
    {
        var response = await client.GetStringAsync(url);
        var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response);
        return apiResponse;
    }
}
