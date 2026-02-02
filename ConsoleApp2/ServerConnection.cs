using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleApp2;

internal class ServerConnection
{

    HttpClient client = new HttpClient();
    string Token = String.Empty;
    public ServerConnection(string baseUrl)
    {
        client.BaseAddress = new Uri(baseUrl);

    }
    public async Task<bool> Register(string username, string password, string email, DateOnly birthDate)
    {
        try
        {
            var jsonData = new
            {
                username,
                password,
                email,
                birthDate
            };
            string jsonString = JsonSerializer.Serialize(jsonData);
            StringContent sendTs = new StringContent(jsonString, Encoding.UTF8);
            HttpResponseMessage response = await client.PostAsync("/register", sendTs);
            
            Message oneMsg = JsonSerializer.Deserialize<Message>(await response.Content.ReadAsStringAsync());
            response.EnsureSuccessStatusCode();
            

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
        return true;
    }
    public async Task<bool> Login(string name, string password)
    {
        try
        {
            var jsonData = new
            {
                name,
                password
            };
            string jsonString = JsonSerializer.Serialize(jsonData);
            StringContent sendTs = new StringContent(jsonString, Encoding.UTF8);
            HttpResponseMessage response = await client.PostAsync("/Login", sendTs);

            Message oneMsg = JsonSerializer.Deserialize<Message>(await response.Content.ReadAsStringAsync());
            response.EnsureSuccessStatusCode();

            Token = oneMsg.token;
            client.DefaultRequestHeaders.Add("authorization","Bearer " + Token);

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
        return true;
    }
    public async Task<bool> ChangePassword(string newPassword)
    {
        try
        {
            var jsonData = new
            {
                newPassword
            };
            string jsonString = JsonSerializer.Serialize(jsonData);
            StringContent sendTs = new StringContent(jsonString, Encoding.UTF8);
            HttpResponseMessage response = await client.PatchAsync("/profile", sendTs);

            Message oneMsg = JsonSerializer.Deserialize<Message>(await response.Content.ReadAsStringAsync());
            response.EnsureSuccessStatusCode();


        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
        return true;
    }
    public async Task<bool> ChangeUsername(string newUsername)
    {
        try
        {
            var jsonData = new
            {
                newUsername
            };
            string jsonString = JsonSerializer.Serialize(jsonData);
            StringContent sendTs = new StringContent(jsonString, Encoding.UTF8);
            HttpResponseMessage response = await client.PutAsync("/profile", sendTs);

            Message oneMsg = JsonSerializer.Deserialize<Message>(await response.Content.ReadAsStringAsync());
            response.EnsureSuccessStatusCode();


        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return false;
        }
        return true;
    }
    public async Task<List<Profile>> GetProfiles(string newUsername)
    {
        List<Profile> data = new List<Profile>();
        try
        {
            HttpResponseMessage response = await client.GetAsync("/profile");
            string responseString = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            data = JsonSerializer.Deserialize<List<Profile>>(responseString);

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return data;
        }
        return data;
    }
    public async Task DeleteUser()
    {
        try
        {
            HttpResponseMessage response = await client.DeleteAsync("/profile");

            Message oneMsg = JsonSerializer.Deserialize<Message>(await response.Content.ReadAsStringAsync());
            response.EnsureSuccessStatusCode();


        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
