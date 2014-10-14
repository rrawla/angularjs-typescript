var WebApp;
(function (WebApp) {
    (function (Controllers) {
        var TeamsController = (function () {
            function TeamsController($scope, dataService, $location) {
                this.$scope = $scope;
                this.dataService = dataService;
                this.$location = $location;
                var self = this;
                $scope.PageTitle = "Teams";
                $scope.isBusy = true;
                dataService.getTeams().then(function (data) {
                    $scope.easternTeams = [];
                    $scope.westernTeams = [];

                    $.each(data, function (i, item) {
                        if (item.Division == "Eastern") {
                            $scope.easternTeams.push(item);
                        } else {
                            $scope.westernTeams.push(item);
                        }
                    });
                    $scope.isBusy = false;
                });

                $scope.editTeam = function (id) {
                    $location.url("/team/" + id);
                };
            }
            return TeamsController;
        })();
        Controllers.TeamsController = TeamsController;

        Controllers.TeamController.$inject = ["$scope", "$routeParams", "dataService", "$location"];
    })(WebApp.Controllers || (WebApp.Controllers = {}));
    var Controllers = WebApp.Controllers;
})(WebApp || (WebApp = {}));
//# sourceMappingURL=TeamsController.js.map
