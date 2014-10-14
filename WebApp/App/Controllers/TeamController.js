var WebApp;
(function (WebApp) {
    (function (Controllers) {
        var TeamController = (function () {
            function TeamController($scope, $routeParams, dataService, $location) {
                this.$scope = $scope;
                this.$routeParams = $routeParams;
                this.dataService = dataService;
                this.$location = $location;
                var self = this;
                $scope.PageTitle = "Edit Team";

                $scope.currentTeam = dataService.getTeam($routeParams.id);

                $scope.save = function () {
                    dataService.saveTeam($scope.currentTeam).then(function () {
                        $location.url("/teams");
                    });
                    ;
                };
            }
            return TeamController;
        })();
        Controllers.TeamController = TeamController;
        TeamController.$inject = ["$scope", "$routeParams", "dataService", "$location"];
    })(WebApp.Controllers || (WebApp.Controllers = {}));
    var Controllers = WebApp.Controllers;
})(WebApp || (WebApp = {}));
//# sourceMappingURL=TeamController.js.map
