var WebApp;
(function (WebApp) {
    (function (Services) {
        var GameService = (function () {
            function GameService($http, $q, dataService) {
                this.$http = $http;
                this.$q = $q;
                this.dataService = dataService;
                var self = this;
            }
            GameService.prototype.getGames = function () {
                var self = this;
                self.games = [];
                var easternTeams = [];
                var westernTeams = [];
                $.each(self.dataService.teams, function (i, item) {
                    if (item.Division == "Eastern") {
                        easternTeams.push(item);
                    } else {
                        westernTeams.push(item);
                    }
                });

                $.each(easternTeams, function (i, item) {
                    var newGame = new WebApp.Models.Game();
                    newGame.HomeTeam = item;
                    newGame.AwayTeam = westernTeams[i];
                    newGame.AwayTeamScore = 0;
                    newGame.HomeTeamScore = 0;
                    self.games.push(newGame);
                });

                return self.games;
            };
            return GameService;
        })();
        Services.GameService = GameService;
        Services.DataService.$inject = ["$http", "$q", "dataService"];
    })(WebApp.Services || (WebApp.Services = {}));
    var Services = WebApp.Services;
})(WebApp || (WebApp = {}));
//# sourceMappingURL=GameService.js.map
