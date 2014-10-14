module WebApp.Services {
    export class GameService {
        games: Models.Game[];
        
        constructor(private $http: ng.IHttpService,
            private $q: ng.IQService,
            private dataService: DataService) {
            var self: GameService = this;
        }

        getGames(): Models.Game[] {
            var self: GameService = this;
            self.games = [];
            var easternTeams: Models.Team[] = [];
            var westernTeams: Models.Team[] = [];
            $.each(self.dataService.teams, function (i, item) {
                if (item.Division == "Eastern") {
                    easternTeams.push(item);
                } else {
                    westernTeams.push(item);
                }
            });

            $.each(easternTeams, function (i, item) {
                var newGame: Models.Game = new Models.Game();
                newGame.HomeTeam = item;
                newGame.AwayTeam = westernTeams[i];
                newGame.AwayTeamScore = 0;
                newGame.HomeTeamScore = 0;
                self.games.push(newGame);
            });

            return self.games;
        }
    }
    DataService.$inject = ["$http", "$q", "dataService"];
} 