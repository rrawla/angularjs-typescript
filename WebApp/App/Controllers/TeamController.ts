module WebApp.Controllers {
    export interface IEditTeamControllerInterface extends IControllerScopeBase {
        currentTeam: Models.Team;
        save();
    }

    export interface IEditTeamRouteParamsInterface extends ng.route.IRouteParamsService {
        id: number;
    }

    export class TeamController {
        
        constructor(private $scope: IEditTeamControllerInterface,
            private $routeParams: IEditTeamRouteParamsInterface,
            private dataService: WebApp.Services.DataService,
            private $location: ng.ILocationService) {
            var self: TeamController = this;
            $scope.PageTitle = "Edit Team";

            $scope.currentTeam = dataService.getTeam($routeParams.id);

            $scope.save = function () {
                dataService.saveTeam($scope.currentTeam)
                    .then(() => {
                        $location.url("/teams");
                    });;
            };
        }
    }
    TeamController.$inject = ["$scope", "$routeParams", "dataService", "$location"];
} 