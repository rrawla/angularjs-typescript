var WebApp;
(function (WebApp) {
    (function (Controllers) {
        var NewTeamController = (function () {
            function NewTeamController($scope, dataService, $location) {
                this.$scope = $scope;
                this.dataService = dataService;
                this.$location = $location;
                this.self = this;
                $scope.PageTitle = "New Team";

                $scope.save = function () {
                    dataService.addTeam($scope.currentTeam).then(function () {
                        $location.url("/teams");
                    });
                };
            }
            return NewTeamController;
        })();
        Controllers.NewTeamController = NewTeamController;
        NewTeamController.$inject = ["$scope", "dataService", "$location"];
    })(WebApp.Controllers || (WebApp.Controllers = {}));
    var Controllers = WebApp.Controllers;
})(WebApp || (WebApp = {}));
//# sourceMappingURL=NewTeamController.js.map
