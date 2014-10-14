var WebApp;
(function (WebApp) {
    (function (Modules) {
        function fantasyFootballModuleConfig($routeProvider) {
            var route = {
                controller: "WebApp.Controllers.TeamController",
                templateUrl: "/App/Templates/Team.html"
            };
            $routeProvider.when("/team/:id", route);
            route = {
                controller: "WebApp.Controllers.NewTeamController",
                templateUrl: "/App/Templates/Team.html"
            };
            $routeProvider.when("/newteam", route);
            route = {
                controller: "WebApp.Controllers.TeamsController",
                templateUrl: "/App/Templates/Teams.html"
            };
            $routeProvider.when("/teams", route);
            route = {
                controller: "WebApp.Controllers.ScoreboardController",
                templateUrl: "/App/Templates/Scoreboard.html"
            };
            $routeProvider.when("/scoreboard", route);

            route = {
                redirectTo: "/teams"
            };
            $routeProvider.otherwise(route);
        }
        fantasyFootballModuleConfig.$inject = ["$routeProvider"];

        function createDataService($http, $q) {
            return new WebApp.Services.DataService($http, $q);
        }
        createDataService.$inject = ["$http", "$q"];

        function createGameService($http, $q, dataService) {
            return new WebApp.Services.GameService($http, $q, dataService);
        }

        createGameService.$inject = ["$http", "$q", "dataService"];

        angular.module("fantasyFootball", ["ngRoute"]).config(fantasyFootballModuleConfig).factory("dataService", createDataService).factory("gameService", createGameService);
    })(WebApp.Modules || (WebApp.Modules = {}));
    var Modules = WebApp.Modules;
})(WebApp || (WebApp = {}));
//# sourceMappingURL=FantasyFootballModule.js.map
