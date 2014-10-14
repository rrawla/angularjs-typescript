module WebApp.Controllers {

    export class NewTeamController {
        
        self: NewTeamController = this;
        constructor(private $scope: IEditTeamControllerInterface,
            private dataService: WebApp.Services.DataService,
            private $location: ng.ILocationService) {
            $scope.PageTitle = "New Team";

            $scope.save = function() {
                dataService.addTeam($scope.currentTeam)
                    .then(() => {
                        $location.url("/teams");
                    });
            };
        }
    }
    NewTeamController.$inject = ["$scope", "dataService", "$location"];

} 