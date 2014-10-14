interface SignalR {
    scoresHub: ScoresHub;
}

interface ScoresHub {
    client: ScoresHubClient;
}


interface ScoresHubClient {
    addPointsToTeam: (teamId: number, points: number) => void;
}

module WebApp.Controllers {
    export interface IScoreboardControllerScope extends IControllerScopeBase {
        games: Models.Game[];
    }


    
    export class ScoreboardController {
        
        constructor(private $scope: IScoreboardControllerScope,
            private dataService: WebApp.Services.DataService,
            private gameService: WebApp.Services.GameService) {
            var self: ScoreboardController = this;
            $scope.PageTitle = "Scoreboard";

            $scope.games = gameService.getGames();

            var hub = $.connection.scoresHub;
            hub.client.addPointsToTeam = (teamId: number, points: number) => {
                $.each($scope.games, function (i, game) {
                    if (game.HomeTeam.TeamId == teamId) {
                        $scope.$apply(() => {
                            game.HomeTeamScore += points;
                        });
                    }
                    if (game.AwayTeam.TeamId == teamId) {
                        $scope.$apply(() => {
                            game.AwayTeamScore += points;
                        });
                    }
                });

            };

            $.connection.hub.start();


       }
    }
    ScoreboardController.$inject = ["$scope", "dataService", "gameService"];
} 