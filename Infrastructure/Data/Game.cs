using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Infrastructure.Data
{
    public class Game
    {
        public long GameId { get; set; }
        public long HomeTeamId { get; set; }
        public Team HomeTeam { get; set; }
        public long AwayTeamId { get; set; }
        public Team AwayTeam { get; set; }
        public long HomeTeamScore { get; set; }
        public long AwayTeamScore { get; set; }
    }
}