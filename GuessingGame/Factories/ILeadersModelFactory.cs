using GuessingGame.Models;
using System.Threading.Tasks;

namespace GuessingGame.Factories
{
    public interface ILeadersModelFactory
    {
        /// <summary>
        /// Creates LeadersModel
        /// </summary>
        /// <param name="playerName">Currently authorized player name</param>
        /// <param name="topCount">Number of leaders to expose</param>
        /// <returns></returns>
        Task<LeadersModel> GetLeadersModel(string playerName, int topCount);
    }
}