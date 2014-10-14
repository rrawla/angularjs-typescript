var WebApp;
(function (WebApp) {
    (function (Controllers) {
        var ScoreboardController = (function () {
            function ScoreboardController($scope, dataService, gameService) {
                this.$scope = $scope;
                this.dataService = dataService;
                this.gameService = gameService;
                var self = this;
                $scope.PageTitle = "Scoreboard";

                $scope.games = gameService.getGames();

                var hub = $.connection.scoresHub;
                hub.client.addPointsToTeam = function (teamId, points) {
                    $.each($scope.games, function (i, game) {
                        if (game.HomeTeam.TeamId == teamId) {
                            $scope.$apply(function () {
                                game.HomeTeamScore += points;
                            });
                        }
                        if (game.AwayTeam.TeamId == teamId) {
                            $scope.$apply(function () {
                                game.AwayTeamScore += points;
                            });
                        }
                    });
                };

                $.connection.hub.start();
            }
            return ScoreboardController;
        })();
        Controllers.ScoreboardController = ScoreboardController;
        ScoreboardController.$inject = ["$scope", "dataService", "gameService"];
    })(WebApp.Controllers || (WebApp.Controllers = {}));
    var Controllers = WebApp.Controllers;
})(WebApp || (WebApp = {}));
//# sourceMappingURL=ScoreboardController.js.map
